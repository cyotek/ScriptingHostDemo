using System.Collections.Generic;
using System.Drawing;

// Adding Scripting to .NET Applications
// https://www.cyotek.com/blog/adding-scripting-to-net-applications

// Copyright © 2020 Cyotek Ltd. All Rights Reserved.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this example useful?
// https://www.paypal.me/cyotek

namespace Cyotek.Demo.ScriptingHost
{
  internal class PixelPicture
  {
    #region Private Fields

    private Color _fillColor;

    private int _height;

    private Color _lineColor;

    private Color[] _pixels;

    private int _width;

    #endregion Private Fields

    #region Public Constructors

    public PixelPicture()
    {
      _width = 16;
      _height = 16;
      _pixels = new Color[_width * _height];
      _fillColor = Color.White;
      _lineColor = Color.Black;
    }

    #endregion Public Constructors

    #region Public Properties

    public Color FillColor
    {
      get { return _fillColor; }
      set { _fillColor = value; }
    }

    public int Height
    {
      get { return _height; }
      set { this.AssignSize(ref _height, value); }
    }

    public int Length
    {
      get { return _width * _height; }
    }

    public Color LineColor
    {
      get { return _lineColor; }
      set { _lineColor = value; }
    }

    public int Width
    {
      get { return _width; }
      set { this.AssignSize(ref _width, value); }
    }

    #endregion Public Properties

    #region Public Indexers

    public Color this[int index]
    {
      get { return _pixels[index]; }
      set { _pixels[index] = value; }
    }

    public Color this[int x, int y]
    {
      get { return this[(y * _width) + x]; }
      set { this[(y * _width) + x] = value; }
    }

    #endregion Public Indexers

    #region Public Methods

    public void Clear()
    {
      this.Clear(_fillColor);
    }

    public void Clear(Color color)
    {
      for (int i = 0; i < _pixels.Length; i++)
      {
        _pixels[i] = color;
      }
    }

    public void DrawCircle(int cx, int cy, int radius)
    {
      this.DrawCircle(cx, cy, radius, _lineColor);
    }

    public void DrawCircle(int cx, int cy, int radius, Color color)
    {
      foreach (Point point in Geometry.GetCirclePoints(cx, cy, radius))
      {
        this.SetPixel(point, color);
      }
    }

    public void DrawCurve(int x0, int y0, int x1, int y1, int x2, int y2)
    {
      this.DrawCurve(x0, y0, x1, y1, x2, y2, _lineColor); ;
    }

    public void DrawCurve(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
    {
      foreach (Point point in Geometry.GetQuadraticBezierCurvePoints(x0, y0, x1, y1, x2, y2))
      {
        this.SetPixel(point, color);
      }
    }

    public void DrawEllipse(int x, int y, int w, int h)
    {
      this.DrawEllipse(x, y, w, h, _lineColor);
    }

    public void DrawEllipse(int x, int y, int w, int h, Color color)
    {
      foreach (Point point in Geometry.GetEllipsePoints(x, y, x + w, y + h))
      {
        this.SetPixel(point, color);
      }
    }

    public void DrawLine(int x1, int y1, int x2, int y2)
    {
      this.DrawLine(x1, y1, x2, y2, _lineColor);
    }

    public void DrawLine(int x1, int y1, int x2, int y2, Color color)
    {
      foreach (Point point in Geometry.GetLinePoints(x1, y1, x2, y2))
      {
        this.SetPixel(point, color);
      }
    }

    public void DrawRectangle(int x, int y, int width, int height)
    {
      this.DrawRectangle(x, y, width, height, _lineColor);
    }

    public void DrawRectangle(int x, int y, int width, int height, Color color)
    {
      this.DrawLine(x, y, x + width, y, color);
      this.DrawLine(x + width, y, x + width, y + height, color);
      this.DrawLine(x + width, y + height, x, y + height, color);
      this.DrawLine(x, y + height, x, y, color);
    }

    public void FillRectangle(int x, int y, int width, int height)
    {
      this.FillRectangle(x, y, width, height, _fillColor);
    }

    public void FillRectangle(int x, int y, int width, int height, Color color)
    {
      for (int r = 0; r <= height; r++)
      {
        this.DrawLine(x, y + r, x + width, y + r, color);
      }
    }

    public void FloodFill(int x, int y)
    {
      this.FloodFill(x, y, _lineColor, _fillColor);
    }

    public void FloodFill(int x, int y, Color targetColor, Color replacementColor)
    {
      Queue<Point> q = new Queue<Point>();

      // Derived from https://rosettacode.org/wiki/Bitmap/Flood_fill#C.23

      q.Enqueue(new Point(x, y));
      while (q.Count > 0)
      {
        Point n = q.Dequeue();
        if (!ColorMatch(this.GetPixel(n.X, n.Y), targetColor))
          continue;
        Point w = n, e = new Point(n.X + 1, n.Y);
        while ((w.X >= 0) && ColorMatch(this.GetPixel(w.X, w.Y), targetColor))
        {
          this.SetPixel(w.X, w.Y, replacementColor);
          if ((w.Y > 0) && ColorMatch(this.GetPixel(w.X, w.Y - 1), targetColor))
            q.Enqueue(new Point(w.X, w.Y - 1));
          if ((w.Y < _height - 1) && ColorMatch(this.GetPixel(w.X, w.Y + 1), targetColor))
            q.Enqueue(new Point(w.X, w.Y + 1));
          w.X--;
        }
        while ((e.X <= _width - 1) && ColorMatch(this.GetPixel(e.X, e.Y), targetColor))
        {
          this.SetPixel(e.X, e.Y, replacementColor);
          if ((e.Y > 0) && ColorMatch(this.GetPixel(e.X, e.Y - 1), targetColor))
            q.Enqueue(new Point(e.X, e.Y - 1));
          if ((e.Y < _height - 1) && ColorMatch(this.GetPixel(e.X, e.Y + 1), targetColor))
            q.Enqueue(new Point(e.X, e.Y + 1));
          e.X++;
        }
      }
    }

    public Color GetPixel(int x, int y)
    {
      return this[x, y];
    }

    public Color GetPixel(int index)
    {
      return this[index];
    }

    public void Plot(int x, int y)
    {
      this.SetPixel(x, y, _lineColor);
    }

    public void Plot(int x, int y, Color color)
    {
      this.SetPixel(x, y, color);
    }

    public void SetPixel(Point point, Color color)
    {
      this.SetPixel(point.X, point.Y, color);
    }

    public void SetPixel(int x, int y, Color color)
    {
      if (x >= 0 && x < _width && y >= 0 && y < _height)
      {
        this[x, y] = color;
      }
    }

    public void SetPixel(int index, Color color)
    {
      if (index >= 0 && index < this.Length)
      {
        this[index] = color;
      }
    }

    #endregion Public Methods

    #region Private Methods

    private static bool ColorMatch(Color a, Color b)
    {
      return (a.ToArgb() & 0xffffff) == (b.ToArgb() & 0xffffff);
    }

    private void AssignSize(ref int size, int value)
    {
      size = value;

      _pixels = new Color[_width * _height];
    }

    #endregion Private Methods
  }
}