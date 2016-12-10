using Magnum.Core.Events;
using Magnum.Core.Windows;
using Magnum.Tools.ErrorList.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Magnum.Tools.ErrorList
{
  [Module(ModuleName = "Magnum.Tools.ErrorList")]
  public class ErrorListModule : IModule
  {
    private readonly IUnityContainer _container;

    public ErrorListModule(IUnityContainer container)
    {
      _container = container;
    }

    private IEventAggregator EventAggregator
    {
      get { return _container.Resolve<IEventAggregator>(); }
    }

    #region IModule Members

    public void Initialize()
    {
      EventAggregator.GetEvent<SplashMessageUpdateEvent>().Publish(new SplashMessageUpdateEvent { Message = "Loading Error List Module" });
      _container.RegisterType<ErrorListToolSettings>(new ContainerControlledLifetimeManager());
      _container.RegisterType<ErrorListViewModel>();
      IWorkspace workspace = _container.Resolve<WorkspaceBase>();
      workspace.Tools.Add(_container.Resolve<ErrorListViewModel>());
    }

    #endregion
  }
}