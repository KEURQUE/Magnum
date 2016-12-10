using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Events
{
  /// <summary>
  /// Class SplashCloseEvent - This event happens when the splash window is closed
  /// </summary>
  public class SplashCloseEvent : CompositePresentationEvent<SplashCloseEvent>
  {
  }
}
