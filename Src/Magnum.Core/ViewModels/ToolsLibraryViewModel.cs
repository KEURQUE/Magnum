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
  /// Represents the current collection of tools known to Revolver.
  /// </summary>
  public class ToolsLibraryViewModel : ViewModelBase, IToolsLibrary
  {
    /// <summary>
    /// The workspace instance
    /// </summary>
    protected IWorkspace _workspace;

    public ToolsLibraryViewModel(WorkspaceBase workspace)
    {
      _workspace = workspace;

      _ToolsViewSource = (ListCollectionView)CollectionViewSource.GetDefaultView(_Tools);
      _ToolsViewSource.SortDescriptions.Add(new SortDescription("PrioritySorting", ListSortDirection.Ascending));
      _ToolsViewSource.SortDescriptions.Add(new SortDescription("Model.Category", ListSortDirection.Ascending));
      _ToolsViewSource.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
      _ToolsViewSource.GroupDescriptions.Add(new PropertyGroupDescription("Model.Category"));
      _ToolsViewSource.Filter = CustomFilter;

      PinnedTools = new ObservableCollection<ToolViewModel>();

      ApplicationModel.Application.OnIsInitializedChanged += Application_OnIsInitializedChanged;
    }

    void Application_OnIsInitializedChanged(object sender, EventArgs e)
    {
      RefreshTools();
      RefreshPinnedTools();
    }

    private bool CustomFilter(object item)
    {
      ToolViewModel pi = item as ToolViewModel;
      if (pi != null)
      {
        if (pi.ContentId == "Error List")
          return false;
        if (StringUtils.SimpleMatch(pi.Model.DisplayName, FilterText))
          return true;
        if (StringUtils.SimpleMatch(pi.Model.Category, FilterText))
          return true;

        return false;
      }

      return true;
    }

    private readonly ListCollectionView _ToolsViewSource;

    public ListCollectionView ToolsViewSource
    {
      get { return _ToolsViewSource; }
    }

    private readonly ObservableCollection<ToolViewModel> _Tools = new ObservableCollection<ToolViewModel>();

    private string _FilterText = string.Empty;

    public string FilterText
    {
      get { return _FilterText; }
      set
      {
        _FilterText = value;
        RaisePropertyChanged("FilterText");
        _ToolsViewSource.Refresh();
      }
    }

    public IWorkspace Workspace
    {
      get { return _workspace; }
      set { _workspace = value; }
    }

    private void RefreshTools()
    {
      _Tools.Clear();

      foreach (ToolViewModel toolViewModel in _workspace.Tools)
      {
        toolViewModel.Model.PropertyChanged += toolDescriptor_PropertyChanged;
        _Tools.Add(toolViewModel);
      }
    }

    void toolDescriptor_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "IsPinned")
      {
        RefreshPinnedTools();
      }
    }

    public ObservableCollection<ToolViewModel> PinnedTools { get; private set; }

    private void RefreshPinnedTools()
    {
      PinnedTools.Clear();
      PinnedTools.AddRange(from tvm in _Tools where tvm.Model.IsPinned select tvm);
    }
  }
}
