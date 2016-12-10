using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Magnum.Core.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Magnum.Core.Windows;
using Magnum.Core.Services;
using Microsoft.Windows.Controls.Ribbon;
using Magnum.Core.Events;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Engine.Wrappers.Services;
using Magnum.IconLibrary;
using Magnum.Tools.ResourceBrowser.Models;
using Magnum.Tools.ResourceBrowser.Events;
using Magnum.Tools.ResourceBrowser.Views;

namespace Magnum.Tools.ResourceBrowser.ViewModels
{
  public class ResourceBrowserViewModel : ToolViewModel, IResourceBrowser
  {
    #region Fields
    private readonly ResourceBrowserModel _model;
    private readonly ResourceBrowserView _view;
    #endregion

    #region Constructors
    public ResourceBrowserViewModel(IUnityContainer container, WorkspaceBase workspace)
      : base(container, workspace)
    {
      PreferredLocation = PaneLocation.Center;
      Name = "Resource Browser";
      Title = "Resource Browser";
      ContentId = "ResourceBrowser";
      RibbonHeader = "RESOURCE BROWSER";
      Description = "View and edit game files that are contained in the database";
      IsVisible = false;
      IconSource = BitmapImages.LoadBitmapFromResourceKey("ResourceBrowser_128x128");
      _model = new ResourceBrowserModel(Name, container, "Standard", Description);
      Model = _model;

      _view = new ResourceBrowserView();
      _view.DataContext = _model;
      View = _view;

      LoadSettings();

      CloseCommand = new UndoableCommand<object>(Close);
    }
    #endregion

    #region Methods
    public override void Close(object info)
    {
      base.Close(info);

      _model.OnClosing(info);
      SaveToolSettings();
    }

    private void LoadSettings()
    {
      //Set the preferred height, width and location of the tool based on previous session values
      ResourceBrowserToolSettings fightEditorToolSettings = Container.Resolve<ResourceBrowserToolSettings>();
      this.PreferredHeight = fightEditorToolSettings.PreferredHeight;
      this.PreferredWidth = fightEditorToolSettings.PreferredWidth;
      this.PreferredLocation = fightEditorToolSettings.PreferredLocation;
      this.FocusOnRibbonOnClick = fightEditorToolSettings.FocusOnRibbonOnClick;
      this.KeepRibbonTabActive = fightEditorToolSettings.KeepRibbonTabActive;
      this._model.DataPaneWidth = fightEditorToolSettings.DataPaneWidth;
    }
    #endregion

    #region Events
    void SaveToolSettings()
    {
      IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();
      eventAggregator.GetEvent<ResourceBrowserSettingsChangeEvent>().Publish(this);
    }
    #endregion
  }
}
