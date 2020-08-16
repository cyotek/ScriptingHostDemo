using Cyotek.Scripting.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cyotek.Demo.ScriptingHost
{
  internal class SampleScriptEnvironment : ScriptEnvironment
  {
    private readonly TextBoxBase _logControl;

    public SampleScriptEnvironment(TextBoxBase logControl)
    {
      _logControl = logControl;
    }

    protected override void ClearScreen()
    {
      _logControl.Clear();
    }

    protected override void ShowAlert(object obj)
    {
      throw new NotImplementedException();
    }

    protected override bool ShowConfirm(object obj)
    {
      throw new NotImplementedException();
    }

    protected override void WriteLine(string value)
    {
      _logControl.AppendText(value + Environment.NewLine);
    }
  }
}
