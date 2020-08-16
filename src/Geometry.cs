using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Cyotek.Demo.ScriptingHost
{
  internal static class Geometry
  {
    #region Public Methods

    public static IEnumerable<Point> GetCirclePoints(int cx, int cy, int r)
    {
      // Derived from http://members.chello.at/easyfilter/bresenham.html
      int x = -r, y = 0, err = 2 - 2 * r; /* II. Quadrant */
      do
      {
        yield return new Point(cx - x, cy + y); /*   I. Quadrant */
        yield return new Point(cx - y, cy - x); /*  II. Quadrant */
        yield return new Point(cx + x, cy - y); /* III. Quadrant */
        yield return new Point(cx + y, cy + x); /*  IV. Quadrant */
        r = err;
        if (r <= y) err += ++y * 2 + 1;           /* e_xy+e_y < 0 */
        if (r > x || err > y) err += ++x * 2 + 1; /* e_xy+e_x > 0 or no 2nd y-step */
      } while (x < 0);
    }

    public static IEnumerable<Point> GetEllipsePoints(int x0, int y0, int x1, int y1)
    {
      // Derived from http://members.chello.at/easyfilter/bresenham.html
      int a = Math.Abs(x1 - x0), b = Math.Abs(y1 - y0), b1 = b & 1; /* values of diameter */
      long dx = 4 * (1 - a) * b * b, dy = 4 * (b1 + 1) * a * a; /* error increment */
      long err = dx + dy + b1 * a * a, e2; /* error of 1.step */

      if (x0 > x1) { x0 = x1; x1 += a; } /* if called with swapped points */
      if (y0 > y1) y0 = y1; /* .. exchange them */
      y0 += (b + 1) / 2; y1 = y0 - b1;   /* starting pixel */
      a *= 8 * a; b1 = 8 * b * b;

      do
      {
        yield return new Point(x1, y0); /*   I. Quadrant */
        yield return new Point(x0, y0); /*  II. Quadrant */
        yield return new Point(x0, y1); /* III. Quadrant */
        yield return new Point(x1, y1); /*  IV. Quadrant */
        e2 = 2 * err;
        if (e2 <= dy) { y0++; y1--; err += dy += a; }  /* y step */
        if (e2 >= dx || 2 * err > dy) { x0++; x1--; err += dx += b1; } /* x step */
      } while (x0 <= x1);

      while (y0 - y1 < b)
      {  /* too early stop of flat ellipses a=1 */
        yield return new Point(x0 - 1, y0); /* -> finish tip of ellipse */
        yield return new Point(x1 + 1, y0++);
        yield return new Point(x0 - 1, y1);
        yield return new Point(x1 + 1, y1--);
      }
    }

    public static IEnumerable<Point> GetLinePoints(int x0, int y0, int x1, int y1)
    {
      // Derived from http://members.chello.at/easyfilter/bresenham.html
      int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
      int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
      int err = dx + dy, e2; /* error value e_xy */

      for (; ; )
      {  /* loop */
        yield return (new Point(x0, y0));
        if (x0 == x1 && y0 == y1) break;
        e2 = 2 * err;
        if (e2 >= dy) { err += dy; x0 += sx; } /* e_xy+e_x > 0 */
        if (e2 <= dx) { err += dx; y0 += sy; } /* e_xy+e_y < 0 */
      }
    }

    public static IEnumerable<Point> GetQuadraticBezierCurvePoints(int x0, int y0, int x1, int y1, int x2, int y2)
    {
      // Derived from http://members.chello.at/easyfilter/bresenham.html
      int sx = x2 - x1, sy = y2 - y1;
      long xx = x0 - x1, yy = y0 - y1, xy;         /* relative values for checks */
      double dx, dy, err, cur = xx * sy - yy * sx;                    /* curvature */

      Debug.Assert(xx * sx <= 0 && yy * sy <= 0);  /* sign of gradient must not change */

      if (sx * (long)sx + sy * (long)sy > xx * xx + yy * yy)
      { /* begin with longer part */
        x2 = x0; x0 = sx + x1; y2 = y0; y0 = sy + y1; cur = -cur;  /* swap P0 P2 */
      }
      if (cur != 0)
      {                                    /* no straight line */
        xx += sx; xx *= sx = x0 < x2 ? 1 : -1;           /* x step direction */
        yy += sy; yy *= sy = y0 < y2 ? 1 : -1;           /* y step direction */
        xy = 2 * xx * yy; xx *= xx; yy *= yy;          /* differences 2nd degree */
        if (cur * sx * sy < 0)
        {                           /* negated curvature? */
          xx = -xx; yy = -yy; xy = -xy; cur = -cur;
        }
        dx = 4.0 * sy * cur * (x1 - x0) + xx - xy;             /* differences 1st degree */
        dy = 4.0 * sx * cur * (y0 - y1) + yy - xy;
        xx += xx; yy += yy; err = dx + dy + xy;                /* error 1st step */
        do
        {
          bool b;
          yield return new Point(x0, y0);                                     /* plot curve */
          if (x0 == x2 && y0 == y2) break;  /* last pixel -> curve finished */
          b = 2 * err < dx;                  /* save value for test of y step */
          if (2 * err > dy) { x0 += sx; dx -= xy; err += dy += yy; } /* x step */
          if (b) { y0 += sy; dy -= xy; err += dx += xx; } /* y step */
        } while (dy < dx);           /* gradient negates -> algorithm fails */
      }

      foreach (Point point in Geometry.GetLinePoints(x0, y0, x2, y2))
      {
        yield return point;                  /* plot remaining part to end */
      }
    }

    #endregion Public Methods
  }
}