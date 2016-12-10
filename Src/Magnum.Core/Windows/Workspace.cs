using Magnum.Core.Windows;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Windows
{
  /// <summary>
  /// Class Workspace
  /// </summary>
  public class Workspace : WorkspaceBase
  {
    /// <summary>
    /// The generic workspace that will be used if the application does not have its workspace
    /// </summary>
    /// <param name="container">The injected container - can be used by custom flavors of workspace</param>
    public Workspace(IUnityContainer container)
      : base(container)
    {
    }
  }
}
