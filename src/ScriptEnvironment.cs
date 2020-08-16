using Jint;
using Jint.Native;
using Jint.Parser;
using Jint.Parser.Ast;
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

    #endregion Private Fields

    #region Public Methods

    public void Execute(string input)
    {
      Program program;

      program = new JavaScriptParser().Parse(input);

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
      Program program;

      program = new JavaScriptParser().Parse(script);

      this.InitializeEngine();

      _engine.Execute(program);
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
        output = TransformValue((object)result);

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
      this.AddFunction("alert", new Action<object>(this.ShowAlert));
      this.AddFunction("confirm", new Func<object, bool>(this.ShowConfirm));
      this.AddFunction("cls", new Action(this.ClearScreen));
      // TODO: prompt
    }

    public void AddValue(string name, object value)
    {
      this.InitializeEngine();

      _engine.SetValue(name, value);
    }

    public void AddFunction(string name, Delegate value)
    {
      this.InitializeEngine();

      _engine.SetValue(name, value);
    }

    public void AddType(string name,Type type)
    {
      this.InitializeEngine();

      _engine.SetValue(name, TypeReference.CreateTypeReference(_engine, type));
    }

    protected abstract void ShowAlert(object obj);

    protected abstract bool ShowConfirm(object obj);

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

    private static bool HasMainFunction(Program program)
    {
      bool result;

      result = false;

      for (int i = 0; i < program.FunctionDeclarations.Count; i++)
      {
        FunctionDeclaration functionDeclaration;

        functionDeclaration = program.FunctionDeclarations[i];

        if (string.Equals(functionDeclaration.Id.Name, MainFunctionName, StringComparison.OrdinalIgnoreCase))
        {
          result = true;
          break;
        }
      }

      return result;
    }

    private static string TransformValue(object value)
    {
      string result;

      if (value is JsValue jsValue)
      {
        result = TransformValue(jsValue);
      }
      else if (value is null)
      {
        result = "null";
      }
      else
      {
        result = value.ToString();
      }

      return result;
    }

    private static string TransformValue(JsValue jsValue)
    {
      string result;

      switch (jsValue.Type)
      {
        case Types.String:
          result = jsValue.AsString();
          break;

        case Types.Undefined:
          result = "undefined";
          break;

        case Types.Null:
          result = "null";
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

    private void WriteLine(object obj)
    {
      this.WriteLine(ScriptEnvironment.TransformValue(obj));
    }

    #endregion Private Methods
  }
}