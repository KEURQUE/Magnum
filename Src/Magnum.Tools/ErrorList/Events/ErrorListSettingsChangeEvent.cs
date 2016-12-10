using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Tools.ErrorList.ViewModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Tools.ErrorList
{
  /// <summary>
  /// Class ErrorListSettingsChangeEvent - This event happens when the error list tool setting is changed.
  /// </summary>
  public class ErrorListSettingsChangeEvent : CompositePresentationEvent<ErrorListViewModel>
  {
  }
}
