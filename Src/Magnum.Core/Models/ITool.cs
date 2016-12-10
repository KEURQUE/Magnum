using Magnum.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Magnum.Core.Models
{
  /// <summary>
  /// Interface ITool
  /// </summary>
  public interface ITool
  {
    /// <summary>
    /// The content model
    /// </summary>
    /// <value>The model.</value>
    IModel Model { get; set; }

    /// <summary>
    /// The content view
    /// </summary>
    /// <value>The view.</value>
    UserControl View { get; set; }

    /// <summary>
    /// The content handler which does save and load of the file
    /// </summary>
    /// <value>The handler.</value>
    IContentHandler Handler { get; set; }
  }
}
