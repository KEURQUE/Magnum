using Engine.System.Interfaces;
using Engine.Wrappers.Services;
using Magnum.Core.Models;
using Magnum.Core.Services;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Collections.Generic;
using Magnum.Controls.PropertyGrid;

namespace Magnum.Tools.PropertyGrid.Models
{
  public class PropertyGridModel : ToolModel, IPropertyGrid
  {
    #region Fields
    ISelectionManagerService _selectionManager;
    private object _selectedObject;
    private string _selectedObjectName;
    private bool _hasASelectedObject;
    Stack<object> _previouslySelectedItems;
    Stack<object> _selectedItemsBeforeGoingBack;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyGridModel"/> class.
    /// </summary>
    public PropertyGridModel(string displayName, IUnityContainer container, string category, string description, bool isPinned = true, string shortcut = null)
      : base(displayName, container, category, description, isPinned, shortcut)
    {
      _previouslySelectedItems = new Stack<object>();
      _selectedItemsBeforeGoingBack = new Stack<object>();

      _selectionManager = container.Resolve<ISelectionManagerService>();
      _selectionManager.OnSelectedObjectChanged += RefreshSelectedObject;
      _selectionManager.OnSelectedObjectsChanged += RefreshSelectedObjects;

      BackCommand = new UndoableCommand<object>(GoBack, CanGoBack);
      NextCommand = new UndoableCommand<object>(GoNext, CanGoNext);
    }
    #endregion

    #region Properties
    public object SelectedObject
    {
      get
      {
        return _selectedObject;
      }
      set
      {
        if (_selectedObject != value)
        {
          if (_selectedObject != null && value != null)
          {
            _previouslySelectedItems.Push(_selectedObject);
          }
          _selectedObject = value;
          
          if (value != null)
            HasASelectedObject = true;
          else
            HasASelectedObject = false;

          RaisePropertyChanged("SelectedObject");
        }
      }
    }

    public string SelectedObjectName
    {
      get
      {
        return _selectedObjectName;
      }
      set
      {
        if (_selectedObjectName != value)
        {
          _selectedObjectName = value;
          RaisePropertyChanged("SelectedObjectName");
        }
      }
    }

    public bool HasASelectedObject
    {
      get
      {
        return _hasASelectedObject;
      }
      set
      {
        if (_hasASelectedObject != value)
        {
          _hasASelectedObject = value;
          RaisePropertyChanged("HasASelectedObject");
        }
      }
    }
    #endregion

    public override void GoBack(object obj)
    {
        _selectedItemsBeforeGoingBack.Push(SelectedObject);
        SelectedObject = _previouslySelectedItems.Pop();
    }

    protected override bool CanGoBack(object obj)
    {
      if (_previouslySelectedItems.Count > 0)
        return true;
      else
        return false;
    }

    public override void GoNext(object obj)
    {
      SelectedObject = _selectedItemsBeforeGoingBack.Pop();
    }

    protected override bool CanGoNext(object obj)
    {
      if (_selectedItemsBeforeGoingBack.Count > 0 && _selectedItemsBeforeGoingBack.First() != SelectedObject)
        return true;
      else
        return false;
    }

    #region Events
    void RefreshSelectedObject(object sender, EventArgs e)
    {
      if (_selectionManager != null)
      {
        SelectedObject = _selectionManager.SelectedObject;
        SelectedObjectName = String.Empty;
        if (SelectedObject is ISelectable)
          SelectedObjectName = (SelectedObject as ISelectable).SelectionName;
      }
      else
      {
        SelectedObject = null;
        SelectedObjectName = String.Empty;
        if (SelectedObject is ISelectable)
          SelectedObjectName = (SelectedObject as ISelectable).SelectionName;
      }
    }

    void RefreshSelectedObjects(object sender, EventArgs e)
    {
      if (_selectionManager != null && _selectionManager.SelectedObjects.Count > 0)
      {
        SelectedObject = _selectionManager.SelectedObjects[0];
        SelectedObjectName = String.Empty;
        if (SelectedObject is ISelectable)
          SelectedObjectName = (SelectedObject as ISelectable).SelectionName;
      }
      else if (_selectionManager != null && _selectionManager.SelectedObject != null)
      {
        SelectedObject = _selectionManager.SelectedObject;
        SelectedObjectName = String.Empty;
        if (SelectedObject is ISelectable)
          SelectedObjectName = (SelectedObject as ISelectable).SelectionName;
      }
      else
      {
        SelectedObject = null;
        SelectedObjectName = String.Empty;
      }
    }
    #endregion
  }
}
