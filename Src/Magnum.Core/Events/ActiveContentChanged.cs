using Magnum.Core.Models;
using Magnum.Core.ViewModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Events
{
  /// <summary>
  /// Class ActiveContentChangedEvent - This event is used when the active content is changed in Avalon Dock.
  /// </summary>
  public class ActiveContentChangedEvent : CompositePresentationEvent<object>
  {
  }
}
