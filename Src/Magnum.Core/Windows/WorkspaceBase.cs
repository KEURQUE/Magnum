using Magnum.Controls.Services;
using Magnum.Core;
using Magnum.Core.Args;
using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Microsoft.Practices.Unity;
using Microsoft.Windows.Controls.Ribbon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace Magnum.Core.Windows
{
  /// <summary>
  /// Class WorkspaceBase
  /// </summary>
  public abstract class WorkspaceBase : ViewModelBase, IWorkspace
  {
    #region Fields
    /// <summary>
    /// The injected container
    /// </summary>
    private readonly IUnityContainer _container;
    /// <summary>
    /// The active document
    /// </summary>
    private DocumentViewModel _activeDocument;
    /// <summary>
    /// The last active tool
    /// </summary>
    private ToolViewModel _lastActiveTool;
    /// <summary>
    /// The active tool
    /// </summary>
    private ToolViewModel _activeTool;
    /// <summary>
    /// The injected command manager
    /// </summary>
    protected ICommandManager _commandManager;
    /// <summary>
    /// The list of documents
    /// </summary>
    protected ObservableCollection<DocumentViewModel> _docs = new ObservableCollection<DocumentViewModel>();
    /// <summary>
    /// The menu service
    /// </summary>
    protected MenuItemViewModel _menus;
    /// <summary>
    /// The tools library service
    /// </summary>
    protected IToolsLibrary _toolsLibrary;
    /// <summary>
    /// The quick launch service
    /// </summary>
    protected IQuickLaunch _quickLaunch;
    /// <summary>
    /// The ribbon service
    /// </summary>
    protected IRibbonService _ribbonService;
    /// <summary>
    /// The status bar service
    /// </summary>
    protected IStatusBarService _statusbarService;
    /// <summary>
    /// The progress bar service
    /// </summary>
    protected IProgressBarService _progressbarService;
    /// <summary>
    /// The list of tools
    /// </summary>
    protected ObservableCollection<ToolViewModel> _tools = new ObservableCollection<ToolViewModel>();
    /// <summary>
    /// Is the main window busy
    /// </summary>
    protected bool _isBusy;
    /// <summary>
    /// Is the main window showing a message box
    /// </summary>
    private bool _isMessageBoxShowing;
    /// <summary>
    /// Indicates if a new error/warning/message has appeared
    /// </summary>
    protected bool _notifyError;
    /// <summary>
    /// Is the tools library opened
    /// </summary>
    protected bool _isToolsLibraryOpened;
    /// <summary>
    /// Is the quick launch opened
    /// </summary>
    protected bool _isQuickLaunchOpened;
    /// <summary>
    /// The list of searchable items
    /// </summary>
    protected ObservableCollection<ISearchable> _searchableItems = new ObservableCollection<ISearchable>();
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceBase"/> class.
    /// </summary>
    /// <param name="container">The injected container.</param>
    protected WorkspaceBase(IUnityContainer container)
    {
      _container = container;
      _docs = new ObservableCollection<DocumentViewModel>();
      _docs.CollectionChanged += _docs_CollectionChanged;
      _tools = new ObservableCollection<ToolViewModel>();
      _menus = _container.Resolve<IMenuService>() as MenuItemViewModel;
      _menus.PropertyChanged += _menus_PropertyChanged;
      _ribbonService = _container.Resolve<IRibbonService>();
      _statusbarService = _container.Resolve<IStatusBarService>();
      _progressbarService = _container.Resolve<IProgressBarService>();
      _commandManager = _container.Resolve<ICommandManager>();
      _isBusy = false;
      _isMessageBoxShowing = false;
      _notifyError = false;
      _isToolsLibraryOpened = false;
      _isQuickLaunchOpened = false;
      _searchableItems = new ObservableCollection<ISearchable>();
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the menu.
    /// </summary>
    /// <value>The menu.</value>
    public IList<CommandableBase> Menus
    {
      get { return _menus.Children; }
    }

    /// <summary>
    /// Gets the ribbon.
    /// </summary>
    /// <value>The ribbon.</value>
    public Ribbon Ribbon
    {
      get { return _ribbonService.Ribbon; }
    }

    /// <summary>
    /// Gets the tools library.
    /// </summary>
    /// <value>The tools library.</value>
    public IToolsLibrary ToolsLibrary
    {
      get { return _toolsLibrary; }
      set
      {
        if (_toolsLibrary != value)
          _toolsLibrary = value;
      }
    }
    
    /// <summary>
    /// Gets the quick launch.
    /// </summary>
    /// <value>The quick launch.</value>
    public IQuickLaunch QuickLaunch
    {
      get { return _quickLaunch; }
      set
      {
        if (_quickLaunch != value)
          _quickLaunch = value;
      }
    }

    /// <summary>
    /// Gets the progress bar.
    /// </summary>
    /// <value>The progress bar.</value>
    public IProgressBarService ProgressBar
    {
      get { return _progressbarService; }
    }

    /// <summary>
    /// Gets the status bar.
    /// </summary>
    /// <value>The status bar.</value>
    public IStatusBarService StatusBar
    {
      get { return _statusbarService; }
    }
    #endregion

    #region Methods
    #endregion

    #region Events
    /// <summary>
    /// Handles the PropertyChanged event of the menu control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    private void _menus_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      RaisePropertyChanged("Menus");
    }

    void _docs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        if (_docs.Count == 0)
          _activeDocument = null;
      }
    }
    #endregion

    #region IWorkspace Members
    /// <summary>
    /// The list of documents which are open in the workspace
    /// </summary>
    /// <value>The documents.</value>
    public ObservableCollection<DocumentViewModel> Documents
    {
      get { return _docs; }
      set { _docs = value; }
    }

    /// <summary>
    /// The list of tools that are available in the workspace
    /// </summary>
    /// <value>The tools.</value>
    public ObservableCollection<ToolViewModel> Tools
    {
      get { return _tools; }
      set { _tools = value; }
    }

    /// <summary>
    /// Gets the searchable items.
    /// </summary>
    /// <value>The tools library.</value>
    public ObservableCollection<ISearchable> SearchableItems
    {
      get { return _searchableItems; }
      set
      {
        if (_searchableItems != value)
          _searchableItems = value;
      }
    }

    /// <summary>
    /// The current document which is active in the workspace
    /// </summary>
    /// <value>The active document.</value>
    public DocumentViewModel ActiveDocument
    {
      get { return _activeDocument; }
      set
      {
        if (_activeDocument != value)
        {
          _activeDocument = value;
          RaisePropertyChanged("ActiveDocument");
          _commandManager.Refresh();
          _menus.Refresh();
          //TODO: Implement pub/sub Active document changed event
        }
      }
    }

    /// <summary>
    /// The last tool that was active in the workspace
    /// </summary>
    /// <value>The last active tool.</value>
    public ToolViewModel LastActiveTool
    {
      get { return _lastActiveTool; }
      set
      {
        if (_lastActiveTool != value)
        {
          _lastActiveTool = value;
          RaisePropertyChanged("LastActiveTool");
        }
      }
    }

    /// <summary>
    /// The current tool which is active in the workspace
    /// </summary>
    /// <value>The active tool.</value>
    public ToolViewModel ActiveTool
    {
      get { return _activeTool; }
      set
      {
        if (_activeTool != value)
        {
          _activeTool = value;
          RaisePropertyChanged("ActiveTool");
          _commandManager.Refresh();
          _menus.Refresh();
          //TODO: Implement pub/sub Active tool changed event
        }
      }
    }

    /// <summary>
    /// Gets the title of the application.
    /// </summary>
    /// <value>The title.</value>
    public virtual string Title
    {
      get { return "Revolver"; }
    }

    /// <summary>
    /// Gets the icon of the application.
    /// </summary>
    /// <value>The icon.</value>
    public virtual ImageSource Icon { get; protected set; }

    public virtual bool IsBusy
    {
      get
      {
        return _isBusy;
      }
      set
      {
        if (_isBusy != value)
          _isBusy = value;
        RaisePropertyChanged("IsBusy");
      }
    }

    public virtual bool IsMessageBoxShowing
    {
      get
      {
        return _isMessageBoxShowing;
      }
      set
      {
        if (_isMessageBoxShowing != value)
          _isMessageBoxShowing = value;
        RaisePropertyChanged("IsMessageBoxShowing");
      }
    }

    public virtual bool NotifyError
    {
      get
      {
        return _notifyError;
      }
      set
      {
        if (_notifyError != value)
          _notifyError = value;
        RaisePropertyChanged("NotifyError");
      }
    }

    public virtual bool IsToolsLibraryOpened
    {
      get
      {
        return _isToolsLibraryOpened;
      }
      set
      {
        if (_isToolsLibraryOpened != value)
          _isToolsLibraryOpened = value;
        RaisePropertyChanged("IsToolsLibraryOpened");
      }
    }

    public virtual bool IsQuickLaunchOpened
    {
      get
      {
        return _isQuickLaunchOpened;
      }
      set
      {
        if (_isQuickLaunchOpened != value)
          _isQuickLaunchOpened = value;
        RaisePropertyChanged("IsQuickLaunchOpened");
      }
    }

    /// <summary>
    /// Closing this instance.
    /// </summary>
    /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
    /// <returns><c>true</c> if the application is closing, <c>false</c> otherwise</returns>
    public virtual bool Closing(CancelEventArgs e)
    {
      for (int i = 0; i < Documents.Count; i++)
      {
        DocumentViewModel vm = Documents[i];
        if (vm.Model.IsDirty)
        {
          ActiveDocument = vm;

          //Execute the close command
          vm.CloseCommand.Execute(e);

          //If canceled
          if (e.Cancel == true)
          {
            return false;
          }

          // This can happen only when a view model with no location was closed
          if (Documents.Count > 0 && vm != Documents[i])
          {
            //Closed the document - now reduce the count as Documents.Count would have decreased.
            i--;
          }
        }
      }

      // Save all tool settings
      for (int i = 0; i < Tools.Count; i++)
      {
        ToolViewModel tvm = Tools[i];

        //Execute the close command
        WindowClosingEventArgs args = new WindowClosingEventArgs(true);
        tvm.CloseCommand.Execute(args);

        //If canceled
        if (args.Cancel == true)
        {
          return false;
        }
      }
      return true;
    }
    #endregion
  }
}
