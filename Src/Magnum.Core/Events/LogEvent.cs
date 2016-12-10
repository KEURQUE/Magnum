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
  /// Class LogEvent - This event is used when a logging operation happens.
  /// </summary>
  public class LogEvent : CompositePresentationEvent<ILoggerService>
  {
  }
}
