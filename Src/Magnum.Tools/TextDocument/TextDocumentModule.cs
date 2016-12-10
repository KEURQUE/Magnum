using Magnum.Core.Events;
using Magnum.Core.Windows;
using Magnum.Tools.TextDocument.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Tools.TextDocument
{
  [Module(ModuleName = "Magnum.Tools.TextDocument")]
  public class TextDocumentModule : IModule
  {
    private readonly IUnityContainer _container;

    public TextDocumentModule(IUnityContainer container)
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
      EventAggregator.GetEvent<SplashMessageUpdateEvent>().Publish(new SplashMessageUpdateEvent { Message = "Loading TextDocument Module" });
      _container.RegisterType<TextViewModel>();
    }

    #endregion
  }
}
