using Magnum.Core.Services;
using Magnum.Core.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.ApplicationModel
{
  public sealed class Application
  {
    #region Fields
    private static volatile Application instance;
    private static object syncRoot = new Object();
    //private static List<String> _commandLineArgs = null;
    //private static IList<AbstractSelectable> _selection = null;
    private static Xceed.Wpf.AvalonDock.DockingManager _DockingManager;
    private static IStatusBarService _StatusBar;
    private static IWorkspace _Workspace;
    private static IShell _MainWindow;
    private static bool _isGameLoaded;
    private static bool _isInitialized;
    #endregion

    #region Constructors
    private Application()
    {
      _isGameLoaded = false;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Returns the Application abstraction
    /// </summary>
    public static Application Instance
    {
      get
      {
        if (instance == null)
        {
          lock (syncRoot)
          {
            if (instance == null)
              instance = new Application();
          }
        }

        return instance;
      }
    }

    /// <summary>
    /// Returns the DockingManager abstraction
    /// </summary>
    public static Xceed.Wpf.AvalonDock.DockingManager DockingManager
    {
      get { return _DockingManager; }
      set
      {
        if (_DockingManager != value)
          _DockingManager = value;
      }
    }

    /// <summary>
    /// Returns the StatusBar abstraction
    /// </summary>
    public static IStatusBarService StatusBar
    {
      get { return _StatusBar; }
      set
      {
        if (_StatusBar != value)
          _StatusBar = value;
      }
    }

    /// <summary>
    /// Returns the IWorkspace abstraction
    /// </summary>
    public static IWorkspace Workspace
    {
      get { return _Workspace; }
      set
      {
        if (_Workspace != value)
          _Workspace = value;
      }
    }

    /// <summary>
    /// Returns the MainWindow abstraction
    /// </summary>
    public static IShell MainWindow
    {
      get { return _MainWindow; }
      set
      {
        if (_MainWindow != value)
          _MainWindow = value;
      }
    }

    public static event EventHandler OnIsGameLoadedChanged;
    /// <summary>
    /// Indicates if a game is loaded or not
    /// </summary>
    public static bool IsGameLoaded
    {
      get { return _isGameLoaded; }
      set
      {
        if (_isGameLoaded != value)
        {
          _isGameLoaded = value;

          var handler = OnIsGameLoadedChanged;
          if (handler != null)
            handler(null, null);
        }
      }
    }

    public static event EventHandler OnIsInitializedChanged;
    /// <summary>
    /// Indicates if the application is initialized or not
    /// </summary>
    public static bool IsInitialized
    {
      get { return _isInitialized; }
      set
      {
        if (_isInitialized != value)
        {
          _isInitialized = value;

          var handler = OnIsInitializedChanged;
          if (handler != null)
            handler(null, null);
        }
      }
    }
    #endregion

    #region Methods
    #endregion

    #region Events
    #endregion
  }
}
