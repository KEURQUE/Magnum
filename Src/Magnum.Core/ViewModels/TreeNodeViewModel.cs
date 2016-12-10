using Magnum.Core.Services;
using Magnum.Core.Windows;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Magnum.Core.ViewModels
{
  public class TreeNodeViewModel : ViewModelBase, ITreeNodeItem, ISearchable
  {
    private readonly ObservableCollection<ITreeNodeItem> _children;
    private readonly string _title;

    private TreeNodeViewModel _parent;
    private bool _expanded;
    private bool _searched;
    private bool _match = true;
    private ContextMenu _contextMenu;
    private bool _isSelected;
    private int _id;
    private IItemType _itemType;
    private BitmapSource _icon;
    private IUnityContainer _container;
    private string _Value = String.Empty;

    public TreeNodeViewModel(string title, int id, IItemType itemType, BitmapImage icon, IUnityContainer container, IEnumerable<ITreeNodeItem> children = null)
    {
      this._title = title;
      this._id = id;
      this._itemType = itemType;
      this._icon = icon;
      this._container = container;
      if (children != null)
      {
        this._children = new ObservableCollection<ITreeNodeItem>(children);
      }
      else
      {
        this._children = new ObservableCollection<ITreeNodeItem>(Enumerable.Empty<ITreeNodeItem>());
      }
      this._children.CollectionChanged += _children_CollectionChanged;
      
      IWorkspace workspace = _container.Resolve<WorkspaceBase>();
      workspace.SearchableItems.Add(this);

      DisplayName = _title;
      IconSource = _icon;
      Category = "< New Item >";
      Description = _itemType.ToString();
    }

    public TreeNodeViewModel(string title, IEnumerable<ITreeNodeItem> children)
      : this(title, -1, IItemType.Unknown, null, null, children)
    {
      this._title = title;
    }

    public TreeNodeViewModel(string title)
      : this(title, -1, IItemType.Unknown, null, null, Enumerable.Empty<ITreeNodeItem>())
    {
    }

    public TreeNodeViewModel()
      : this("", -1, IItemType.Unknown, null, null, Enumerable.Empty<ITreeNodeItem>())
    {
    }

    public override string ToString()
    {
      return _title;
    }

    public virtual bool IsCriteriaMatched(string criteria)
    {
      return String.IsNullOrEmpty(criteria) || String.IsNullOrWhiteSpace(criteria) || _title.Contains(criteria);
    }

    public void Add(ITreeNodeItem newItem)
    {
      this._children.Add(newItem);
    }

    public void ApplyCriteria(string criteria, Stack<ITreeNodeItem> ancestors)
    {
      if (IsCriteriaMatched(criteria))
      {
        Searched = (!String.IsNullOrEmpty(criteria) || !String.IsNullOrWhiteSpace(criteria));
        IsMatch = true;
        foreach (var ancestor in ancestors)
        {
          ancestor.IsMatch = true;
          ancestor.IsExpanded = !String.IsNullOrEmpty(criteria);
        }
      }
      else
      {
        IsMatch = false;
        Searched = false;
      }

      ancestors.Push(this);
      foreach (var child in Children)
        child.ApplyCriteria(criteria, ancestors);

      if (String.IsNullOrEmpty(criteria) && ancestors.Count == 1)
        this.IsExpanded = true;
      ancestors.Pop();
    }

    [Browsable(false)]
    public ObservableCollection<ITreeNodeItem> Children
    {
      get { return _children; }
    }

    [ReadOnly(true)]
    public string Title
    {
      get { return _title; }
    }

    [Browsable(false)]
    public int Id
    {
      get
      {
        return _id;
      }
      set
      {
        if (_id != value)
        {
          _id = value;
          RaisePropertyChanged("Id");
        }
      }
    }

    /// <summary>
    /// Used for the breadcrumb mostly
    /// </summary>
    [Browsable(false)]
    [ReadOnly(true)]
    public TreeNodeViewModel Parent
    {
      get
      {
        return _parent;
      }
      set
      {
        if (_parent != value)
        {
          _parent = value;
          RaisePropertyChanged("Parent");
        }
      }
    }

    [ReadOnly(true)]
    public IItemType ItemType
    {
      get
      {
        return _itemType;
      }
      set
      {
        if (_itemType != value)
        {
          _itemType = value;
          RaisePropertyChanged("ItemType");
        }
      }
    }

    [Browsable(false)]
    public BitmapSource Icon
    {
      get
      {
        return _icon;
      }
      set
      {
        if (_icon != value)
        {
          _icon = value;
          RaisePropertyChanged("Icon");
        }
      }
    }

    [Browsable(false)]
    public bool IsExpanded
    {
      get { return _expanded; }
      set
      {
        if (value == _expanded)
          return;

        _expanded = value;
        if (_expanded)
        {
          foreach (var child in Children)
            child.IsMatch = true;
        }
        RaisePropertyChanged("IsExpanded");
      }
    }

    [Browsable(false)]
    public virtual bool IsSelected
    {
      get
      {
        return _isSelected;
      }
      set
      {
        if (_isSelected != value)
        {
          _isSelected = value;
          RaisePropertyChanged("IsSelected");
        }
      }
    }

    [Browsable(false)]
    public bool Searched
    {
      get { return _searched; }
      set
      {
        if (value == _searched)
          return;

        _searched = value;
        RaisePropertyChanged("Searched");
      }
    }

    [Browsable(false)]
    public bool IsMatch
    {
      get { return _match; }
      set
      {
        if (value == _match)
          return;

        _match = value;
        RaisePropertyChanged("IsMatch");
      }
    }

    [Browsable(false)]
    public bool IsLeaf
    {
      get { return !Children.Any(); }
    }

    [Browsable(false)]
    public ContextMenu ContextMenu
    {
      get
      {
        return _contextMenu;
      }
      set
      {
        if (_contextMenu != value)
        {
          _contextMenu = value;
          RaisePropertyChanged("ContextMenu");
        }
      }
    }

    /// <summary>
    /// Used for the breadcrumb mostly
    /// </summary>
    [Browsable(false)]
    public string Value
    {
      get { return _Value; }
      set
      {
        _Value = value;
        RaisePropertyChanged("Value");
      }
    }

    #region ISearchable Members
    [Browsable(false)]
    public string AssignedTool { get; set; }

    [Browsable(false)]
    public string DisplayName { get; set; }

    [Browsable(false)]
    public string Category { get; set; }

    [Browsable(false)]
    public string Description { get; set; }

    [Browsable(false)]
    public ImageSource IconSource { get; set; }

    [Browsable(false)]
    public IUndoableCommand OpenCommand { get; set; }

    public void Open()
    {
      if (OpenCommand != null)
        OpenCommand.Execute(null);
    }
    #endregion

    #region Events

    void _children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
      {
        foreach (var item in e.NewItems.OfType<TreeNodeViewModel>())
        {
          item.Parent = this;
          item.Value = (this.Value + "\\" + Title).TrimStart('\\');
        }
      }
      else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
      {
        foreach (var item in e.OldItems.OfType<TreeNodeViewModel>())
        {
          item.Parent = null;
        }
      }
    }

    #endregion
  }
}
