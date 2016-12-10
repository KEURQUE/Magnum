using Magnum.Core.Services;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Events
{
  /// <summary>
  /// Class RibbonClosingEvent - This event is used when the window closes, thus closing the ribbon.
  /// </summary>
  public class RibbonClosingEvent : CompositePresentationEvent<IRibbonService>
  {
  }
}
