namespace Cyotek.Demo.ScriptingHost
{
  internal static class Filters
  {
    #region Public Fields

    public const string AllFiles = "All Files (*.*)|*.*";

    public const string General = "All Supported Files (*.js; *.bmp; *.jpg; *.png)|*.js;*.bmp;*.jpg;*.png|" + Filters.ScriptFiles + "|" + Filters.Images + "|" + Filters.AllFiles;

    public const string Images = "All Supported Images (*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*.png|Bitmaps (*.bmp)|*.bmp|Joint Photographic Experts Group (*.jpg)|*.jpg|Portable Network Graphics (*.png)|*.png";

    public const string ScriptFiles = "Script Files (*.js)|*.js";

    #endregion Public Fields
  }
}