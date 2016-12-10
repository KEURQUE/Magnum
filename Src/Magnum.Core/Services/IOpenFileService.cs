using Magnum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Services
{
  /// <summary>
  /// Interface IOpenFileService - used to open a file
  /// </summary>
  public interface IOpenFileService
  {
    /// <summary>
    /// Opens the document from the specified location.
    /// </summary>
    /// <param name="location">The location.</param>
    /// <returns>ITool.</returns>
    ITool Open(object location = null);

    /// <summary>
    /// Opens from content from an ID.
    /// </summary>
    /// <param name="contentID">The content ID.</param>
    /// <param name="makeActive">if set to <c>true</c> makes the new document as the active document.</param>
    /// <returns>ITool.</returns>
    ITool OpenFromID(string contentID, bool makeActive = false);
  }
}
