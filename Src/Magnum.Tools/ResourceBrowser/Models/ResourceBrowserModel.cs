using Engine.System.Entities;
using Magnum.Core.Managers;
using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Magnum.IconLibrary;
using Magnum.Tools.PropertyGrid.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using Magnum.Core.Extensions;
using Engine.Wrappers.Services;
using Magnum.Core.Utils;
using Magnum.Tools.SelectionDrivenEditor.ViewModels;
using Magnum.Tools.SelectionDrivenEditor.Models;
using Magnum.Core.Args;
using Magnum.Controls.Breadcrumb;

namespace Magnum.Tools.ResourceBrowser.Models
{
  class ResourceBrowserModel : SelectionDrivenEditorViewModel
  {
    double _dataPaneWidth;

    public ResourceBrowserModel(string displayName, IUnityContainer container, string category, string description, bool isPinned = true, string shortcut = null)
      : base(displayName, container, category, description, isPinned, shortcut)
    {
      Container = container;
      _dataPaneWidth = 350.0;

      InitializeCommands();

      DataPane = new EditorPane(this);

      InitializeTreeview();
    }

    #region Properties

    public double DataPaneWidth
    {
      get { return _dataPaneWidth; }
      set
      {
        _dataPaneWidth = value;
        RaisePropertyChanged("DataPaneWidth");
      }
    }

    #endregion

    #region Methods
    private void OpenSelectedItem(object obj)
    {
      var workspace = Container.Resolve<WorkspaceBase>();
      if (workspace != null)
      {
        ToolViewModel tvm = workspace.Tools.First(f => f.ContentId == "ResourceBrowser");
        if (tvm != null)
        {
          if (!tvm.IsActive)
          {
            tvm.IsActive = true;
          }
        }
      }
    }

    public override void ApplyFilter()
    {
      foreach (var node in DataPane.Items)
        node.ApplyCriteria(FilterText, new Stack<ITreeNodeItem>());
    }

    private void InitializeCommands()
    {
      var commandManager = Container.Resolve<ICommandManager>();
      var ribbonService = Container.Resolve<IRibbonService>();

      // Ribbon
      var editSettingsCommand = new UndoableCommand<object>(EditSettings);
      commandManager.RegisterCommand("EDITRESOURCEBROWSERSETTINGS", editSettingsCommand);

      ribbonService.Add(new RibbonItemViewModel("RESOURCE BROWSER", 1, "F", true));
      ribbonService.Get("RESOURCE BROWSER").Add(new RibbonItemViewModel("Settings", 3));
      ribbonService.Get("RESOURCE BROWSER").Get("Settings").Add(new MenuItemViewModel("Edit Settings", 1,
                                                            BitmapImages.LoadBitmapFromResourceKey("Settings_16x16"),
                                                            commandManager.GetCommand("EDITRESOURCEBROWSERSETTINGS"), "EDITRESOURCEBROWSERSETTINGS", null, false, null,
                                                            String.Empty, "Edit tool settings."));
    }

    public void EditSettings(object obj)
    {
      var selectionManager = Container.Resolve<ISelectionManagerService>();
      if (selectionManager != null)
        selectionManager.SelectObject(Container.Resolve<IResourceBrowser>());
    }

    public void InitializeTreeview()
    {
      DirectoryInfo diTop = new DirectoryInfo(new Uri(@"..\..\Editor\Debug\Content", UriKind.RelativeOrAbsolute).ToString());
      ResourceBrowserItem rootItem = new ResourceBrowserItem(DataPane, "Root", -1, diTop, IItemType.Folder, BitmapImages.LoadBitmapFromIconType(IconType.Folder), Container);

      AddResourceBrowserItems(rootItem);

      DataPane.Items.Add(rootItem);
    }

    private void AddResourceBrowserItems(ResourceBrowserItem rootItem)
    {
      try
      {
        foreach (var fi in rootItem.Path.EnumerateFiles())
        {
          try
          {
            ResourceBrowserItem childItem = AddChildItem(fi);

            if (childItem != null)
              rootItem.Children.Add(childItem);
          }
          catch (UnauthorizedAccessException UnAuthTop)
          {
            Console.WriteLine("{0}", UnAuthTop.Message);
          }
        }

        AddDirectory(rootItem);
      }
      catch (DirectoryNotFoundException DirNotFound)
      {
        Console.WriteLine("{0}", DirNotFound.Message);
      }
      catch (UnauthorizedAccessException UnAuthDir)
      {
        Console.WriteLine("UnAuthDir: {0}", UnAuthDir.Message);
      }
      catch (PathTooLongException LongPath)
      {
        Console.WriteLine("{0}", LongPath.Message);
      }
    }

    private void AddDirectory(ResourceBrowserItem rootItem)
    {
      foreach (var di in rootItem.Path.EnumerateDirectories("*"))
      {
        ResourceBrowserItem folderItem = new ResourceBrowserItem(DataPane, di.Name, di.GetHashCode(), di, IItemType.Folder, BitmapImages.LoadBitmapFromIconType(IconType.Folder), Container);

        AddDirectory(folderItem);

        try
        {
          foreach (var fi in di.EnumerateFiles("*"))
          {
            try
            {
              ResourceBrowserItem childItem = AddChildItem(fi);

              if (childItem != null)
                folderItem.Files.Add(childItem);
            }
            catch (UnauthorizedAccessException UnAuthFile)
            {
              Console.WriteLine("UnAuthFile: {0}", UnAuthFile.Message);
            }
          }
        }
        catch (UnauthorizedAccessException UnAuthSubDir)
        {
          Console.WriteLine("UnAuthSubDir: {0}", UnAuthSubDir.Message);
        }

        rootItem.Add(folderItem);
      }
    }

    private ResourceBrowserItem AddChildItem(FileInfo fi)
    {
      ResourceBrowserItem childItem = null;

      if (fi.Extension.Contains("png"))
        childItem = new ResourceBrowserItem(EditorPane, fi.Name, fi.GetHashCode(), fi.Directory, IItemType.Texture, BitmapImages.LoadBitmapFromIconType(IconType.Texture), Container);
      else if (fi.Extension.Contains("xnb"))
        childItem = new ResourceBrowserItem(EditorPane, fi.Name, fi.GetHashCode(), fi.Directory, IItemType.Model, BitmapImages.LoadBitmapFromIconType(IconType.Model), Container);
      else if (fi.Extension.Contains("xml"))
        childItem = new ResourceBrowserItem(EditorPane, fi.Name, fi.GetHashCode(), fi.Directory, IItemType.Entity, BitmapImages.LoadBitmapFromIconType(IconType.Entity), Container);

      return childItem;
    }

    public WindowClosingEventArgs OnClosing(object info)
    {
      IWorkspace workspace = Container.Resolve<WorkspaceBase>();
      var statusBar = Container.Resolve<IStatusBarService>();
      var saveManager = Container.Resolve<ISaveManagerService>();
      WindowClosingEventArgs e = info as WindowClosingEventArgs;

      if (e == null) // This means we closed the Resource Browser via the X button and not by closing the MainWindow
      {
        e = new WindowClosingEventArgs(true);

        ToolViewModel viewer = workspace.Tools.First(f => f.ContentId == "ResourceBrowser");
        if (viewer != null)
        {
          viewer.IsVisible = false;
          viewer.IsActive = false;
        }
      }
      return e;
    }
    #endregion
  }
}
