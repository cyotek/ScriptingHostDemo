namespace Cyotek.Demo.ScriptingHost
{
  internal static class Filters
  {
    #region Public Fields

    public const string AllFiles = "All Files (*.*)|*.*";

    public const string General = Filters.ScriptFiles + "|" + Filters.AllFiles;

    public const string ScriptFiles = "Script Files (*.js)|*.js";

    #endregion Public Fields
  }
}