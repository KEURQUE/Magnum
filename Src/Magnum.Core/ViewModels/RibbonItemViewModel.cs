using Magnum.Core.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Magnum.Core.ViewModels
{
  /// <summary>
  /// Class RibbonItemViewModel
  /// </summary>
  public sealed class RibbonItemViewModel : MenuItemBase
  {
    #region CTOR
    /// <summary>
    /// Initializes a new instance of the <see cref="RibbonItemViewModel"/> class.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="icon">The icon.</param>
    /// <param name="command">The command.</param>
    /// <param name="isCheckable">if set to <c>true</c> does nothing in the case of ribbon - default value is false.</param>
    /// <param name="container">The container.</param>
    /// <exception cref="System.ArgumentException">Header cannot be SEP for a Ribbon</exception>
    public RibbonItemViewModel(string header, int priority, string keyTip = "", bool isContextualTabGroup = false, ImageSource icon = null, IUndoableCommand command = null, string commandKey = "",
                            bool isCheckable = false, IUnityContainer container = null)
      : base(header, priority, icon, command, commandKey, null, isCheckable, "", "")
    {
      Priority = priority;
      IsSeparator = false;
      Header = header;
      IsContextualTabGroup = isContextualTabGroup;
      KeyTip = keyTip;
      Key = header;
      Command = command;
      CommandKey = commandKey;
      IsCheckable = isCheckable;
      if (isCheckable)
        IsChecked = false;
      if (Header == "SEP")
        throw new ArgumentException("Header cannot be SEP for a Ribbon");
    }
    #endregion

    public string KeyTip { get; set; }

    public bool IsContextualTabGroup { get; set; }
  }
}
