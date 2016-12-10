using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Magnum.IconLibrary;
using Magnum.Tools.PropertyGrid.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Magnum.Core.Extensions;
using Magnum.Core.ApplicationModel;
using Engine.Wrappers.Services;

namespace Magnum.Tools.ErrorList.Models
{
  public class ErrorListModel : ToolModel
  {
    #region Fields
    private readonly ObservableCollection<ErrorListItem> _items;
    private readonly ListCollectionView _ItemsViewSource;
    private bool _showErrors = true;
    private bool _showWarnings = true;
    private bool _showMessages = true;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorListModel"/> class.
    /// </summary>
    public ErrorListModel(string displayName, IUnityContainer container, string category, string description, bool isPinned = true, string shortcut = null)
      : base(displayName, container, category, description, isPinned, shortcut)
    {
      _items = new ObservableCollection<ErrorListItem>();
      _items.Add(new ErrorListItem
      {
        ItemType = ErrorListItemType.Error,
        Number = 1,
        Description = "This is an error.",
        Path = "File1.txt",
        Line = 42,
        Column = 24
      });
      _items.Add(new ErrorListItem
      {
        ItemType = ErrorListItemType.Warning,
        Number = 2,
        Description = "This is a warning.",
        Path = "File1.txt",
        Line = 1,
        Column = 2
      });
      _items.Add(new ErrorListItem
      {
        ItemType = ErrorListItemType.Message,
        Number = 3,
        Description = "This is a message.",
      });

      _ItemsViewSource = (ListCollectionView)CollectionViewSource.GetDefaultView(_items);
      _ItemsViewSource.SortDescriptions.Add(new SortDescription("ItemType", ListSortDirection.Ascending));
      _ItemsViewSource.GroupDescriptions.Add(new PropertyGroupDescription("ItemType"));
      _ItemsViewSource.Filter = CustomFilter;

      InitializeCommands();
    }
    #endregion

    #region Properties
    public ObservableCollection<ErrorListItem> Items
    {
      get { return _items; }
    }

    public ListCollectionView FilteredItems
    {
      get { return _ItemsViewSource; }
    }

    public bool ShowErrors
    {
      get { return _showErrors; }
      set
      {
        _showErrors = value;
        _ItemsViewSource.Refresh();

        // Update the ribbon
        var ribbonService = Container.Resolve<IRibbonService>();
        MenuItemViewModel mvm = ribbonService.Get("ERROR LIST").Get("Filter").Get("Toggle Show Errors") as MenuItemViewModel;

        if (mvm != null)
        {
          mvm.Label = ErrorItemCount > 0 ? ErrorItemCount + " Errors" : "Errors";
          mvm.IsChecked = _showErrors;
        }

        RaisePropertyChanged("ShowErrors");
        RaisePropertyChanged("FilteredItems");
      }
    }

    public bool ShowWarnings
    {
      get { return _showWarnings; }
      set
      {
        _showWarnings = value;
        _ItemsViewSource.Refresh();

        // Update the ribbon
        var ribbonService = Container.Resolve<IRibbonService>();
        MenuItemViewModel mvm = ribbonService.Get("ERROR LIST").Get("Filter").Get("Toggle Show Warnings") as MenuItemViewModel;

        if (mvm != null)
        {
          mvm.Label = WarningItemCount > 0 ? WarningItemCount + " Warnings" : "Warnings";
          mvm.IsChecked = _showWarnings;
        }

        RaisePropertyChanged("ShowWarnings");
        RaisePropertyChanged("FilteredItems");
      }
    }

    public bool ShowMessages
    {
      get { return _showMessages; }
      set
      {
        _showMessages = value;
        _ItemsViewSource.Refresh();

        // Update the ribbon
        var ribbonService = Container.Resolve<IRibbonService>();
        MenuItemViewModel mvm = ribbonService.Get("ERROR LIST").Get("Filter").Get("Toggle Show Messages") as MenuItemViewModel;

        if (mvm != null)
        {
          mvm.Label = MessageItemCount > 0 ? MessageItemCount + " Messages" : "Messages";
          mvm.IsChecked = _showMessages;
        }

        RaisePropertyChanged("ShowMessages");
        RaisePropertyChanged("FilteredItems");
      }
    }

    public int ErrorItemCount
    {
      get
      {
        var count = 0;
        foreach (ErrorListItem item in _items)
        {
          if (item.ItemType == ErrorListItemType.Error)
            count++;
        }
        return count;
      }
    }

    public int WarningItemCount
    {
      get
      {
        var count = 0;
        foreach (ErrorListItem item in _items)
        {
          if (item.ItemType == ErrorListItemType.Warning)
            count++;
        }
        return count;
      }
    }

    public int MessageItemCount
    {
      get
      {
        var count = 0;
        foreach (ErrorListItem item in _items)
        {
          if (item.ItemType == ErrorListItemType.Message)
            count++;
        }
        return count;
      }
    }
    #endregion

    #region Methods
    private bool CustomFilter(object item)
    {
      ErrorListItem pi = item as ErrorListItem;
      if (pi != null)
      {
        if (pi.ItemType == ErrorListItemType.Error && !ShowErrors)
          return false;
        else if (pi.ItemType == ErrorListItemType.Warning && !ShowWarnings)
          return false;
        else if (pi.ItemType == ErrorListItemType.Message && !ShowMessages)
          return false;
      }

      return true;
    }

