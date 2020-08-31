using Cyotek.Scripting.JavaScript;
using Cyotek.Windows.Forms;
using System;
using System.Windows.Forms;

// Adding Scripting to .NET Applications
// https://www.cyotek.com/blog/adding-scripting-to-net-applications

// Copyright © 2020 Cyotek Ltd. All Rights Reserved.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this example useful?
// https://www.paypal.me/cyotek

namespace Cyotek.Demo.ScriptingHost
{
  internal class SampleScriptEnvironment : ScriptEnvironment
  {
    #region Private Fields

    private readonly TextBoxBase _logControl;

    #endregion Private Fields

    #region Public Constructors

    public SampleScriptEnvironment(TextBoxBase logControl)
    {
      _logControl = logControl;
    }

    #endregion Public Constructors

    #region Protected Methods

    protected override void ClearScreen()
    {
      _logControl.Clear();
    }

    protected override void ShowAlert(string message)
    {
      MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    protected override bool ShowConfirm(string message)
    {
      return MessageBox.Show(message, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    protected override string ShowPrompt(string message, string defaultValue)
    {
      string result;
      Form owner;

      owner = _logControl.FindForm();

      if (owner.InvokeRequired)
      {
        Func<string, string, string> caller;
        IAsyncResult asyncResult;

        caller = this.ShowPromptDialog;

        asyncResult = owner.BeginInvoke(caller, message, defaultValue);
        asyncResult.AsyncWaitHandle.WaitOne();

        result = (string)owner.EndInvoke(asyncResult);

        asyncResult.AsyncWaitHandle.Close();
      }
      else
      {
        result = InputDialog.ShowInputDialog(_logControl.FindForm(), message, Application.ProductName, defaultValue);
      }

      return result;
    }

    protected override void WriteLine(string value)
    {
      if (_logControl.InvokeRequired)
      {
        _logControl.Invoke(new Action<string>(this.WriteLine), value);
      }
      else
      {
        _logControl.AppendText(value + Environment.NewLine);
      }
    }

    #endregion Protected Methods

    #region Private Methods

    private string ShowPromptDialog(string message, string defaultValue)
    {
      return InputDialog.ShowInputDialog(_logControl.FindForm(), message, defaultValue);
    }

    #endregion Private Methods
  }
}