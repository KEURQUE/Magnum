using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Args
{
  public class WindowClosingEventArgs : CancelEventArgs
  {
    public WindowClosingEventArgs()
    {
      WindowClosing = false;
    }

    public WindowClosingEventArgs(bool windowClosing)
    {
      WindowClosing = windowClosing;
    }

    public bool WindowClosing { get; set; }
  }
}
