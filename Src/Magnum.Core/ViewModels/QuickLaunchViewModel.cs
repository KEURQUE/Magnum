using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Diagnostics;
using Magnum.Core;
using Microsoft.Practices.Prism.Events;
using Magnum.Core.Extensions;
using Magnum.Core.Services;
using Magnum.Core.Windows;
using Magnum.Core.Models;
using Magnum.Core.Utils;

namespace Magnum.Core.ViewModels
{
  /// <summary>
  /// Represents the current collection of searchable items known to Revolver.
  /// </summary>
  public class QuickLaunchViewModel : ViewModelBase, IQuickLaunch
  {
    /// <summary>
    /// The workspace instance
    /// </summary>
    protected IWorkspace _workspace;

    public QuickLaunchViewModel(WorkspaceBase workspace)
    {
      _workspace = workspace;

      _SearchableItemsViewSource = CollectionViewSource.GetDefaultView(_SearchableItems);
      _SearchableItemsViewSource.SortDescriptions.Add(new SortDescription("DisplayName", ListSortDirection.Ascending));
      _SearchableItemsViewSource.SortDescriptions.Add(new SortDescription("Category", ListSortDirection.Ascending));
      _SearchableItemsViewSource.SortDescriptions.Add(new SortDescription("Description", ListSortDirection.Ascending));
      _SearchableItemsViewSource.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
      _SearchableItemsViewSource.Filter = CustomFilter;

      ApplicationModel.Application.OnIsInitializedChanged += Application_OnIsInitializedChanged;
    }

    void Application_OnIsInitializedChanged(object sender, EventArgs e)
    {
      RefreshItems();
    }

    private bool CustomFilter(object item)
    {
      ISearchable pi = item as ISearchable;
      if (pi != null)
      {
        if (pi.DisplayName == "Error List")
          return false;
        if (StringUtils.SimpleMatch(pi.DisplayName, FilterText))
          return true;
        if (StringUtils.SimpleMatch(pi.Category, FilterText))
          return true;
        if (StringUtils.SimpleMatch(pi.Description, FilterText))
          return true;

        return false;
      }

      return false;
    }

    private readonly ICollectionView _SearchableItemsViewSource;

    public ICollectionView SearchableItemsViewSource
    {
      get { return _SearchableItemsViewSource; }
    }

    private readonly ObservableCollection<ISearchable> _SearchableItems = new ObservableCollection<ISearchable>();

    private string _FilterText = string.Empty;

    public string FilterText
    {
      get { return _FilterText; }
      set
      {
        _FilterText = value;
        RaisePropertyChanged("FilterText");
        _SearchableItemsViewSource.Refresh();

        if (!String.IsNullOrEmpty(_FilterText) && !_workspace.IsQuickLaunchOpened)
          _workspace.IsQuickLaunchOpened = true;
        else if (String.IsNullOrEmpty(_FilterText))
          _workspace.IsQuickLaunchOpened = false;
      }
    }

    public IWorkspace Workspace
    {
      get { return _workspace; }
      set { _workspace = value; }
    }

    private void RefreshItems()
    {
      _SearchableItems.Clear();

      foreach (ISearchable item in _workspace.SearchableItems)
        _SearchableItems.Add(item);
    }
  }
}
