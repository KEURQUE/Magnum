using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Natives
{
  [StructLayout(LayoutKind.Sequential)]
  public struct MARGINS
  {
    public int leftWidth;
    public int rightWidth;
    public int topHeight;
    public int bottomHeight;
  }
}
