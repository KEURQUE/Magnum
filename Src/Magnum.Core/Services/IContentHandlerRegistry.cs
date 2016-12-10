using Magnum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Services
{
  /// <summary>
  /// Interface IContentHandlerRegistry
  /// </summary>
  public interface IContentHandlerRegistry
  {
    /// <summary>
    /// Register a content handler with the registry
    /// </summary>
    /// <param name="handler">The content handler</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise</returns>
    bool Register(IContentHandler handler);

    /// <summary>
    /// Unregisters a content handler
    /// </summary>
    /// <param name="handler">The handler to remove</param>
    /// <returns><c>true</c> if successfully unregistered, <c>false</c> otherwise</returns>
    bool Unregister(IContentHandler handler);

    /// <summary>
    /// Returns a content view model for the specified object which needs to be displayed as a document
    /// The object could be anything - based on the handlers, a content view model is returned
    /// </summary>
    /// <param name="info">The object which needs to be displayed as a document</param>
    /// <returns>The content view model for the given info</returns>
    ITool GetViewModel(object info);

    /// <summary>
    /// Returns a content view model for the specified contentID which needs to be displayed as a document
    /// The contentID is the ID used in AvalonDock
    /// </summary>
    /// <param name="contentId">The contentID which needs to be displayed as a document</param>
    /// <returns>The content view model for the given info</returns>
    ITool GetViewModelFromContentId(string contentId);
  }
}
