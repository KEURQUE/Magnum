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
  /// Class OpenContentEvent - This event happens when a new document is opened.
  /// </summary>
  public class OpenContentEvent : CompositePresentationEvent<DocumentViewModel>
  {
  }
}
