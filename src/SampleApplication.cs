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
  internal class SampleApplication
  {
    #region Private Fields

    private readonly MainForm _host;

    private string _title;

    #endregion Private Fields

    #region Public Constructors

    public SampleApplication(MainForm host)
    {
      _host = host;
      _title = Application.ProductName;
    }

    #endregion Public Constructors

    #region Public Properties

    public string Title
    {
      get { return _title; }
      set
      {
        _title = value;
        _host.UpdateWindowTitle();
      }
    }

    #endregion Public Properties

    #region Public Methods

    public void Quit()
    {
      _host.Close();
    }

    #endregion Public Methods
  }
}