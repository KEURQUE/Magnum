using Magnum.Controls.Services;
using Magnum.Core.Collections;
using Magnum.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Magnum.Core
{
  /// <summary>
  /// Class CommandableBase
  /// </summary>
  public class CommandableBase : AbstractPrioritizedTree<CommandableBase>, ICommandable
  {
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandableBase"/> class.
    /// </summary>
    protected CommandableBase()
      : base()
    {
    }
    #endregion

    #region ICommandable Members
    /// <summary>
    /// Gets the command.
    /// </summary>
    /// <value>The command.</value>
    public virtual IUndoableCommand Command { get; internal set; }

    /// <summary>
    /// Gets or sets the command parameter.
    /// </summary>
    /// <value>The command parameter.</value>
    public virtual object CommandParameter { get; set; }

    /// <summary>
    /// Gets the input gesture text.
    /// </summary>
    /// <value>The input gesture text.</value>
    public string InputGestureText { get; internal set; }
    #endregion
  }
}
