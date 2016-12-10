using Magnum.Core;
using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.Views;
using Magnum.Core.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Windows.Controls.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace Magnum.Core.ViewModels
{
  /// <summary>
  /// The abstract class which has to be inherited if you want to create a tool
  /// </summary>
  public class ToolViewModel : ViewModelBase, IToolWindow, ISearchable
  {
    #region Fields
    private readonly IUnityContainer _container;
    private IWorkspace _workspace;
    protected string _Name = string.Empty;
    protected string _ContentId = string.Empty;
    protected string _RibbonHeader = string.Empty;
    protected bool _IsActive = false;
    protected bool _IsSelected = false;
    private bool _IsVisible = true;
    protected string _Title = string.Empty;
    private ToolModel _Model = null;
    protected PaneLocation _PreferredLocation = PaneLocation.Bottom;
    protected double _PreferredWidth = 200;
    protected double _PreferredHeight = 200;
    protected bool _FocusOnRibbonOnClick = true;
    protected bool _KeepRibbonTabActive = false;
    private string _DisplayName = string.Empty;
    private string _Category;
    private string _Description = string.Empty;
    private bool _CanOpenMultipleInstances = false;
    #endregion

    #region Constructors
    public ToolViewModel(IUnityContainer container, WorkspaceBase workspace)
    {
      _workspace = workspace;
      _container = container;

      _Category = "Tools";

      CloseCommand = new UndoableCommand<object>(Close, CanClose);
      OpenCommand = new UndoableCommand<object>(Open, CanOpen);
      HideCommand = new UndoableCommand<object>(Hide, CanHide);
      SaveCommand = new UndoableCommand<object>(Save, CanSave);

      _workspace.SearchableItems.Add(this);
    }
    #endregion

    #region Properties

    [Browsable(false)]
    public IUnityContainer Container
    {
      get { return _container; }
    }

    [Browsable(false)]
    public IWorkspace Workspace
    {
      get { return _workspace; }
      set { _workspace = value; }
    }

    [Browsable(false)]
    public LayoutAnchorableItem LayoutAnchorableItem { get; set; }

    /// <summary>
    /// Gets or sets the close command.
    /// </summary>
    /// <value>The close command.</value>
    [Browsable(false)]
    public virtual IUndoableCommand CloseCommand { get; set; }

    /// <summary>
    /// Gets or sets the open command.
    /// </summary>
    /// <value>The open command.</value>
    [Browsable(false)]
    public virtual IUndoableCommand OpenCommand { get; set; }

    /// <summary>
    /// Gets or sets the hide command.
    /// </summary>
    /// <value>The hide command.</value>
    [Browsable(false)]
    public virtual IUndoableCommand HideCommand { get; set; }

    /// <summary>
    /// Gets or sets the save command.
    /// </summary>
    /// <value>The save command.</value>
    [Browsable(false)]
    public virtual IUndoableCommand SaveCommand { get; set; }

    /// <summary>
    /// The name of the tool.
    /// </summary>
    [Description("The name of the tool.")]
    [ReadOnly(true)]
    public string Name
    {
      get { return _Name; }
      set
      {
        if (_Name != value)
        {
          _Name = value;
          RaisePropertyChanged("Name");
          DisplayName = _Name;
        }
      }
    }

    public event EventHandler OnPreferredLocationChanged;

    /// <summary>
    /// The preferred location of the tool.
    /// </summary>
    [Description("The preferred location of the tool.")]
    public PaneLocation PreferredLocation
    {
      get { return _PreferredLocation; }
      set
      {
        if (_PreferredLocation != value)
        {
          _PreferredLocation = value;
          RaisePropertyChanged("PreferredLocation");

          var handler = OnPreferredLocationChanged;
          if (handler != null)
            handler(this, null);
        }
      }
    }

    public event EventHandler OnPreferredWidthChanged;

    /// <summary>
    /// The preferred width of the tool.
    /// </summary>
    [Description("The preferred width of the tool.")]
    public virtual double PreferredWidth
    {
      get { return _PreferredWidth; }
      set
      {
        if (_PreferredWidth != value)
        {
          _PreferredWidth = value;
          RaisePropertyChanged("PreferredWidth");

          var handler = OnPreferredWidthChanged;
          if (handler != null)
            handler(this, null);
        }
      }
    }

    public event EventHandler OnPreferredHeightChanged;
    /// <summary>
    /// The preferred height of the tool.
    /// </summary>
    [Description("The preferred height of the tool.")]
    public virtual double PreferredHeight
    {
      get { return _PreferredHeight; }
      set
      {
        if (_PreferredHeight != value)
        {
          _PreferredHeight = value;
          RaisePropertyChanged("PreferredHeight");

          var handler = OnPreferredHeightChanged;
          if (handler != null)
            handler(this, null);
        }
      }
    }

    /// <summary>
    /// The content model.
    /// </summary>
    /// <value>The model.</value>
    [Description("The content model.")]
    [Browsable(false)]
    public virtual ToolModel Model
    {
      get { return _Model; }
      set
      {
        if (_Model != value)
        {
          _Model = value;
          _Model.PropertyChanged += _Model_PropertyChanged;

          // Show the standard group first, and the misc group last.
          OnCategoryChanged();
          OnShortcutChanged();
        }
      }
    }

    /// <summary>
    /// The document view.
    /// </summary>
    /// <value>The view.</value>
    [Description("The document view.")]
    [Browsable(false)]
    public virtual IDocumentView View { get; set; }

    /// <summary>
    /// The title of the document.
    /// </summary>
    /// <value>The title.</value>
    [Description("The title of the document.")]
    [Browsable(false)]
    public virtual string Title
    {
      get
      {
        if (Model.IsDirty)
        {
          return _Title + "*";
        }
        return _Title;
      }
      set
      {
        if (_Title != value)
        {
          _Title = value;
          RaisePropertyChanged("Title");
        }
      }
    }

    /// <summary>
    /// The image source that can be used as an icon in the tab
    /// </summary>
    /// <value>The icon source.</value>
    [Description("The image source that can be used as an icon in the tab.")]
    [Browsable(false)]
    public virtual ImageSource IconSource { get; set; }

    /// <summary>
    /// The content ID - unique value for each document.
    /// </summary>
    /// <value>The content id.</value>
    [Description("The content ID - unique value for each document.")]
    [Browsable(false)]
    public virtual string ContentId
    {
      get { return _ContentId; }
      protected set
      {
        if (_ContentId != value)
        {
          _ContentId = value;
          RaisePropertyChanged("ContentId");
        }
      }
    }

    /// <summary>
    /// The ribbon header - unique value for each tool.
    /// </summary>
    /// <value>The content id.</value>
    [Description("The ribbon header - unique value for each tool.")]
    [Browsable(false)]
    public virtual string RibbonHeader
    {
      get { return _RibbonHeader; }
      protected set
      {
        if (_RibbonHeader != value)
        {
          _RibbonHeader = value;
          RaisePropertyChanged("RibbonHeader");
        }
      }
    }

    /// <summary>
    /// Is the document selected.
    /// </summary>
    /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
    [Description("Is the document selected.")]
    [Browsable(false)]
    public virtual bool IsSelected
    {
      get { return _IsSelected; }
      set
      {
        _IsSelected = value;
        RaisePropertyChanged("IsSelected");
      }
    }

    public event EventHandler OnActiveChanged;
    /// <summary>
    /// Is the document active.
    /// </summary>
    /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
    [Description("Is the document active.")]
    [Browsable(false)]
    public virtual bool IsActive
    {
      get { return _IsActive; }
      set
      {
        _IsActive = value;
        RaisePropertyChanged("IsActive");

        if (ApplicationModel.Application.IsInitialized)
        {
          var ribbonService = Container.Resolve<IRibbonService>();
          foreach (RibbonTab rt in ribbonService.Ribbon.Items)
          {
            if (rt.Header.ToString() == RibbonHeader)
              if (KeepRibbonTabActive)
              {
                rt.Visibility = System.Windows.Visibility.Visible;
                if (FocusOnRibbonOnClick)
                  rt.IsSelected = true;
              }
              else if (IsVisible && IsActive)
              {
                rt.Visibility = System.Windows.Visibility.Visible;
                if (FocusOnRibbonOnClick)
                  rt.IsSelected = true;
              }
              else
                rt.Visibility = System.Windows.Visibility.Collapsed;
          }
        }

        var handler = OnActiveChanged;
        if (handler != null)
          handler(this, null);
      }
    }

    public event EventHandler OnVisibleChanged;

    /// <summary>
    /// The visibility of the tool.
    /// </summary>
    [Description("The visibility of the tool.")]
    [Browsable(false)]
    public bool IsVisible
    {
      get { return _IsVisible; }
      set
      {
        _IsVisible = value;
        RaisePropertyChanged("IsVisible");

        if (ApplicationModel.Application.IsInitialized)
        {
          var ribbonService = Container.Resolve<IRibbonService>();
          foreach (RibbonTab rt in ribbonService.Ribbon.Items)
          {
            if (rt.Header.ToString() == RibbonHeader)
              if (!IsVisible)
                rt.Visibility = System.Windows.Visibility.Collapsed;
              else if (KeepRibbonTabActive)
                rt.Visibility = System.Windows.Visibility.Visible;
          }
        }

        var handler = OnVisibleChanged;
        if (handler != null)
          handler(this, null);
      }
    }

    public event EventHandler OnFocusOnRibbonOnClickChanged;
    /// <summary>
    /// Focus on the contextual ribbon group when the tool becomes active.
    /// </summary>
    [Description("Focus on the contextual ribbon group when the tool becomes active.")]
    public virtual bool FocusOnRibbonOnClick
    {
      get { return _FocusOnRibbonOnClick; }
      set
      {
        if (_FocusOnRibbonOnClick != value)
        {
          _FocusOnRibbonOnClick = value;
          RaisePropertyChanged("FocusOnRibbonOnClick");

          var handler = OnFocusOnRibbonOnClickChanged;
          if (handler != null)
            handler(this, null);
        }
      }
    }

    public event EventHandler OnKeepRibbonTabActiveChanged;
    /// <summary>
    /// Keeps the contextual ribbon group active.
    /// </summary>
    [Description("Keeps the contextual ribbon group active.")]
    public virtual bool KeepRibbonTabActive
    {
      get { return _KeepRibbonTabActive; }
      set
      {
        if (_KeepRibbonTabActive != value)
        {
          _KeepRibbonTabActive = value;
          RaisePropertyChanged("KeepRibbonTabActive");

          var handler = OnKeepRibbonTabActiveChanged;
          if (handler != null)
            handler(this, null);
        }
      }
    }

    /// <summary>
    /// Some categories must appear before others. This is used to 
    /// correctly sort the groups.
    /// </summary>
    [Browsable(false)]
    public string PrioritySorting { get; private set; }

    [Browsable(false)]
    public Type AssociatedType { get { return Model.ToolControlType; } }

    [Browsable(false)]
    public Visibility Visibility
    {
      get { return IsVisible ? Visibility.Visible : Visibility.Collapsed; }
    }

    [Browsable(false)]
    public string ShortcutText { get; private set; }

    [Browsable(false)]
    public bool HasShortcutText
    {
      get { return !string.IsNullOrEmpty(ShortcutText); }
    }

    [Browsable(false)]
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

    [Browsable(false)]
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

    [Browsable(false)]
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

    [Browsable(false)]
    public bool CanOpenMultipleInstances
    {
      get { return _CanOpenMultipleInstances; }
      set
      {
        if (_CanOpenMultipleInstances != value)
        {
          _CanOpenMultipleInstances = value;
          RaisePropertyChanged("CanOpenMultipleInstances");
        }
      }
    }
    #endregion

    #region Methods
    private void OnCategoryChanged()
    {
      if (Model.Category == "Standard")
        PrioritySorting = "A";
      else if (Model.Category == "Miscellaneous")
        PrioritySorting = "C";
      else
        PrioritySorting = "B";

      RaisePropertyChanged("PrioritySorting");
    }

    private void OnShortcutChanged()
    {
      //TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(KeyGesture));
      //ShortcutText = tc.ConvertToString(Model.Shortcut);
      ShortcutText = Model.Shortcut;
      RaisePropertyChanged("ShortcutText");
      RaisePropertyChanged("HasShortcutText");
    }

    /// <summary>
    /// Determines whether this instance can close.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><c>true</c> if this instance can close; otherwise, <c>false</c>.</returns>
    protected virtual bool CanClose(object obj)
    {
      return true;
    }

    /// <summary>
    /// Closes this instance.
    /// 
    /// Focuses on the last active tool.
    /// TODO(ndistefano): This should definitely go in some WindowManager class or something...
    /// </summary>
    /// <param name="obj">The object.</param>
    public virtual void Close(object obj)
    {
      IWorkspace workspace = _container.Resolve<WorkspaceBase>();
      if (workspace != null && workspace.LastActiveTool != null && workspace.LastActiveTool.ContentId != ContentId)
      {
        ApplicationModel.Application.DockingManager.ActiveContent = workspace.LastActiveTool;
      }
      else
      {
        foreach (ToolViewModel tvm in workspace.Tools)
        {
          // Focus on any tool that is visible
          if (tvm.IsVisible)
          {
            ApplicationModel.Application.DockingManager.ActiveContent = tvm;
            break;
          }
        }
      }
    }

    /// <summary>
    /// Determines whether this instance can be opened.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><c>true</c> if this instance can be opened; otherwise, <c>false</c>.</returns>
    protected virtual bool CanOpen(object obj)
    {
      return true;
    }

    /// <summary>
    /// Opens this instance.
    /// </summary>
    /// <param name="obj">The object.</param>
    public virtual void Open(object obj)
    {
    }

    /// <summary>
    /// Determines whether this instance can be hidden.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><c>true</c> if this instance can be hidden; otherwise, <c>false</c>.</returns>
    protected virtual bool CanHide(object obj)
    {
      return true;
    }

    /// <summary>
    /// Hides this instance. This is called when we hide a ToolViewModel inside LayoutAnchorables in AvalonDock.
    /// Binded in the style in MainWindow.xaml.
    /// 
    /// Focuses on the last active tool.
    /// TODO(ndistefano): This should definitely go in some WindowManager class or something...
    /// </summary>
    /// <param name="obj">The object.</param>
    public virtual void Hide(object obj)
    {
      IWorkspace workspace = _container.Resolve<WorkspaceBase>();
      if (workspace != null && workspace.LastActiveTool != null && workspace.LastActiveTool.ContentId != ContentId)
      {
        ApplicationModel.Application.DockingManager.ActiveContent = workspace.LastActiveTool;
      }
      else
      {
        foreach (ToolViewModel tvm in workspace.Tools)
        {
          // Focus on any tool that is visible
          if (tvm.IsVisible)
          {
            ApplicationModel.Application.DockingManager.ActiveContent = tvm;
            break;
          }
        }
      }
      (LayoutAnchorableItem.LayoutElement as LayoutAnchorable).Hide();
    }
    
    /// <summary>
    /// Determines whether this instance can be saved.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><c>true</c> if this instance can be saved; otherwise, <c>false</c>.</returns>
    protected virtual bool CanSave(object obj)
    {
      return true;
    }

    /// <summary>
    /// Saves this instance.
    /// </summary>
    /// <param name="obj">The object.</param>
    public virtual void Save(object obj)
    {
    }
    #endregion

    #region Events
    void _Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "IsVisible")
        RaisePropertyChanged("Visibility");
      else if (e.PropertyName == "Category")
        OnCategoryChanged();
      else if (e.PropertyName == "Shortcut")
        OnShortcutChanged();
    }
    #endregion

    #region ISearchable Members
    [Browsable(false)]
    public string AssignedTool { get; set; }

    public void Open()
    {
      if (OpenCommand != null)
        OpenCommand.Execute(null);
    }

    #endregion
  }
}
