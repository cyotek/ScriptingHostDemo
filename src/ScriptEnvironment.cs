using Esprima;
using Esprima.Ast;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Interop;
using System;

namespace Cyotek.Scripting.JavaScript
{
  public abstract class ScriptEnvironment
  {
    #region Public Fields

    public const string MainFunctionName = "main";

    #endregion Public Fields

    #region Private Fields

    private Engine _engine;

    private bool _interactive;

    #endregion Private Fields

    #region Protected Constructors

    protected ScriptEnvironment()
    {
      _interactive = true;
    }

    #endregion Protected Constructors

    #region Public Properties

    public bool Interactive
    {
      get { return _interactive; }
      set { _interactive = value; }
    }

    #endregion Public Properties

    #region Public Methods

    public void AddFunction(string name, Delegate value)
    {
      this.InitializeEngine();

      _engine.SetValue(name, value);
    }

    public void AddType(string name, Type type)
    {
      this.InitializeEngine();

      _engine.SetValue(name, TypeReference.CreateTypeReference(_engine, type));
    }

    public void AddValue(string name, object value)
    {
      this.InitializeEngine();

      _engine.SetValue(name, value);
    }

    public void Execute(string input)
    {
      Script program;

      program = new JavaScriptParser(input).ParseScript();

      this.InitializeEngine();

      _engine.Execute(program);

      if (ScriptEnvironment.HasMainFunction(program) && !ScriptEnvironment.HasMainCaller(program))
      {
        _engine.Invoke(MainFunctionName);
      }
    }

    public void Invoke(string name)
    {
      this.InitializeEngine();

      _engine.Invoke(name);
    }

    public void Load(string script)
    {
      Script program;

      program = new JavaScriptParser(script).ParseScript();

      this.InitializeEngine();

      _engine.Execute(program);
    }

    public virtual string TransformValue(object value, bool useLiterals)
    {
      string result;

      if (value is JsValue jsValue)
      {
        result = ScriptEnvironment.TransformValue(jsValue, useLiterals);
      }
      else if (value is null)
      {
        result = useLiterals ? "null" : null;
      }
      else
      {
        result = value.ToString();
      }

      return result;
    }

    public void WrappedExecute(string script)
    {
      try
      {
        JsValue completionValue;
        JsValue result;
        string output;

        this.Execute(script);

        completionValue = _engine.GetCompletionValue();
        result = _engine.GetValue(completionValue);
        output = ScriptEnvironment.TransformValue(result, true);

        this.WriteLine(output);
      }
      catch (Exception ex)
      {
        this.WriteLine(string.Format("{0}: {1}", ex.GetType().Name, ex.GetBaseException().Message));
      }
    }

    #endregion Public Methods

    #region Protected Methods

    protected abstract void ClearScreen();

    protected string GetValueString(object obj)
    {
      return (obj ?? "NULL").ToString();
    }

    protected virtual void InitializeEnvironment()
    {
      this.AddFunction("print", new Action<object>(this.WriteLine));
      this.AddFunction("log", new Action<object>(this.WriteLine));
      this.AddFunction("cls", new Action(this.ClearScreen));

      // interactive functions
      this.AddFunction("alert", new Action<object>(this.ShowAlert));
      this.AddFunction("confirm", new Func<object, bool>(this.ShowConfirm));
      this.AddFunction("prompt", new Func<object, object, string>(this.ShowPrompt));
    }

    protected abstract void ShowAlert(string message);

    protected abstract bool ShowConfirm(string message);

    protected abstract string ShowPrompt(string message, string defaultValue);

    protected abstract void WriteLine(string value);

    #endregion Protected Methods

    #region Private Methods

    private static bool HasMainCaller(Program program)
    {
      bool result;

      result = false;

      foreach (Statement statement in program.Body)
      {
        if (statement is ExpressionStatement expressionStatement
            && expressionStatement.Expression is CallExpression callExpression
            && callExpression.Callee is Identifier identifier
            && string.Equals(identifier.Name, MainFunctionName, StringComparison.OrdinalIgnoreCase))
        {
          result = true;
          break;
        }
      }

      return result;
    }

    private static bool HasMainFunction(Script program)
    {
      bool result;

      result = false;

      for (int i = 0; i < program.ChildNodes.Count; i++)
      {
        if (program.ChildNodes[i] is FunctionDeclaration functionDeclaration && string.Equals(functionDeclaration.Id.Name, MainFunctionName, StringComparison.OrdinalIgnoreCase))
        {
          result = true;
          break;
        }
      }

      return result;
    }

    private static string TransformValue(JsValue jsValue, bool useLiterals)
    {
      string result;

      switch (jsValue.Type)
      {
        case Types.String:
          result = jsValue.AsString();
          break;

        case Types.Undefined:
          result = useLiterals ? "undefined" : null;
          break;

        case Types.Null:
          result = useLiterals ? "null" : null;
          break;

        case Types.Boolean:
          result = jsValue.AsBoolean().ToString();
          break;

        case Types.Number:
          result = jsValue.AsNumber().ToString();
          break;

        case Types.Object:
          result = jsValue.ToObject().ToString();
          break;

        case Types.None:
          result = string.Empty;
          break;

        default:
          result = jsValue.AsString();
          break;
      }

      return result;
    }

    private void InitializeEngine()
    {
      if (_engine == null)
      {
        _engine = new Engine();

        this.InitializeEnvironment();
      }
    }

    private void ShowAlert(object message)
    {
      if (_interactive)
      {
        this.ShowAlert(this.TransformValue(message, false));
      }
    }

    private bool ShowConfirm(object message)
    {
      return _interactive && this.ShowConfirm(this.TransformValue(message, false));
    }

    private string ShowPrompt(object message, object defaultValue)
    {
      return _interactive
         ? this.ShowPrompt(this.TransformValue(message, false), this.TransformValue(defaultValue, false))
         : null;
    }

    private void WriteLine(object value)
    {
      this.WriteLine(this.TransformValue(value, true));
    }

    #endregion Private Methods
  }
}