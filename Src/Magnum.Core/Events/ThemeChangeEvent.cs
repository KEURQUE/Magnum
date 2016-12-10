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
  /// Class ThemeChangeEvent - This event happens when a theme is changed.
  /// </summary>
  public class ThemeChangeEvent : CompositePresentationEvent<ITheme>
  {
  }
}
