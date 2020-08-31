using Cyotek.Demo.ScriptingHost;
using Cyotek.Demo.Windows.Forms;
using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

// Adding Scripting to .NET Applications
// https://www.cyotek.com/blog/adding-scripting-to-net-applications

// Copyright © 2020 Cyotek Ltd. All Rights Reserved.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this example useful?
// https://www.paypal.me/cyotek

namespace Cyotek.Demo
{
  internal partial class MainForm : BaseForm
  {
    #region Private Fields

    private SampleApplication _application;

    private Dictionary<Color, Brush> _brushCache;

    private string _fileName;

    private PixelPicture _picture;

    private SampleScriptEnvironment _scriptEnvironment;

    #endregion Private Fields

    #region Public Constructors

    public MainForm()
    {
      this.InitializeComponent();

      _brushCache = new Dictionary<Color, Brush>();
    }

    #endregion Public Constructors

    #region Internal Methods

    internal void UpdateWindowTitle()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new MethodInvoker(this.UpdateWindowTitle));
      }
      else
      {
        this.Text = string.Format("{1} - {0}", _application.Title, string.IsNullOrEmpty(_fileName) ? "Untitled" : Path.GetFileName(_fileName));
      }
    }

    #endregion Internal Methods

    #region Protected Methods

    protected override void OnShown(EventArgs e)
    {
      _application = new SampleApplication(this);

      _picture = new PixelPicture
      {
        Width = 16,
        Height = 16
      };

      _scriptEnvironment = new SampleScriptEnvironment(logTextBox);
      _scriptEnvironment.AddType("color", typeof(Color));
      _scriptEnvironment.AddValue("picture", _picture);
      _scriptEnvironment.AddValue("application", _application);

      base.OnShown(e);

      this.ProcessCommandLine();
    }

    #endregion Protected Methods

    #region Private Methods

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      AboutDialog.ShowAboutDialog();
    }

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      _scriptEnvironment.WrappedExecute(scriptTextBox.Text);
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.SetStatus(string.Empty);

      renderPanel.Invalidate();
    }

    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.PerformClipboardAction(tb => tb.Copy());
    }

    private void CutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.PerformClipboardAction(tb => tb.Cut());
    }

    private void CyotekLinkToolStripStatusLabel_Click(object sender, EventArgs e)
    {
      AboutDialog.OpenCyotekHomePage();

      cyotekLinkToolStripStatusLabel.LinkVisited = true;
    }

    private void DrawGrid(Graphics g, float scale, float w, float h)
    {
      if (scale > 4 && gridLinesToolStripMenuItem.Checked)
      {
        Pen pen;

        pen = Pens.Black;

        g.DrawRectangle(pen, 0, 0, w, h);

        for (int r = 1; r < _picture.Height; r++)
        {
          g.DrawLine(pen, 0, r * scale, w, r * scale);
        }

        for (int c = 1; c < _picture.Width; c++)
        {
          g.DrawLine(pen, c * scale, 0, c * scale, h);
        }
      }
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private Control GetActiveControl(Control control)
    {
      Control activeControl;
      ContainerControl containerControl;
      TabControl tabControl;

      containerControl = control as ContainerControl;
      tabControl = control as TabControl;

      if (containerControl != null)
      {
        activeControl = this.GetActiveControl(containerControl.ActiveControl);
      }
      else if (tabControl?.SelectedTab?.Controls.Count == 1)
      {
        activeControl = tabControl.SelectedTab.Controls[0];
      }
      else
      {
        activeControl = control;
      }

      return activeControl;
    }

    private Brush GetBrush(Color color)
    {
      if (!_brushCache.TryGetValue(color, out Brush value))
      {
        value = new SolidBrush(color);

        _brushCache.Add(color, value);
      }

      return value;
    }

    private void GridLinesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      gridLinesToolStripMenuItem.Checked = !gridLinesToolStripMenuItem.Checked;

      renderPanel.Invalidate();
    }

    private bool IsImageFile(string fileName, out FastBitmap image)
    {
      // TODO: Better to use FreeImage or something that will
      // detect an image format by reading the first few bytes

      try
      {
        image = FastBitmap.FromFile(fileName);
      }
      catch
      {
        image = null;
      }

      return image != null;
    }

    private void LoadImage(FastBitmap image)
    {
      _picture.Width = image.Width;
      _picture.Height = image.Height;

      for (int y = 0; y < image.Height; y++)
      {
        for (int x = 0; x < image.Height; x++)
        {
          _picture.SetPixel(x, y, image[x, y]);
        }
      }
    }

    private void NewFile()
    {
      scriptTextBox.Text = string.Empty;

      _picture.Width = 16;
      _picture.Height = 16;
      _picture.Clear();

      _fileName = null;
      this.UpdateUi();
    }

    private void NewToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.NewFile();
    }

    private void OpenFile()
    {
      string fileName;

      fileName = FileDialogHelper.GetOpenFileName("Open Script", Filters.General, "js");

      if (!string.IsNullOrEmpty(fileName))
      {
        this.OpenFile(fileName);
      }
    }

    private void OpenFile(string fileName)
    {
      try
      {
        if (this.IsImageFile(fileName, out FastBitmap image))
        {
          this.LoadImage(image);
          image.Dispose();
          _fileName = null;
        }
        else
        {
          scriptTextBox.Text = File.ReadAllText(fileName);
          _fileName = fileName;
        }

        this.UpdateUi();
      }
      catch (Exception ex)
      {
        MessageBox.Show(string.Format("Failed to open file. {0}", ex.GetBaseException().Message), _application.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.OpenFile();
    }

    private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.PerformClipboardAction(tb => tb.Paste());
    }

    private void PerformClipboardAction(Action<TextBoxBase> action)
    {
      Control control;

      control = this.GetActiveControl(this);

      if (control is TextBoxBase textBox)
      {
        action(textBox);
      }
      else
      {
        SystemSounds.Beep.Play();
      }
    }

    private void ProcessCommandLine()
    {
      string[] args;

      this.NewFile();

      args = Environment.GetCommandLineArgs();

      if (args.Length > 1)
      {
        for (int i = 1; i < args.Length; i++)
        {
          this.OpenFile(args[i]);
        }
      }
    }

    private void RenderPanel_Paint(object sender, PaintEventArgs e)
    {
      Graphics g;
      Size size;
      float scale;
      float w;
      float h;

      g = e.Graphics;
      size = renderPanel.ClientSize;
      scale = Math.Min(size.Width - 1, size.Height - 1) / (float)Math.Max(_picture.Width, _picture.Height);

      w = _picture.Width * scale;
      h = _picture.Height * scale;

      g.FillRectangle(Brushes.White, 0, 0, w, h);

      for (int r = 0; r < _picture.Height; r++)
      {
        for (int c = 0; c < _picture.Width; c++)
        {
          Brush brush;
          float x;
          float y;

          brush = this.GetBrush(_picture[c, r]);
          x = c * scale;
          y = r * scale;

          g.FillRectangle(brush, x, y, scale, scale);
        }
      }

      this.DrawGrid(g, scale, w, h);
    }

    private void RunScript()
    {
      this.SetStatus("Running script...");

      backgroundWorker.RunWorkerAsync();
    }

    private void RunToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.RunScript();
    }

    private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SaveFileAs();
    }

    private void SaveFile()
    {
      if (string.IsNullOrEmpty(_fileName))
      {
        this.SaveFileAs();
      }
      else
      {
        this.SaveFile(_fileName);
      }
    }

    private void SaveFile(string fileName)
    {
      try
      {
        File.WriteAllText(fileName, scriptTextBox.Text);
        _fileName = fileName;

        this.UpdateUi();
      }
      catch (Exception ex)
      {
        MessageBox.Show(string.Format("Failed to save file. {0}", ex.GetBaseException().Message), _application.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void SaveFileAs()
    {
      string fileName;

      fileName = FileDialogHelper.GetSaveFileName("Save Script As", Filters.ScriptFiles, "js", _fileName);

      if (!string.IsNullOrEmpty(fileName))
      {
        this.SaveFile(fileName);
      }
    }

    private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SaveFile();
    }

    private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.PerformClipboardAction(tb => tb.SelectAll());
    }

    private void SetStatus(string message)
    {
      Cursor.Current = string.IsNullOrEmpty(message) ? Cursors.Default : Cursors.WaitCursor;

      statusToolStripStatusLabel.Text = message;
    }

    private void UpdateStatusBar()
    {
      widthToolStripStatusLabel.Text = string.Format("Width: {0}px", _picture.Width);
      heightToolStripStatusLabel.Text = string.Format("Height: {0}px", _picture.Width);
    }

    private void UpdateUi()
    {
      this.UpdateStatusBar();
      this.UpdateWindowTitle();

      renderPanel.Invalidate();
    }

    #endregion Private Methods
  }
}