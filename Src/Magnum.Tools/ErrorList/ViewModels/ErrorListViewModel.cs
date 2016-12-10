using Magnum.Core.Events;
using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Magnum.IconLibrary;
using Magnum.Tools.ErrorList.Models;
using Magnum.Tools.ErrorList.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Microsoft.Windows.Controls.Ribbon;
using System;

namespace Magnum.Tools.ErrorList.ViewModels
{
  public class ErrorListViewModel : ToolViewModel, IErrorList
  {
    private readonly ErrorListModel _model;
    private readonly ErrorListView _view;

    public ErrorListViewModel(IUnityContainer container, WorkspaceBase workspace)
      : base(container, workspace)
    {
      Name = "Error List";
      Title = "Error List";
      ContentId = "Error List";
      RibbonHeader = "ERROR LIST";
      Description = "Shows different errors from the engine or the editor";
      IsVisible = false;
      IconSource = BitmapImages.LoadBitmapFromResourceKey("Error_16x16");
      _model = new ErrorListModel(Name, container, "Standard", Description);
      Model = _model;

      _view = new ErrorListView();
      _view.DataContext = _model;
      View = _view;

      LoadSettings();

      var manager = Container.Resolve<ICommandManager>();
      CloseCommand = new UndoableCommand<object>(Close);
      OpenCommand = manager.GetCommand("NEW");
    }

    #region Methods
    private void LoadSettings()
    {
      //Set the preferred height, width and location of the tool based on previous session values
      ErrorListToolSettings errorListToolSettings = Container.Resolve<ErrorListToolSettings>();
      this.PreferredHeight = errorListToolSettings.PreferredHeight;
      this.PreferredWidth = errorListToolSettings.PreferredWidth;
      this.PreferredLocation = errorListToolSettings.PreferredLocation;
      this.FocusOnRibbonOnClick = errorListToolSettings.FocusOnRibbonOnClick;
      this.KeepRibbonTabActive = errorListToolSettings.KeepRibbonTabActive;
      (this.Model as ErrorListModel).ShowErrors = errorListToolSettings.ShowErrors;
      (this.Model as ErrorListModel).ShowWarnings = errorListToolSettings.ShowWarnings;
      (this.Model as ErrorListModel).ShowMessages = errorListToolSettings.ShowMessages;
      if (!errorListToolSettings.ShowErrors || !errorListToolSettings.ShowWarnings || !errorListToolSettings.ShowMessages)
        this.FocusOnRibbonOnClick = false;
    }

    public override void Close(object obj)
    {
      SaveToolSettings();

      base.Close(obj);
    }
    #endregion

    #region Events
    void SaveToolSettings()
    {
      IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<ErrorListSettingsChangeEvent>().Publish(this);
    }
    #endregion
  }
}