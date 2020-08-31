using Cyotek.Demo.Windows.Forms;
using System;
using System.ComponentModel;
using System.Windows.Forms;

// Adding Scripting to .NET Applications
// https://www.cyotek.com/blog/adding-scripting-to-net-applications

// Copyright © 2020 Cyotek Ltd. All Rights Reserved.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this example useful?
// https://www.paypal.me/cyotek

namespace Cyotek.Windows.Forms
{
  internal partial class InputDialog : BaseForm
  {
    #region Private Fields

    private string _footerText;

    private string _promptText;

    private string _value;

    #endregion Private Fields

    #region Public Constructors

    public InputDialog()
    {
      this.InitializeComponent();
    }

    #endregion Public Constructors

    #region Public Properties

    public string FooterText
    {
      get { return _footerText; }
      set { _footerText = value; }
    }

    [DefaultValue(false)]
    public bool Multiline
    {
      get { return inputTextBox.Multiline; }
      set { inputTextBox.Multiline = value; }
    }

    [DefaultValue("")]
    public string PromptText
    {
      get { return _promptText; }
      set { _promptText = value; }
    }

    public Func<string, bool> ValidationCallback { get; set; }

    [DefaultValue("")]
    public string Value
    {
      get { return _value; }
      set { _value = value; }
    }

    #endregion Public Properties

    #region Public Methods

    public static string ShowInputDialog(string promptText)
    {
      return ShowInputDialog(promptText, string.Empty, string.Empty);
    }

    public static string ShowInputDialog(IWin32Window owner, string promptText)
    {
      return ShowInputDialog(owner, promptText, string.Empty);
    }

    public static string ShowInputDialog(string promptText, string caption, string defaultValue)
    {
      return ShowInputDialog(null, promptText, caption, defaultValue);
    }

    public static string ShowInputDialog(string promptText, string caption)
    {
      return ShowInputDialog(promptText, caption, string.Empty);
    }

    public static string ShowInputDialog(string promptText, string caption, Func<string, bool> validationCallback)
    {
      return ShowInputDialog(null, promptText, caption, string.Empty, validationCallback);
    }

    public static string ShowInputDialog(IWin32Window owner, string promptText, string caption)
    {
      return ShowInputDialog(owner, promptText, caption, string.Empty);
    }

    public static string ShowInputDialog(IWin32Window owner, string promptText, string caption, string defaultValue)
    {
      return ShowInputDialog(owner, promptText, caption, defaultValue, null);
    }

    public static string ShowInputDialog(IWin32Window owner, string promptText, string caption, string defaultValue, Func<string, bool> validationCallback)
    {
      string result;

      using (InputDialog dialog = new InputDialog
      {
        Text = caption,
        PromptText = promptText,
        Value = defaultValue,
        ValidationCallback = validationCallback
      })
      {
        result = dialog.ShowDialog(owner) == DialogResult.OK ? dialog.Value : null;
      }

      return result;
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnLoad(EventArgs e)
    {
      int height;

      base.OnLoad(e);

      inputTextBox.Text = _value;

      promptLabel.Text = _promptText;

      if (!string.IsNullOrEmpty(_footerText))
      {
        footerLabel.Text = _footerText;
        footerLabel.Visible = true;
      }

      //if (!this.IsDesignTime() && TranslationProvider.LanguageFoldersPresent)
      //{
      //  okButton.TranslateText("Dialog.OkButton");
      //  cancelButton.TranslateText("Dialog.CancelButton");
      //}

      inputTextBox.Top = promptLabel.Bottom + (promptLabel.Margin.Bottom + inputTextBox.Margin.Top);
      footerLabel.Top = inputTextBox.Bottom + (inputTextBox.Margin.Bottom + footerLabel.Margin.Top);
      height = footerLabel.Bottom + (promptLabel.Left * 2);

      //this.SetClientHeight(height);
    }

    #endregion Protected Methods

    #region Private Methods

    private void cancelButton_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      if (this.ValidationCallback != null && !this.ValidationCallback(this.Value))
      {
        this.DialogResult = DialogResult.None;
      }
      else
      {
        _value = inputTextBox.Text;
        this.DialogResult = DialogResult.OK;
      }
    }

    #endregion Private Methods
  }
}