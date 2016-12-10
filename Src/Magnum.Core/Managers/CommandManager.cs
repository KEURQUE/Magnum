using Magnum.Core.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Magnum.Core.Managers
{
  /// <summary>
  /// The command manager class where you can register and query for a command
  /// </summary>
  public class CommandManager : ICommandManager
  {
    #region Fields
    /// <summary>
    /// The dictionary which holds all the commands
    /// </summary>
    private readonly Dictionary<string, IUndoableCommand> _commands;

    IUnityContainer _container;

    int _undoIndex = 0;
    int _redoIndex = 0;
    #endregion

    #region Constructors
    /// <summary>
    /// Command manager constructor
    /// </summary>
    public CommandManager(IUnityContainer container)
    {
      _container = container;
      _commands = new Dictionary<string, IUndoableCommand>();
      UndoStack = new Stack<IUndoableCommand>();
      RedoStack = new Stack<IUndoableCommand>();

      UndoCommand = new UndoableCommand<object>(Undo, CanUndo);
      RedoCommand = new UndoableCommand<object>(Redo, CanRedo);
      RegisterCommand("UNDO", UndoCommand);
      RegisterCommand("REDO", RedoCommand);
    }
    #endregion

    #region Properties
    public Stack<IUndoableCommand> UndoStack;
    public Stack<IUndoableCommand> RedoStack;

    public int UndoIndex
    {
      get
      {
        return _undoIndex;
      }
      set
      {
        if (_undoIndex != value)
        {
          _undoIndex = value;
          if (_undoIndex < 0)
            _undoIndex = 0;
        }
      }
    }

    public int RedoIndex
    {
      get
      {
        return _redoIndex;
      }
      set
      {
        if (_redoIndex != value)
        {
          _redoIndex = value;
          if (_redoIndex < 0)
            _redoIndex = 0;
        }
      }
    }

    /// <summary>
    /// Gets or sets the undo command.
    /// </summary>
    /// <value>The undo command.</value>
    public virtual IUndoableCommand UndoCommand { get; set; }

    /// <summary>
    /// Gets or sets the redo command.
    /// </summary>
    /// <value>The redo command.</value>
    public virtual IUndoableCommand RedoCommand { get; set; }
    #endregion

    #region ICommandManager Members
    /// <summary>
    /// Register a command with the command manager
    /// </summary>
    /// <param name="name">The name of the command</param>
    /// <param name="command">The command to register</param>
    /// <returns>true if a command is registered, false otherwise</returns>
    public bool RegisterCommand(string name, IUndoableCommand command)
    {
      if (_commands.ContainsKey(name))
        throw new Exception("Commmand " + name + " already exists !");

      // We inject the manager into the command
      command.Manager = this;

      _commands.Add(name, command);
      return true;
    }
    /// <summary>
    /// Gets the command if registered with the command manager
    /// </summary>
    /// <param name="name">The name of the command</param>
    /// <returns>The command if available, null otherwise</returns>
    public IUndoableCommand GetCommand(string name)
    {
      if (_commands.ContainsKey(name))
        return _commands[name];
      return null;
    }

    /// <summary>
    /// Calls RaiseCanExecuteChanged on all Delegate commands available in the command manager
    /// </summary>
    public void Refresh()
    {
      foreach (var keyValuePair in _commands)
      {
        if (keyValuePair.Value is IUndoableCommand)
        {
          var c = keyValuePair.Value as IUndoableCommand;
          c.RaiseCanExecuteChanged();
        }
      }
    }

    public void AddToUndoStack(IUndoableCommand command)
    {
      UndoStack.Push(command);

      // We restart the redo stack if we are entering a new set
      if (RedoStack.Count > 0 && RedoIndex > 0 && command.IsNewCommand)
      {
        RedoStack.Clear();
        RedoIndex = 0;
      }

      UndoIndex++;
      Refresh();
    }

    public void AddToRedoStack(IUndoableCommand command)
    {
      RedoStack.Push(command);
      RedoIndex++;
      Refresh();
    }

    /// <summary>
    /// Determines whether this instance can undo a previous command.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><c>true</c> if this instance can undo a previous command; otherwise, <c>false</c>.</returns>
    protected virtual bool CanUndo(object obj)
    {
      return UndoStack.Count > 0;
    }

    /// <summary>
    /// Undo a previous command.
    /// </summary>
    /// <param name="obj">The object.</param>
    public virtual void Undo(object obj)
    {
      if (UndoStack.Count > 0)
      {
        IUndoableCommand boppedCommand = UndoStack.Pop();

        UndoIndex--;
        boppedCommand.Undo(obj);
      }
    }

    /// <summary>
    /// Determines whether this instance can redo a previous command.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><c>true</c> if this instance can redo a previous command; otherwise, <c>false</c>.</returns>
    protected virtual bool CanRedo(object obj)
    {
      return RedoStack.Count > 0;
    }

    /// <summary>
    /// Redo a previous command.
    /// </summary>
    /// <param name="obj">The object.</param>
    public virtual void Redo(object obj)
    {
      if (RedoStack.Count > 0)
      {
        IUndoableCommand boppedCommand = RedoStack.Pop();
        RedoIndex--;
        boppedCommand.Redo(obj);
      }
    }
    #endregion
  }
}
