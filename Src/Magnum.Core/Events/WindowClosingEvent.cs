using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magnum.Core.Events
{
  /// <summary>
  /// Class WindowClosingEvent - This event is used when a the window closes.
  /// </summary>
  public class WindowClosingEvent : CompositePresentationEvent<Window>
  {
  }
}
