using Esprima;
using Esprima.Ast;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Interop;
using System;

// Adding Scripting to .NET Applications
// https://www.cyotek.com/blog/adding-scripting-to-net-applications

// Copyright © 2020 Cyotek Ltd. All Rights Reserved.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this example useful?
// https://www.paypal.me/cyotek

namespace Cyotek.Scripting.JavaScript
{
  public abstract class ScriptEnvironment
  {
    #region Public Fields

    public const string MainFunctionName = "main";

    #endregion Public Fields

    #region Private Fields

    private static readonly object[] _defaultArguments = new object[0];

    private Engine _engine;

    private bool _interactive;

    private bool _suppressErrors;

    #endregion Private Fields

    #region Protected Constructors

    protected ScriptEnvironment()
    {
      _interactive = true;
      _suppressErrors = true;
    }

    #endregion Protected Constructors

    #region Public Properties

    public bool Interactive
    {
      get { return _interactive; }
      set { _interactive = value; }
    }

    public bool SuppressErrors
    {
      get { return _suppressErrors; }
      set { _suppressErrors = value; }
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

    public object Evaluate(string script)
    {
      this.Load(script);

      return _engine.GetCompletionValue().ToObject();
    }

    public object Execute(string script)
    {
      object result;

      this.Load(script, out Script program);

      if (ScriptEnvironment.HasMainFunction(program) && !ScriptEnvironment.HasMainCaller(program))
      {
        result = this.Invoke(MainFunctionName);
      }
      else
      {
        result = _engine.GetCompletionValue().ToObject();
      }

      return result;
    }

    public object Invoke(string name)
    {
      return this.Invoke(name, _defaultArguments);
    }

    public object Invoke(string name, params object[] arguments)
    {
      object result;

      try
      {
        this.InitializeEngine();

        result = _engine.Invoke(name, arguments).ToObject();
      }
      catch (Exception ex)
      {
        result = null;

        this.HandleException(ex);

        if (!_suppressErrors)
        {
          throw;
        }
      }

      return result;
    }

    public void Load(string script)
    {
      this.Load(script, out Script _);
    }

    #endregion Public Methods

    #region Protected Methods

    protected abstract void ClearScreen();

    protected virtual void HandleException(Exception ex)
    {
      // TODO: Convert to cancellable event
      this.WriteLine(ex.GetBaseException().Message);
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

    private static string GetValueString(JsValue jsValue, bool useLiterals)
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

    private static bool HasFunction(Script program, string name)
    {
      bool result;

      result = false;

      for (int i = 0; i < program.ChildNodes.Count; i++)
      {
        if (program.ChildNodes[i] is FunctionDeclaration functionDeclaration
            && string.Equals(functionDeclaration.Id.Name, name, StringComparison.OrdinalIgnoreCase))
        {
          result = true;
          break;
        }
      }

      return result;
    }

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
      return ScriptEnvironment.HasFunction(program, MainFunctionName);
    }

    private string GetValueString(object value, bool useLiterals)
    {
      string result;

      if (value is JsValue jsValue)
      {
        result = ScriptEnvironment.GetValueString(jsValue, useLiterals);
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

    private void InitializeEngine()
    {
      if (_engine == null)
      {
        _engine = new Engine();

        this.InitializeEnvironment();
      }
    }

    private void Load(string script, out Script program)
    {
      program = null;

      try
      {
        program = new JavaScriptParser(script).ParseScript();

        this.InitializeEngine();

        _engine.Execute(program);
      }
      catch (Exception ex)
      {
        this.HandleException(ex);

        if (!_suppressErrors)
        {
          throw;
        }
      }
    }

    private void ShowAlert(object message)
    {
      if (_interactive)
      {
        this.ShowAlert(this.GetValueString(message, false));
      }
    }

    private bool ShowConfirm(object message)
    {
      return _interactive && this.ShowConfirm(this.GetValueString(message, false));
    }

    private string ShowPrompt(object message, object defaultValue)
    {
      return _interactive
         ? this.ShowPrompt(this.GetValueString(message, false), this.GetValueString(defaultValue, false))
         : null;
    }

    private void WriteLine(object value)
    {
      this.WriteLine(this.GetValueString(value, true));
    }

    #endregion Private Methods
  }
}