using Magnum.Core.Services;
using Magnum.Core.Windows;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Events
{
  /// <summary>
  /// Class WorkspaceClosingEvent - This event is used when a status bar closes.
  /// </summary>
  public class StatusBarClosingEvent : CompositePresentationEvent<IStatusBarService>
  {
  }
}
