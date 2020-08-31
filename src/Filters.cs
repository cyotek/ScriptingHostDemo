// Adding Scripting to .NET Applications
// https://www.cyotek.com/blog/adding-scripting-to-net-applications

// Copyright © 2020 Cyotek Ltd. All Rights Reserved.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this example useful?
// https://www.paypal.me/cyotek

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