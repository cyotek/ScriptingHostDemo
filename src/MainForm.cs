using Cyotek.Demo.ScriptingHost;
using Cyotek.Demo.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace Cyotek.Demo
{
  internal partial class MainForm : BaseForm
  {
    private Dictionary<Color, Brush> _brushCache;

    private string _fileName;

    private PixelPicture _picture;

    private SampleScriptEnvironment _scriptEnvironment;

    public MainForm()
    {
      this.InitializeComponent();

      _brushCache = new Dictionary<Color, Brush>();
    }

    protected override void OnShown(EventArgs e)
    {
      _picture = new PixelPicture
      {
        Width = 16,
        Height = 16
      };

      _scriptEnvironment = new SampleScriptEnvironment(logTextBox);
      _scriptEnvironment.AddType("color", typeof(Color));
      _scriptEnvironment.AddValue("picture", _picture);

      base.OnShown(e);

      this.ProcessCommandLine();
    }

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      AboutDialog.ShowAboutDialog();
    }

    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.PerformClipboardAction(tb => tb.Copy());
    }

    private void CutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.PerformClipboardAction(tb => tb.Cut());
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
        scriptTextBox.Text = File.ReadAllText(fileName);
        _fileName = fileName;

        this.RunScript();
        this.UpdateUi();
      }
      catch (Exception ex)
      {
        MessageBox.Show(string.Format("Failed to open file. {0}", ex.GetBaseException().Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

      if (args.Length == 2)
      {
        this.OpenFile(args[1]);
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
      _scriptEnvironment.WrappedExecute(scriptTextBox.Text);

      renderPanel.Invalidate();
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
        MessageBox.Show(string.Format("Failed to save file. {0}", ex.GetBaseException().Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private void UpdateUi()
    {
      //this.LoadFields();
      //this.UpdateSimulationControls();
      //this.UpdateStatusBar();
      this.UpdateWindowTitle();

      renderPanel.Invalidate();
    }

    private void UpdateWindowTitle()
    {
      this.Text = string.Format("{1} - {0}", Application.ProductName, string.IsNullOrEmpty(_fileName) ? "Untitled" : Path.GetFileName(_fileName));
    }
  }
}