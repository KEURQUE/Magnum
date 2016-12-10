using Magnum.Core;
using Magnum.Core.Attributes;
using Magnum.Core.Events;
using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Services
{
  public class OpenFileService : IOpenFileService
  {
    /// <summary>
    /// The injected container
    /// </summary>
    private readonly IUnityContainer _container;
    /// <summary>
    /// The injected event aggregator
    /// </summary>
    private readonly IEventAggregator _eventAggregator;
    /// <summary>
    /// The injected logger
    /// </summary>
    private readonly ILoggerService _logger;

    /// <summary>
    /// Constructor for Open file service
    /// </summary>
    /// <param name="container">The injected container</param>
    /// <param name="eventAggregator">The injected event aggregator</param>
    /// <param name="logger">The injected logger</param>
    public OpenFileService(IUnityContainer container, IEventAggregator eventAggregator, ILoggerService logger)
    {
      _container = container;
      _eventAggregator = eventAggregator;
      _logger = logger;
    }

    #region IOpenFileService Members
    /// <summary>
    /// Opens the object - if object is null, show a open file dialog to select a file to open
    /// </summary>
    /// <param name="location">The optional object to open</param>
    /// <returns>A document which was added to the workspace as a content view model</returns>
    public ITool Open(object location = null)
    {
      var statusBar = _container.Resolve<IStatusBarService>();
      var progressBar = _container.Resolve<IProgressBarService>();
      progressBar.Progress(true, 0, 2);
      IWorkspace workspace = _container.Resolve<WorkspaceBase>();
      //RecentViewSettings recentSettings = _container.Resolve<IRecentViewSettings>() as RecentViewSettings;
      var contentHandlerRegistry = _container.Resolve<IContentHandlerRegistry>() as ContentHandlerRegistry;

      workspace.IsBusy = true;
      var dialog = new OpenFileDialog();
      bool? result;

      if (location == null)
      {
        progressBar.Progress(true, 1, 2);
        dialog.Filter = "";
        string sep = "";
        var attributes = contentHandlerRegistry.ContentHandlers.SelectMany(handler => (FileContentAttribute[])(handler.GetType()).GetCustomAttributes(typeof(FileContentAttribute), true)).ToList();
        attributes.Sort((attribute, contentAttribute) => attribute.Priority - contentAttribute.Priority);
        foreach (var contentAttribute in attributes)
        {
          dialog.Filter = String.Format("{0}{1}{2} ({3})|{3}", dialog.Filter, sep, contentAttribute.Display, contentAttribute.Extension);
          sep = "|";
        }

        statusBar.Text = "Please select a file to open with one of the following supported extensions: " + dialog.Filter;
        result = dialog.ShowDialog();
        location = dialog.FileName;
      }
      else
      {
        result = true;
      }

      if (result == true && !string.IsNullOrWhiteSpace(location.ToString()))
      {
        //Let the handler figure out which view model to return
        if (contentHandlerRegistry != null)
        {
          ITool openValue = contentHandlerRegistry.GetViewModel(location);

          if (openValue != null)
          {
            //Check if the document is already open
            foreach (ITool contentViewModel in workspace.Documents)
            {
              if (contentViewModel.Model.Location != null && contentViewModel.Model.Location.Equals(openValue.Model.Location))
              {
                _logger.Log("Document " + contentViewModel.Model.Location + "already open - making it active", LogCategory.Info, LogPriority.Low);
                workspace.ActiveDocument = contentViewModel as DocumentViewModel;
                return contentViewModel;
              }
            }

            _logger.Log("Opening file" + location + " !!", LogCategory.Info, LogPriority.Low);

            // Publish the event to the Application - subscribers can use this object
            _eventAggregator.GetEvent<OpenContentEvent>().Publish(openValue as DocumentViewModel);

            //Add it to the actual workspace
            workspace.Documents.Add(openValue as DocumentViewModel);

            //Make it the active document
            workspace.ActiveDocument = openValue as DocumentViewModel;

            //Add it to the recent documents opened
            //recentSettings.Update(openValue);
          }
          else
          {
            _logger.Log("Unable to find a IContentHandler to open " + location, LogCategory.Error, LogPriority.High);
          }
        }
      }
      else
      {
        _logger.Log("Canceled out of open file dialog", LogCategory.Info, LogPriority.Low);
      }
      workspace.IsBusy = false;
      statusBar.Clear();
      progressBar.Progress(false, 2, 2);
      return null;
    }

    /// <summary>
    /// Opens the contentID
    /// </summary>
    /// <param name="contentID">The contentID to open</param>
    /// <param name="makeActive">if set to <c>true</c> makes the new document as the active document.</param>
    /// <returns>A document which was added to the workspace as a content view model</returns>
    public ITool OpenFromID(string contentID, bool makeActive = false)
    {
      IWorkspace workspace = _container.Resolve<WorkspaceBase>();
      //RecentViewSettings recentSettings = _container.Resolve<IRecentViewSettings>() as RecentViewSettings;
      var handler = _container.Resolve<IContentHandlerRegistry>();

      //Let the handler figure out which view model to return
      ITool openValue = handler.GetViewModelFromContentId(contentID);

      if (openValue != null)
      {
        //Check if the document is already open
        foreach (ITool contentViewModel in workspace.Documents)
        {
          if (contentViewModel.Model.Location.Equals(openValue.Model.Location))
          {
            _logger.Log("Document " + contentViewModel.Model.Location + "already open.", LogCategory.Info, LogPriority.Low);

            if (makeActive)
              workspace.ActiveDocument = contentViewModel as DocumentViewModel;

            return contentViewModel;
          }
        }

        _logger.Log("Opening content with " + contentID + " !!", LogCategory.Info, LogPriority.Low);

        // Publish the event to the Application - subscribers can use this object
        _eventAggregator.GetEvent<OpenContentEvent>().Publish(openValue as DocumentViewModel);

        workspace.Documents.Add(openValue as DocumentViewModel);

        if (makeActive)
          workspace.ActiveDocument = openValue as DocumentViewModel;

        //Add it to the recent documents opened
        //recentSettings.Update(openValue);

        return openValue;
      }

      _logger.Log("Unable to find a IContentHandler to open content with ID = " + contentID, LogCategory.Error, LogPriority.High);
      return null;
    }

    #endregion
  }
}
