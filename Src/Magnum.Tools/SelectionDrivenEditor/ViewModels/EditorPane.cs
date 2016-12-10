using Magnum.Core.Services;
using Magnum.Tools.SelectionDrivenEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magnum.Core.Extensions;
using System.Collections.Specialized;

namespace Magnum.Tools.SelectionDrivenEditor.ViewModels
{
  public class EditorPane : IEditorPane
  {
    #region Fields

    private SelectionDrivenEditorViewModel _viewModel;
    private ObservableCollection<ObjectTreeNode> _items;
    private ObservableCollection<ITreeNodeItem> _selection;
    private SelectionDrivenEditor.ViewModels.SelectionDrivenEditorViewModel.EditorTypeEnum _editorType;

    #endregion

    #region Contructors

    public EditorPane(SelectionDrivenEditorViewModel viewModel, SelectionDrivenEditor.ViewModels.SelectionDrivenEditorViewModel.EditorTypeEnum editorType = SelectionDrivenEditor.ViewModels.SelectionDrivenEditorViewModel.EditorTypeEnum.DataPane)
    {
      _viewModel = viewModel;

      _items = new ObservableCollection<ObjectTreeNode>();
      _items.CollectionChanged += _items_CollectionChanged;
      _selection = new ObservableCollection<ITreeNodeItem>();
      _selection.CollectionChanged += _selection_CollectionChanged;

      _editorType = editorType;
    }

    #endregion

    #region Properties

    public SelectionDrivenEditorViewModel ViewModel
    {
      get { return _viewModel; }
      set
      {
        _viewModel = value;
      }
    }

    public ObservableCollection<ObjectTreeNode> Items
    {
      get
      {
        return _items;
      }
      set
      {
        _items = value;
      }
    }


    public ObservableCollection<ITreeNodeItem> Selection
    {
      get
      {
        return _selection;
      }
      set
      {
        _selection = value;
      }
    }

    public SelectionDrivenEditor.ViewModels.SelectionDrivenEditorViewModel.EditorTypeEnum EditorType
    {
      get
      {
        return _editorType;
      }
      set
      {
        _editorType = value;
      }
    }

    #endregion

    #region Methods

    public void SetSelection(IEnumerable<ITreeNodeItem> objectsToSelect, SelectionSnapshot snapshot)
    {
      Selection.Clear();
      Selection.AddRange(objectsToSelect);
    }

    #endregion

    #region Events

    private void _items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
    }

    private void _selection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
        ViewModel.SetEditorSelection(new SelectionSnapshot(ViewModel));
    }

    #endregion

    public List<ITreeNodeItem> FindItems(List<ITreeNodeItem> foundItems = null, ITreeNodeItem currentItem = null)
    {
      if (foundItems == null)
        foundItems = new List<ITreeNodeItem>();

      if (currentItem == null)
        currentItem = Items.FirstOrDefault() as ITreeNodeItem;

      if (currentItem == null)
        return new List<ITreeNodeItem>();

      foreach (var item in currentItem.Children)
      {
        if (item.IsSelected)
          foundItems.Add(item);
        foundItems.AddRange(item.Children.Where(x => x.IsSelected));

        foreach (var child in item.Children)
          FindItems(foundItems, child);
      }
      return foundItems;
    }
  }
}
