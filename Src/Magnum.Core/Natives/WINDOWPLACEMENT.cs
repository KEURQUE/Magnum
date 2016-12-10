using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Natives
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public struct WINDOWPLACEMENT
  {
    public int length;
    public int flags;
    public int showCmd;
    public POINT minPosition;
    public POINT maxPosition;
    public RECT normalPosition;
  }
}
