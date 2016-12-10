using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Magnum.Core.Windows;
using Magnum.Core.Events;
using Magnum.Tools.ResourceBrowser.ViewModels;

namespace Magnum.Tools.ResourceBrowser
{
  [Module(ModuleName = "Magnum.Tools.ResourceBrowser")]
  public class ResourceBrowserModule : IModule
  {
    private readonly IUnityContainer _container;

    #region Constructors
    public ResourceBrowserModule(IUnityContainer container)
    {
      _container = container;
    }
    #endregion

    private IEventAggregator EventAggregator
    {
      get { return _container.Resolve<IEventAggregator>(); }
    }

    #region IModule Members

    public void Initialize()
    {
      EventAggregator.GetEvent<SplashMessageUpdateEvent>().Publish(new SplashMessageUpdateEvent { Message = "Loading Resource Browser Module" });
      _container.RegisterType<ResourceBrowserToolSettings>(new ContainerControlledLifetimeManager());
      _container.RegisterType<ResourceBrowserViewModel>();
      IWorkspace workspace = _container.Resolve<WorkspaceBase>();
      workspace.Tools.Add(_container.Resolve<ResourceBrowserViewModel>());
    }

    #endregion
  }
}