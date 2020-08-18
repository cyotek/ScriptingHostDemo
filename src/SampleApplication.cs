using System.Windows.Forms;

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