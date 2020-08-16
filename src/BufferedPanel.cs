using System.Windows.Forms;

// Arranging items radially around a central point using C#
// http://www.cyotek.com/blog/arranging-items-radially-around-a-central-point-using-csharp

// Copyright © 2017 Cyotek Ltd. All Rights Reserved.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this example useful?
// https://www.paypal.me/cyotek

namespace Cyotek.Demo.Windows.Forms
{
  internal class BufferedPanel : Panel
  {
    #region Constructors

    public BufferedPanel()
    {
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
    }

    #endregion
  }
}
