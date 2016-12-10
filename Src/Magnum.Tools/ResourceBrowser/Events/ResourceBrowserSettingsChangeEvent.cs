using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Tools.ResourceBrowser.ViewModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Tools.ResourceBrowser.Events
{
  /// <summary>
  /// Class ResourceBrowserSettingsChangeEvent - This event happens when the resource browser tool setting is changed.
  /// </summary>
  public class ResourceBrowserSettingsChangeEvent : CompositePresentationEvent<ResourceBrowserViewModel>
  {
  }
}
