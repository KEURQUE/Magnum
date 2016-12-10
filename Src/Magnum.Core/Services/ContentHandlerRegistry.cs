using Magnum.Core.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Services
{
  /// <summary>
  /// The content handler registry which manages different content handlers
  /// </summary>
  public sealed class ContentHandlerRegistry : IContentHandlerRegistry
  {
    /// <summary>
    /// List of content handlers
    /// </summary>
    private readonly List<IContentHandler> _contentHandlers;

    /// <summary>
    /// Constructor of content handler registry
    /// </summary>
    /// <param name="container">The injected container of the application</param>
    public ContentHandlerRegistry(IUnityContainer container)
    {
      _contentHandlers = new List<IContentHandler>();
    }

    /// <summary>
    /// List of content handlers
    /// </summary>
    public List<IContentHandler> ContentHandlers
    {
      get { return _contentHandlers; }
    }

    #region IContentHandlerRegistry Members
    /// <summary>
    /// Register a content handler with the registry
    /// </summary>
    /// <param name="handler">The content handler</param>
    /// <returns>true, if successful - false, otherwise</returns>
    public bool Register(IContentHandler handler)
    {
      ContentHandlers.Add(handler);
      return true;
    }

    /// <summary>
    /// Unregisters a content handler
    /// </summary>
    /// <param name="handler">The handler to remove</param>
    /// <returns></returns>
    public bool Unregister(IContentHandler handler)
    {
      return ContentHandlers.Remove(handler);
    }

    #endregion

    #region Get the view model
    /// <summary>
    /// Returns a content view model for the specified object which needs to be displayed as a document
    /// The object could be anything - based on the handlers, a content view model is returned
    /// </summary>
    /// <param name="info">The object which needs to be displayed as a document</param>
    /// <returns>The content view model for the given info</returns>
    public ITool GetViewModel(object info)
    {
      for (int i = ContentHandlers.Count - 1; i >= 0; i--)
      {
        var opener = ContentHandlers[i];
        if (opener.ValidateContentType(info))
        {
          ITool vm = opener.OpenContent(info) as ITool;
          vm.Handler = opener;
          return vm;
        }
      }
      return null;
    }

    /// <summary>
    /// Returns a content view model for the specified contentID which needs to be displayed as a document
    /// The contentID is the ID used in AvalonDock
    /// </summary>
    /// <param name="contentId">The contentID which needs to be displayed as a document</param>
    /// <returns>The content view model for the given info</returns>
    public ITool GetViewModelFromContentId(string contentId)
    {
      for (int i = ContentHandlers.Count - 1; i >= 0; i--)
      {
        var opener = ContentHandlers[i];
        if (opener.ValidateContentFromId(contentId))
        {
          ITool vm = opener.OpenContentFromId(contentId) as ITool;
          vm.Handler = opener;
          return vm;
        }
      }
      return null;
    }

    #endregion
  }
}
