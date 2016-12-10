using Magnum.Core.Services;
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
  /// Class ToolSettingsChangeEvent - This event happens when a tool setting is changed.
  /// </summary>
  public class ToolSettingsChangeEvent : CompositePresentationEvent<ToolViewModel>
  {
  }
}
