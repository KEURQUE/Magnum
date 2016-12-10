﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Natives
{
  [StructLayout(LayoutKind.Sequential)]
  public struct APPBARDATA
  {
    public int cbSize;
    public IntPtr hWnd;
    public int uCallbackMessage;
    public int uEdge;
    public RECT rc;
    public bool lParam;
  }
}
