using Magnum.Core.Events;
using Magnum.Core.Windows;
using Magnum.Tools.Logger.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Magnum.Tools.Logger
{
  [Module(ModuleName = "Magnum.Tools.Logger")]
  public class ObjectEditorModule : IModule
  {
    private readonly IUnityContainer _container;

    public ObjectEditorModule(IUnityContainer container)
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
      EventAggregator.GetEvent<SplashMessageUpdateEvent>().Publish(new SplashMessageUpdateEvent { Message = "Loading Logger Module" });
      _container.RegisterType<ObjectEditorViewModel>();
      IWorkspace workspace = _container.Resolve<WorkspaceBase>();
      workspace.Tools.Add(_container.Resolve<ObjectEditorViewModel>());
    }

    #endregion
  }
}