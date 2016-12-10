using Magnum.Core.Services;
using Magnum.Core.Utils;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Magnum.IconLibrary;
using Magnum.Tools.SelectionDrivenEditor.Models;
using Magnum.Tools.SelectionDrivenEditor.ViewModels;
using Microsoft.Practices.Unity;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Magnum.Tools.ResourceBrowser.Models
{
  public class ResourceBrowserItem : ObjectTreeNode
  {
    #region Constructors
    public ResourceBrowserItem()
     : base(){ }

    public ResourceBrowserItem(IEditorPane editor, string title, int id, DirectoryInfo path, IItemType itemType, BitmapImage icon, IUnityContainer container)
      : base(editor, title, id, itemType, icon, container, new ObservableCollection<ResourceBrowserItem>())
    {
      Path = path;
      InitializeFolderItem();
    }
    #endregion

    #region Properties

    public DirectoryInfo Path { get; set; }

    private ObservableCollection<ResourceBrowserItem> _Files = new ObservableCollection<ResourceBrowserItem>();
    public ObservableCollection<ResourceBrowserItem> Files
    {
      get { return _Files; }
      set { _Files = value; }
    }

    #endregion

    #region Methods

    private void InitializeFolderItem()
    {
      if ((IItemType)this.ItemType == IItemType.Folder)
        IsExpanded = true;
    }

    public override bool IsCriteriaMatched(string criteria)
    {
      if (StringUtils.SimpleMatch(this.DisplayName, criteria))
        return true;

      return false;
    }

    #endregion
  }
}
