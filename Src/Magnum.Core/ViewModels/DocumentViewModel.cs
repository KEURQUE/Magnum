using Magnum.Controls.Services;
using Magnum.Core;
using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.Windows;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Magnum.Core.ViewModels
{
  public class DocumentViewModel : ViewModelBase, ITool
  {
    #region Fields
    /// <summary>
    /// The static count value for "Untitled" number.
    /// </summary>
    protected static int Count = 1;
    /// <summary>
    /// The command manager
    /// </summary>
    protected ICommandManager _commandManager;
    /// <summary>
    /// The content id of the document
    /// </summary>
    protected string _contentId = null;
    /// <summary>
    /// Is the document active
    /// </summary>
    protected bool _isActive = false;
    /// <summary>
    /// Is the document selected
    /// </summary>
    protected bool _isSelected = false;
    /// <summary>
    /// The logger instance
    /// </summary>
    protected ILoggerService _logger;
    /// <summary>
    /// The title of the document
    /// </summary>
    protected string _title = null;
    /// <summary>
    /// The tool tip to display on the document
    /// </summary>
    protected string _tooltip = null;
    /// <summary>
    /// The workspace instance
    /// </summary>
    protected IWorkspace _workspace;

    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentViewModel"/> class.
    /// </summary>
    /// <param name="workspace">The injected workspace.</param>
    /// <param name="commandManager">The injected command manager.</param>
    /// <param name="logger">The injected logger.</param>
    protected DocumentViewModel(WorkspaceBase workspace, ICommandManager commandManager, ILoggerService logger)
    {
      _workspace = workspace;
      _commandManager = commandManager;
      _logger = logger;

      CloseCommand = new UndoableCommand<object>(Close, CanClose);
      OpenCommand = new UndoableCommand<object>(Open, CanOpen);
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the close command.
    /// </summary>
    /// <value>The close command.</value>
    public virtual IUndoableCommand CloseCommand { get; protected internal set; }

    /// <summary>
    /// Gets or sets the open command.
    /// </summary>
    /// <value>The open command.</value>
    public virtual IUndoableCommand OpenCommand { get; set; }

    /// <summary>
    /// The content model
    /// </summary>
    /// <value>The model.</value>
    public IModel Model { get; set; }

    /// <summary>
    /// The content view
    /// </summary>
    /// <value>The view.</value>
    public virtual UserControl View { get; set; }

    /// <summary>
    /// The title of the document
    /// </summary>
    /// <value>The title.</value>
    public virtual string Title
    {
      get
      {
        if (Model.IsDirty)
        {
          return _title + "*";
        }
        return _title;
      }
      set
      {
        if (_title != value)
        {
          _title = value;
          RaisePropertyChanged("Title");
        }
      }
    }

    /// <summary>
    /// The tool tip of the document
    /// </summary>
    /// <value>The tool tip.</value>
    public virtual string Tooltip
    {
      get { return _tooltip; }
      protected set
      {
        if (_tooltip != value)
        {
          _tooltip = value;
          RaisePropertyChanged("Tooltip");
        }
      }
    }

    /// <summary>
    /// The image source that can be used as an icon in the tab
    /// </summary>
    /// <value>The icon source.</value>
    public virtual ImageSource IconSource { get; protected internal set; }

    /// <summary>
    /// The content ID - unique value for each document
    /// </summary>
    /// <value>The content id.</value>
    public virtual string ContentId
    {
      get { return _contentId; }
      protected set
      {
        if (_contentId != value)
        {
          _contentId = value;
          RaisePropertyChanged("ContentId");
        }
      }
    }

    /// <summary>
    /// Is the document selected
    /// </summary>
    /// <value><c>true</c> if this document is selected; otherwise, <c>false</c>.</value>
    public virtual bool IsSelected
    {
      get { return _isSelected; }
      set
      {
        if (_isSelected != value)
        {
          _isSelected = value;
          RaisePropertyChanged("IsSelected");
        }
      }
    }

    /// <summary>
    /// Is the document active
    /// </summary>
    /// <value><c>true</c> if this document is active; otherwise, <c>false</c>.</value>
    public virtual bool IsActive
    {
      get { return _isActive; }
      set
      {
        if (_isActive != value)
        {
          _isActive = value;
          RaisePropertyChanged("IsActive");
        }
      }
    }

    /// <summary>
    /// The content handler which does save and load of the file
    /// </summary>
    /// <value>The handler.</value>
    public IContentHandler Handler { get; set; }

    /// <summary>
    /// The property changed event of the Model.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="propertyChangedEventArgs">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    public virtual void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
      RaisePropertyChanged("Title");
    }

    #endregion

    #region Methods
    /// <summary>
    /// Determines whether this instance can close.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><c>true</c> if this instance can close; otherwise, <c>false</c>.</returns>
    protected virtual bool CanClose(object obj)
    {
      return (obj != null) ? _commandManager.GetCommand("CLOSE").CanExecute(obj) : _commandManager.GetCommand("CLOSE").CanExecute(this);
    }

    /// <summary>
    /// Closes this instance.
    /// </summary>
    /// <param name="obj">The object.</param>
    protected virtual void Close(object obj)
    {
      if (obj != null)
      {
        _commandManager.GetCommand("CLOSE").Execute(obj);
      }
      else
      {
        _commandManager.GetCommand("CLOSE").Execute(this);
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
    #endregion

    #region Events
    #endregion
  }
}
