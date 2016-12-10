using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Magnum.Core.Services
{
  /// <summary>
  /// Interface ICommandManager
  /// </summary>
  public interface ICommandManager
  {
    /// <summary>
    /// Registers the command.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="command">The command.</param>
    /// <returns><c>true</c> if successfully added the command, <c>false</c> otherwise</returns>
    bool RegisterCommand(string name, IUndoableCommand command);

    /// <summary>
    /// Gets the command.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>IUndoableCommand.</returns>
    IUndoableCommand GetCommand(string name);

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    void Refresh();

    /// <summary>
    /// Undo command.
    /// </summary>
    IUndoableCommand UndoCommand { get; set; }

    /// <summary>
    /// Redo command.
    /// </summary>
    IUndoableCommand RedoCommand { get; set; }

    void AddToUndoStack(IUndoableCommand command);
    void AddToRedoStack(IUndoableCommand command);
  }
}