    public void AddItem(ErrorListItemType itemType, string description,
            string path = null, int? line = null, int? column = null,
            Action onClick = null)
    {
      _ItemsViewSource.AddNewItem(new ErrorListItem(itemType, Items.Count + 1, description, path, line, column)
      {
        OnClick = onClick
      });
    }

    private void InitializeCommands()
    {
      var commandManager = Container.Resolve<ICommandManager>();
      var ribbonService = Container.Resolve<IRibbonService>();
      var menuService = Container.Resolve<IMenuService>();

      // Ribbon
      var toggleShowErrorsCommand = new UndoableCommand<object>(ToggleShowErrors);
      commandManager.RegisterCommand("TOGGLESHOWERRORS", toggleShowErrorsCommand);
      var toggleShowWarningsCommand = new UndoableCommand<object>(ToggleShowWarnings);
      commandManager.RegisterCommand("TOGGLESHOWWARNINGS", toggleShowWarningsCommand);
      var toggleShowMessagesCommand = new UndoableCommand<object>(ToggleShowMessages);
      commandManager.RegisterCommand("TOGGLESHOWMESSAGES", toggleShowMessagesCommand);

      ribbonService.Add(new RibbonItemViewModel("ERROR LIST", 10, "M", true));
      ribbonService.Get("ERROR LIST").Add(new RibbonItemViewModel("Filter", 1));
      ribbonService.Get("ERROR LIST").Get("Filter").Add(new MenuItemViewModel("Toggle Show Errors", 1,
                                                            BitmapImages.LoadBitmapFromResourceKey("ErrorColor_16x16"),
                                                            commandManager.GetCommand("TOGGLESHOWERRORS"), "TOGGLESHOWERRORS",
                                                            new KeyGesture(Key.D0, ModifierKeys.Control, "Ctrl + 0"), false, null,
                                                            "Errors", "Shows errors.") { IsCheckable = true, IsChecked = ShowErrors });
      ribbonService.Get("ERROR LIST").Get("Filter").Add(new MenuItemViewModel("Toggle Show Warnings", 2,
                                                            BitmapImages.LoadBitmapFromResourceKey("WarningColor_16x16"),
                                                            commandManager.GetCommand("TOGGLESHOWWARNINGS"), "TOGGLESHOWWARNINGS",
                                                            new KeyGesture(Key.OemMinus, ModifierKeys.Control, "Ctrl + -"), false, null,
                                                            "Warnings", "Shows warnings.") { IsCheckable = true, IsChecked = ShowWarnings });
      ribbonService.Get("ERROR LIST").Get("Filter").Add(new MenuItemViewModel("Toggle Show Messages", 3,
                                                            BitmapImages.LoadBitmapFromResourceKey("MessageColor_16x16"),
                                                            commandManager.GetCommand("TOGGLESHOWMESSAGES"), "TOGGLESHOWMESSAGES",
                                                            new KeyGesture(Key.OemPlus, ModifierKeys.Control, "Ctrl + ="), false, null,
                                                            "Messages", "Shows messages.") { IsCheckable = true, IsChecked = ShowMessages });

      var editSettingsCommand = new UndoableCommand<object>(EditSettings);
      commandManager.RegisterCommand("EDITERRORLISTSETTINGS", editSettingsCommand);

      ribbonService.Get("ERROR LIST").Add(new RibbonItemViewModel("Settings", 2));
      ribbonService.Get("ERROR LIST").Get("Settings").Add(new MenuItemViewModel("Edit Settings", 1,
                                                            BitmapImages.LoadBitmapFromResourceKey("Settings_16x16"),
                                                            commandManager.GetCommand("EDITERRORLISTSETTINGS"), "EDITERRORLISTSETTINGS", null, false, null,
                                                            String.Empty, "Edit tool settings."));
    }

    private void ToggleShowErrors(object obj)
    {
      IWorkspace workspace = Container.Resolve<WorkspaceBase>();

      ShowErrors = !ShowErrors;

      ToolViewModel errorList = workspace.Tools.First(f => f.ContentId == "Error List");
      if (errorList != null)
        errorList.FocusOnRibbonOnClick = false;
    }

    private void ToggleShowWarnings(object obj)
    {
      IWorkspace workspace = Container.Resolve<WorkspaceBase>();

      ShowWarnings = !ShowWarnings;
      
      ToolViewModel errorList = workspace.Tools.First(f => f.ContentId == "Error List");
      if (errorList != null)
        errorList.FocusOnRibbonOnClick = false;
    }

    private void ToggleShowMessages(object obj)
    {
      IWorkspace workspace = Container.Resolve<WorkspaceBase>();

      ShowMessages = !ShowMessages;

      ToolViewModel errorList = workspace.Tools.First(f => f.ContentId == "Error List");
      if (errorList != null)
        errorList.FocusOnRibbonOnClick = false;
    }

    public void EditSettings(object obj)
    {
      // TODO(ndistefano): Remove reference to 357.Wrappers and 357.System, or simply move this Tool to Revolver.Tools
      var selectionManager = Container.Resolve<ISelectionManagerService>();
      if (selectionManager != null)
        selectionManager.SelectObject(Container.Resolve<IErrorList>());
    }
    #endregion
  }
}