using Magnum.Core;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Magnum.Core.Models
{
  public class ToolModel : ViewModelBase, IModel
  {
    #region Fields
    private string _DisplayName = string.Empty;
    private string _Category = string.Empty;
    private string _Description = string.Empty;
    private string _Shortcut = string.Empty;
    private IUnityContainer _container;
    private bool _IsDirty;
    private bool _IsSearchBoxVisible = true;
    private bool _IsBreadcrumbVisible = true;
    private string _FilterText = string.Empty;

    Stack<ITreeNodeItem> _previouslySelectedItems;
    ITreeNodeItem _currentlySelectedItem;
    Stack<ITreeNodeItem> _selectedItemsBeforeGoingBack;
    private IUndoableCommand _BackCommand = null;
    private IUndoableCommand _NextCommand = null;
    #endregion

    #region Constructors
    public ToolModel(string displayName, IUnityContainer container, string category = "Miscellaneous", string description = "None Available", bool isPinned = true, string shortcut = null)
    {
      DisplayName = displayName;
      Container = container;
      Category = category;
      Description = description;
      IsPinned = isPinned;
      Shortcut = shortcut;

      _previouslySelectedItems = new Stack<ITreeNodeItem>();
      _selectedItemsBeforeGoingBack = new Stack<ITreeNodeItem>();
      BackCommand = new UndoableCommand<object>(GoBack, CanGoBack);
      NextCommand = new UndoableCommand<object>(GoNext, CanGoNext);
    }

    public ToolModel()
    {
      DisplayName = "Unnamed Tool";
      Category = "Miscellaneous";
      Description = "None Available";
      IsPinned = true;
      Shortcut = null;

      _previouslySelectedItems = new Stack<ITreeNodeItem>();
      _selectedItemsBeforeGoingBack = new Stack<ITreeNodeItem>();
      BackCommand = new UndoableCommand<object>(GoBack, CanGoBack);
      NextCommand = new UndoableCommand<object>(GoNext, CanGoNext);
    }
    #endregion

    #region Properties

    [Browsable(false)]
    public IUnityContainer Container
    {
      get { return _container; }
      set { _container = value; }
    }

    public string DisplayName
    {
      get { return _DisplayName; }
      set
      {
        if (_DisplayName != value)
        {
          _DisplayName = value;
          RaisePropertyChanged("DisplayName");
        }
      }
    }

    public string Category
    {
      get { return _Category; }
      set
      {
        if (_Category != value)
        {
          _Category = value;
          RaisePropertyChanged("Category");
        }
      }
    }

    public string Description
    {
      get { return _Description; }
      set
      {
        if (_Description != value)
        {
          _Description = value;
          RaisePropertyChanged("Description");
        }
      }
    }

    /// <summary>
    /// Gets or sets the go back command.
    /// </summary>
    /// <value>The go back command.</value>
    [Browsable(false)]
    public virtual IUndoableCommand BackCommand
    {
      get { return _BackCommand; }
      set
      {
        if (_BackCommand != value && value != null)
        {
          _BackCommand = value;
          RaisePropertyChanged("BackCommand");

          /*var commandManager = Container.Resolve<ICommandManager>();
          if (commandManager != null)
            commandManager.RegisterCommand(this.DisplayName.ToUpper() + "BACKCOMMAND", value);*/
        }
      }
    }

    /// <summary>
    /// Gets or sets the go next command.
    /// </summary>
    /// <value>The go next command.</value>
    [Browsable(false)]
    public virtual IUndoableCommand NextCommand
    {
      get { return _NextCommand; }
      set
      {
        if (_NextCommand != value && value != null)
        {
          _NextCommand = value;
          RaisePropertyChanged("NextCommand");

          /*var commandManager = Container.Resolve<ICommandManager>();
          if (commandManager != null)
            commandManager.RegisterCommand(this.DisplayName.ToUpper() + "NEXTCOMMAND", value);*/
        }
      }
    }

    public bool IsSearchBoxVisible
    {
      get { return _IsSearchBoxVisible; }
      set
      {
        _IsSearchBoxVisible = value;
        RaisePropertyChanged("IsSearchBoxVisible");
      }
    }

    public bool IsBreadcrumbVisible
    {
      get { return _IsBreadcrumbVisible; }
      set
      {
        _IsBreadcrumbVisible = value;
        RaisePropertyChanged("IsBreadcrumbVisible");
      }
    }

    /// <summary>
    /// Used for the search text boxes in module toolbars
    /// </summary>
    [Browsable(false)]
    public virtual string FilterText
    {
      get { return _FilterText; }
      set
      {
        _FilterText = value;
        RaisePropertyChanged("FilterText");
        ApplyFilter();
      }
    }

    #region IModel Members
    /// <summary>
    /// Is the document dirty - does it need to be saved?
    /// </summary>
    public virtual bool IsDirty
    {
      get { return _IsDirty; }
      set
      {
        _IsDirty = value;
        RaisePropertyChanged("IsDirty");
      }
    }

    public virtual object Location { get; set; }
    #endregion

    public bool IsPinned { get; set; }

    public Type ToolControlType { get; set; }

    public string Shortcut
    {
      get { return _Shortcut; }
      set
      {
        if (_Shortcut != value)
        {
          _Shortcut = value;
          RaisePropertyChanged("Shortcut");
        }
      }
    }

    private object _Root;
    public object Root
    {
      get { return _Root; }
      set
      {
        if (_Root != value)
        {
          _Root = value;
          RaisePropertyChanged("Root");
        }
      }
    }

    public event EventHandler OnCurrentlySelectedItemChanged;
    public ITreeNodeItem CurrentlySelectedItem
    {
      get { return _currentlySelectedItem; }
      set
      {
        if (_currentlySelectedItem != value)
        {
          if (_currentlySelectedItem != null && value != null)
          {
            _currentlySelectedItem.IsSelected = false;
            _previouslySelectedItems.Push(_currentlySelectedItem);
          }
          _currentlySelectedItem = value;
          if (_currentlySelectedItem != null)
            _currentlySelectedItem.IsSelected = true;
          RaisePropertyChanged("CurrentlySelectedItem");

          var handler = OnCurrentlySelectedItemChanged;
          if (handler != null)
            handler(this, null);

          BackCommand.RaiseCanExecuteChanged();
          NextCommand.RaiseCanExecuteChanged();
        }
      }
    }
    #endregion

    #region Methods
    public virtual void ApplyFilter()
    {
    }

    /// <summary>
    /// Determines whether this instance can go back.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><c>true</c> if this instance can go back; otherwise, <c>false</c>.</returns>
    protected virtual bool CanGoBack(object obj)
    {
      if (_previouslySelectedItems.Count > 0)
        return true;
      else
        return false;
    }

    /// <summary>
    /// Go back.
    /// </summary>
    /// <param name="obj">The object.</param>
    public virtual void GoBack(object obj)
    {
      _selectedItemsBeforeGoingBack.Push(CurrentlySelectedItem);
      CurrentlySelectedItem = _previouslySelectedItems.Pop();
    }

    /// <summary>
    /// Determines whether this instance can go next.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><c>true</c> if this instance can go next; otherwise, <c>false</c>.</returns>
    protected virtual bool CanGoNext(object obj)
    {
      if (_selectedItemsBeforeGoingBack.Count > 0 && _selectedItemsBeforeGoingBack.First() != CurrentlySelectedItem)
        return true;
      else
        return false;
    }

    /// <summary>
    /// Go next.
    /// </summary>
    /// <param name="obj">The object.</param>
    public virtual void GoNext(object obj)
    {
      CurrentlySelectedItem = _selectedItemsBeforeGoingBack.Pop();
    }
    #endregion
  }
}
