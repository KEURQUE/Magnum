using Magnum.Core.Models;
using Magnum.Core.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Magnum.Core.Services
{
  /// <summary>
  /// This class may seem restricting in terms of constructors, but it is made so we're sure that
  /// a standard is applied in terms of undo/redo goes. This means that every time that we create a
  /// command, we should implement the undo method as much as possible.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class UndoableCommand<T> : IUndoableCommand
  {
    #region Fields
    protected Action<T> _executeMethod;
    protected Action<T> _undoMethod;
    protected Func<T, bool> _canExecuteMethod;
    protected bool _initialized;
    protected bool _canExecute;
    protected bool _addToUndoStack;
    protected bool _addToRedoStack;
    protected bool _isNewCommand = true;
    protected ToolModel _model;
    #endregion

    #region Constructors

    protected UndoableCommand(ToolModel model)
      : this(null, null, null, model, false, false)
    { }

    protected UndoableCommand(ToolModel model, bool addToUndoStack, bool addToRedoStack)
      : this(null, null, null, model, addToUndoStack, addToRedoStack)
    { }

    /// <summary>
    /// Default constructor for UndoableCommand. You have to supply a method that will be executed at all times,
    /// though the CanExecute method is optional. Same goes for the undoable method.
    /// </summary>
    /// <param name="executeMethod">Method to be executed (Must be supplied)</param>
    /// <param name="canExecuteMethod">Method that will verify if the supplied execute method can be launched [OPTIONAL]</param>
    /// <param name="undoMethod">Method that will be executed when the Undo command has been launched [OPTIONAL]</param>
    /// <param name="model">The tool that is associated to the command [OPTIONAL]</param>
    public UndoableCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod = null, Action<T> undoMethod = null, ToolModel model = null, bool addToUndoStack = false, bool addToRedoStack = false)
      : base()
    {
      this._executeMethod = executeMethod;
      this._canExecuteMethod = canExecuteMethod;
      this._undoMethod = undoMethod;
      this._addToUndoStack = _undoMethod != null || addToUndoStack;
      this._addToRedoStack = _undoMethod != null || addToRedoStack;
      this._model = model;
      this._isNewCommand = true;
    }

    #endregion

    #region Properties

    public ICommandManager Manager { get; set; }

    public bool IsNewCommand
    {
      get
      {
        return _isNewCommand;
      }
    }

    #endregion

    #region Methods
    public virtual bool Initialize()
    {
      return true;
    }

    public virtual void Undo(object parameter)
    {
      if (_undoMethod != null)
      {
        _undoMethod.Invoke((T)parameter);
        _isNewCommand = false;
      }

      if (_addToRedoStack && _executeMethod != null && Manager != null)
      {
        Manager.AddToRedoStack(this);
      }
    }

    public void Redo(object parameter)
    {
      if (_canExecute && _executeMethod != null)
      {
        _executeMethod.Invoke((T)parameter);
      }

      if (_addToUndoStack && _canExecute && _undoMethod != null && Manager != null)
      {
        Manager.AddToUndoStack(this);
      }
    }

    public virtual bool CanExecute(object parameter)
    {
      bool result = true;
      if (_canExecuteMethod != null)
      {
        result = _canExecuteMethod.Invoke((T)parameter);
      }
      return result;
    }

    public virtual void Execute(object parameter)
    {
      if (!_initialized)
      {
        if (Initialize())
        {
          if (_executeMethod != null)
          {
            _executeMethod.Invoke((T)parameter);
          }
          _canExecute = true;
        }
        else
        {
          _canExecute = false;
        }
        _initialized = true;
      }
      else if (_initialized && _canExecute && _executeMethod != null)
      {
        _executeMethod.Invoke((T)parameter);
        _isNewCommand = true;
      }

      if (_addToUndoStack && _canExecute && _undoMethod != null && Manager != null)
      {
        Manager.AddToUndoStack(this);
      }
    }

    public void RaiseCanExecuteChanged()
    {
      CommandManager.InvalidateRequerySuggested();
    }
    #endregion

    #region Events
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }
    #endregion

    #region IDisposable

    public virtual void Dispose()
    {
      this._isNewCommand = true;
    }

    #endregion
  }
}
