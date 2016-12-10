using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Windows
{
  public interface IToolWindow
  {
    /// <summary>
    /// Closes the tool.
    /// </summary>
    void Close(object info);
  }
}
