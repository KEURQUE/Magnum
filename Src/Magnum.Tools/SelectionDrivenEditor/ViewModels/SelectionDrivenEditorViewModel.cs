using Engine.Wrappers.Services;
using Magnum.Core.Models;
using Magnum.Core.ViewModels;
using Magnum.Tools.SelectionDrivenEditor.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Tools.SelectionDrivenEditor.ViewModels
{
  public class SelectionDrivenEditorViewModel : ToolModel
  {
    private EditorTypeEnum _activePane;
    private IEditorPane _dataPane;
    private IEditorPane _editorPane;
    private IEditorPane _contextualPane;

    public SelectionDrivenEditorViewModel(string displayName, IUnityContainer container, string category, string description, bool isPinned = true, string shortcut = null)
      : base(displayName, container, category, description, isPinned, shortcut)
    {
      OnCurrentlySelectedItemChanged += SelectionDrivenEditorViewModel_OnCurrentlySelectedItemChanged;
    }

    public SelectionDrivenEditorViewModel()
      : base()
    {
      OnCurrentlySelectedItemChanged += SelectionDrivenEditorViewModel_OnCurrentlySelectedItemChanged;
    }

    #region Properties

    public enum EditorTypeEnum
    {
      DataPane,
      EditorPane,
      ContextualPane
    }

    public EditorTypeEnum ActivePane
    {
      get
      {
        return _activePane;
      }
      set
      {
        _activePane = value;
      }
    }

    /// <summary>
    /// Cannot be null
    /// </summary>
    public IEditorPane DataPane
    {
      get { return _dataPane; }
      set
      {
        _dataPane = value;
        _dataPane.Items.CollectionChanged += Items_CollectionChanged;
        RaisePropertyChanged("DataPane");
      }
    }

    void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      // Assign the Root item of the Breadcrumb here
      if (DataPane.Items.Count >= 1 && Root == null)
      {
        Root = DataPane.Items.FirstOrDefault();
      }
    }

    public IEditorPane EditorPane
    {
      get { return _editorPane; }
      set
      {
        _editorPane = value;
        RaisePropertyChanged("EditorPane");
      }
    }

    public IEditorPane ContextualPane
    {
      get { return _contextualPane; }
      set
      {
        _contextualPane = value;
        RaisePropertyChanged("ContextualPane");
      }
    }

    #endregion

    #region Methods

    public void SetEditorSelection(SelectionSnapshot selectionSnapshot)
    {
      UpdateDataPaneSelection(selectionSnapshot);
    }

    private void UpdateDataPaneSelection(SelectionSnapshot selectionSnapshot)
    {
      bool selectionShouldChange = true;

      foreach (var alreadySelectedItem in selectionSnapshot.DataPaneSelection)
      {
        if (DataPane.Selection.Any(x => x == alreadySelectedItem))
          selectionShouldChange = false;
      }
      if (selectionShouldChange)
      {
        DataPane.SetSelection(selectionSnapshot.DataPaneSelection, selectionSnapshot);
        ActivePane = EditorTypeEnum.DataPane;
      }

      if (selectionSnapshot.DataPaneSelection.Count() > 0)
        UpdateEditorPaneSelection(selectionSnapshot);
      else
        EditorPane = null;
    }

    private void UpdateEditorPaneSelection(SelectionSnapshot selectionSnapshot)
    {
      if (ActivePane == EditorTypeEnum.DataPane)
      {
        if (EditorPane == null)
        {
          EditorPane = new EditorPane(this, EditorTypeEnum.EditorPane);
          EditorPane.SetSelection(selectionSnapshot.EditorPaneSelection, selectionSnapshot);
          ActivePane = EditorTypeEnum.EditorPane;
        }
        else
        {
          bool selectionShouldChange = true;

          foreach (var alreadySelectedItem in selectionSnapshot.EditorPaneSelection)
          {
            if (EditorPane.Selection.Any(x => x == alreadySelectedItem))
              selectionShouldChange = false;
          }
          if (selectionShouldChange)
          {
            EditorPane.SetSelection(selectionSnapshot.EditorPaneSelection, selectionSnapshot);
            ActivePane = EditorTypeEnum.EditorPane;
          }
        }
      }

      if (selectionSnapshot.EditorPaneSelection.Count() > 0)
        UpdateContextualPaneSelection(selectionSnapshot);
      else
        ContextualPane = null;
    }

    private void UpdateContextualPaneSelection(SelectionSnapshot selectionSnapshot)
    {
      if (ActivePane == EditorTypeEnum.EditorPane)
      {
        if (ContextualPane == null)
        {
          ContextualPane = new EditorPane(this, EditorTypeEnum.ContextualPane);
          foreach (var child in DataPane.Items.OfType<ObjectTreeNode>().Where(x => x.Editor.EditorType == EditorTypeEnum.ContextualPane))
          {
            EditorPane.Items.Add(child);
          }
          ContextualPane.SetSelection(selectionSnapshot.ContextualPaneSelection, selectionSnapshot);
          ActivePane = EditorTypeEnum.ContextualPane;
        }
        else
        {
          bool selectionShouldChange = true;

          if (ContextualPane == null)
            ContextualPane = new EditorPane(this);

          foreach (var alreadySelectedItem in selectionSnapshot.ContextualPaneSelection)
          {
            if (ContextualPane.Selection.Any(x => x == alreadySelectedItem))
              selectionShouldChange = false;
          }
          if (selectionShouldChange)
          {
            ContextualPane.SetSelection(selectionSnapshot.ContextualPaneSelection, selectionSnapshot);
            ActivePane = EditorTypeEnum.ContextualPane;
          }
        }
      }
    }

    #endregion

    #region Events

    private void SelectionDrivenEditorViewModel_OnCurrentlySelectedItemChanged(object sender, EventArgs e)
    {
      var selectionManager = Container.Resolve<ISelectionManagerService>();
      if (selectionManager != null)
        selectionManager.SelectObject(CurrentlySelectedItem);
    }

    #endregion
  }
}
