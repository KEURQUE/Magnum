using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Models
{
  public interface IModel
  {
    /// <summary>
    /// The document location - could be a file location/server object etc.
    /// </summary>
    object Location { get; set; }

    /// <summary>
    /// Is the document dirty - does it need to be saved?
    /// </summary>
    bool IsDirty { get; set; }
  }
}
