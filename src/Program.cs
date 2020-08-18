using System;
using System.Windows.Forms;

namespace Cyotek.Demo.ScriptingHost
{
  internal static class Program
  {
    #region Private Methods

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

    #endregion Private Methods
  }
}