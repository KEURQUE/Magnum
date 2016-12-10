using Magnum.Core.Events;
using Magnum.Core.Windows;
using Magnum.Tools.PropertyGrid.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Magnum.Tools.PropertyGrid
{
  [Module(ModuleName = "Magnum.Tools.PropertyGrid")]
  public class PropertyGridModule : IModule
  {
    private readonly IUnityContainer _container;

    public PropertyGridModule(IUnityContainer container)
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
      EventAggregator.GetEvent<SplashMessageUpdateEvent>().Publish(new SplashMessageUpdateEvent { Message = "Loading Properties Module" });
      _container.RegisterType<PropertyGridViewModel>();
      IWorkspace workspace = _container.Resolve<WorkspaceBase>();
      workspace.Tools.Add(_container.Resolve<PropertyGridViewModel>());
    }

    #endregion
  }
}