using Magnum.Core;
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
  public class MenuItemViewModel : MenuItemBase
  {
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemViewModel"/> class.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="icon">The icon.</param>
    /// <param name="command">The command.</param>
    /// <param name="gesture">The gesture.</param>
    /// <param name="isCheckable">if set to <c>true</c> this menu acts as a checkable menu.</param>
    /// <param name="container">The container.</param>
    public MenuItemViewModel(string header, int priority, ImageSource icon = null, IUndoableCommand command = null, string commandKey = "",
                             KeyGesture gesture = null, bool isCheckable = false, IUnityContainer container = null, string label = "", string description = "")
      : base(header, priority, icon, command, commandKey, gesture, isCheckable, label, description)
    {
    }

    #endregion

    #region Methods
    /// <summary>
    /// Creates an instance of a menu acting as a separator with a priority
    /// </summary>
    /// <param name="priority">The priority.</param>
    /// <returns>AbstractMenuItem.</returns>
    public static MenuItemBase Separator(int priority)
    {
      return new MenuItemViewModel("SEP", priority);
    }
    #endregion
  }
}
