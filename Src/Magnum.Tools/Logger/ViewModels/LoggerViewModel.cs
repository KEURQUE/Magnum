using Magnum.Core.Events;
using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Magnum.IconLibrary;
using Magnum.Tools.Logger.Models;
using Magnum.Tools.Logger.Views;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace Magnum.Tools.Logger.ViewModels
{
  internal class ObjectEditorViewModel : ToolViewModel
  {
    #region Fields
    private readonly IEventAggregator _aggregator;
    private readonly LoggerModel _model;
    private readonly LoggerView _view;
    #endregion

    #region Constructors
    public ObjectEditorViewModel(IUnityContainer container, WorkspaceBase workspace)
      : base(container, workspace)
    {
      PreferredLocation = PaneLocation.Bottom;
      Name = "Logger";
      Title = "Logger";
      ContentId = "Logger";
      RibbonHeader = "LOGGER";
      Description = "Shows different logs from the engine or the editor";
      IsVisible = false;
      IconSource = BitmapImages.LoadBitmapFromResourceKey("Logger_32x32");
      _model = new LoggerModel(Name, container, "Standard", Description);
      Model = _model;

      _view = new LoggerView();
      _view.DataContext = _model;
      View = _view;

      _aggregator = Container.Resolve<IEventAggregator>();
      _aggregator.GetEvent<LogEvent>().Subscribe(AddLog);

      var manager = Container.Resolve<ICommandManager>();
      OpenCommand = manager.GetCommand("LOGSHOW");
    }
    #endregion

    #region Methods
    private void AddLog(ILoggerService logger)
    {
      _model.AddLog(logger);
    }
    #endregion
  }
}