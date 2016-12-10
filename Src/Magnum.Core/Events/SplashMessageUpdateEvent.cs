using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Events
{
  /// <summary>
  /// Class SplashMessageUpdateEvent - This event happens a new module is loaded and wants to display a message on the splash window
  /// </summary>
  public class SplashMessageUpdateEvent : CompositePresentationEvent<SplashMessageUpdateEvent>
  {
    public string Message { get; set; }
  }
}
