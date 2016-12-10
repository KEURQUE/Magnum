using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using CommandManager = Magnum.Core.Managers.CommandManager;
using System.ComponentModel;
using Magnum.Core.Services;
using Magnum.Core.Events;
using Magnum.Core.Models;
using Magnum.Core.Windows;
using Magnum.Core.ViewModels;
using Magnum.Core.Extensions;

namespace Magnum.Tools
{
  namespace Magnum.Core
  {
    /// <summary>
    /// The Core module
    /// </summary>
    [Module(ModuleName = "Magnum.Tools")]
    public class ToolsModule : IModule
    {
      /// <summary>
      /// The container used in the application
      /// </summary>
      private readonly IUnityContainer _container;
      /// <summary>
      /// The event aggregator
      /// </summary>
      private IEventAggregator _eventAggregator;

      /// <summary>
      /// The constructor of the CoreModule
      /// </summary>
      /// <param name="container">The injected container used in the application</param>
      /// <param name="eventAggregator">The injected event aggregator</param>
      public ToolsModule(IUnityContainer container, IEventAggregator eventAggregator)
      {
        _container = container;
        _eventAggregator = eventAggregator;

        // Add composition support for unity
        _container.AddNewExtension<LazySupportExtension>();
      }

      /// <summary>
      /// The event aggregator pattern
      /// </summary>
      private IEventAggregator EventAggregator
      {
        get { return _eventAggregator; }
      }

      #region IModule Members
      /// <summary>
      /// The initialize call of the module - this gets called when the container is trying to load the modules.
      /// Register your <see cref="Type"/>s and Commands here
      /// </summary>
      public void Initialize()
      {
        EventAggregator.GetEvent<SplashMessageUpdateEvent>().Publish(new SplashMessageUpdateEvent { Message = "Loading Core Module" });
        _container.RegisterType<ThemeSettings>(new ContainerControlledLifetimeManager());
        _container.RegisterType<WindowPositionSettings>(new ContainerControlledLifetimeManager());
        _container.RegisterType<StatusBarSettings>(new ContainerControlledLifetimeManager());
        _container.RegisterType<RibbonSettings>(new ContainerControlledLifetimeManager());
        _container.RegisterType<IOpenFileService, OpenFileService>(new ContainerControlledLifetimeManager());
        _container.RegisterType<IContentHandlerRegistry, ContentHandlerRegistry>(new ContainerControlledLifetimeManager());
        _container.RegisterType<ICommandManager, CommandManager>(new ContainerControlledLifetimeManager());
        _container.RegisterType<IRibbonService, RibbonService>(new ContainerControlledLifetimeManager());
        _container.RegisterType<IMenuService, MenuItemViewModel>(new ContainerControlledLifetimeManager(),
                                                                         new InjectionConstructor(
                                                                             new InjectionParameter(typeof(string),
                                                                                                    "$MAIN$"),
                                                                             new InjectionParameter(typeof(int), 1),
                                                                             new InjectionParameter(
                                                                                 typeof(ImageSource), null),
                                                                             new InjectionParameter(typeof(IUndoableCommand),
                                                                                                    null),
                                                                             new InjectionParameter(typeof(string), ""),
                                                                             new InjectionParameter(
                                                                                 typeof(KeyGesture), null),
                                                                             new InjectionParameter(typeof(bool), false),
                                                                             new InjectionParameter(
                                                                                 typeof(IUnityContainer), _container),
                                                                             new InjectionParameter(typeof(string), ""),
                                                                             new InjectionParameter(typeof(string), "")));

        AppCommands();

        //Try resolving a workspace
        try
        {
          _container.Resolve<WorkspaceBase>();
        }
        catch
        {
          _container.RegisterType<WorkspaceBase, Workspace>(new ContainerControlledLifetimeManager());
        }
      }

      #endregion

      /// <summary>
      /// The AppCommands registered by the Core Module
      /// </summary>
      private void AppCommands()
      {
        var manager = _container.Resolve<ICommandManager>();

        var newCommand = new UndoableCommand<object>(NewDocument, CanExecuteNewCommand);
        manager.RegisterCommand("NEW", newCommand);
      }

      #region Commands
      private bool CanExecuteNewCommand(object obj)
      {
        return true;
      }

      private void NewDocument(object obj)
      {
        var contentHandler = _container.Resolve<IContentHandlerRegistry>() as ContentHandlerRegistry;
        var workspace = _container.Resolve<WorkspaceBase>();

        if (contentHandler != null)
        {
          if (contentHandler.ContentHandlers.Count != 1)
          {
            foreach (var handler in contentHandler.ContentHandlers)
            {
              //TODO: This is the place where we want to show a window and make the end user select a type of file
              workspace.Documents.Add(handler.NewContent(null) as DocumentViewModel);
            }
          }
          else
          {
            var openValue = contentHandler.ContentHandlers[0].NewContent(null);
            workspace.Documents.Add(openValue as DocumentViewModel);

            //Make it the active document
            workspace.ActiveDocument = openValue as DocumentViewModel;
          }
        }
        workspace.IsToolsLibraryOpened = false;
      }

      #endregion
    }
  }
}
