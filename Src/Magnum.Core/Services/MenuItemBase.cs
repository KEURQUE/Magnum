using Magnum.Core.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Magnum.Core.Services
{
  public abstract class MenuItemBase : CommandableBase, IMenuService
  {
    #region Static

    /// <summary>
    /// The static separator count
    /// </summary>
    protected static int sepCount = 1;

    #endregion

    #region Members
    /// <summary>
    /// The injected container
    /// </summary>
    protected IUnityContainer _container;

    /// <summary>
    /// Is the menu checked
    /// </summary>
    protected bool _isChecked;
    #endregion

    #region Methods and Properties
    /// <summary>
    /// Gets a value indicating whether this instance is separator.
    /// </summary>
    /// <value><c>true</c> if this instance is separator; otherwise, <c>false</c>.</value>
    public virtual bool IsSeparator { get; internal set; }

    /// <summary>
    /// Gets the icon of the menu.
    /// </summary>
    /// <value>The icon.</value>
    public virtual ImageSource Icon { get; internal set; }

    /// <summary>
    /// Gets the tool tip.
    /// </summary>
    /// <value>The tool tip.</value>
    public virtual string ToolTip
    {
      get
      {
        string value = Header.Replace("_", "");
        if (!string.IsNullOrEmpty(InputGestureText))
        {
          value += " " + InputGestureText;
        }
        return value;
      }
    }

    /// <summary>
    /// Gets the header of the menu.
    /// </summary>
    /// <value>The header.</value>
    public virtual string Header { get; internal set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is checkable.
    /// </summary>
    /// <value><c>true</c> if this instance is checkable; otherwise, <c>false</c>.</value>
    public virtual bool IsCheckable { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance is visible.
    /// </summary>
    /// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
    public virtual bool IsVisible { get; internal set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is checked.
    /// </summary>
    /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
    public virtual bool IsChecked
    {
      get { return _isChecked; }
      set
      {
        _isChecked = value;
        RaisePropertyChanged("IsChecked");
      }
    }
    #endregion

    #region Overrides
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      return Header;
    }

    /// <summary>
    /// Adds the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns><c>true</c> if successfully added, <c>false</c> otherwise</returns>
    /// <exception cref="System.ArgumentException">Expected a AbstractMenuItem as the argument. Only Menu's can be added within a Menu.</exception>
    public override string Add(CommandableBase item)
    {
      if (item.GetType().IsAssignableFrom(typeof(MenuItemBase)))
      {
        throw new ArgumentException("Expected a AbstractMenuItem as the argument. Only Menu's can be added within a Menu.");
      }
      return base.Add(item);
    }

    public void Refresh()
    {
      this.RaisePropertyChanged("Header");
      this.RaisePropertyChanged("Children");
      this.RaisePropertyChanged("Icon");
      this.RaisePropertyChanged("ToolTip");
      this.RaisePropertyChanged("IsVisible");
    }

    #endregion

    #region CTOR
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemBase"/> class.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="icon">The icon.</param>
    /// <param name="command">The command.</param>
    /// <param name="gesture">The gesture.</param>
    /// <param name="isCheckable">if set to <c>true</c> acts as a checkable menu.</param>
    protected MenuItemBase(string header, int priority, ImageSource icon = null, IUndoableCommand command = null, string commandKey = "",
                            KeyGesture gesture = null, bool isCheckable = false, string label = "", string description = "")
    {
      Priority = priority;
      IsSeparator = false;
      Header = header;
      Key = header;
      Command = command;
      CommandKey = commandKey;
      IsCheckable = isCheckable;
      Icon = icon;
      Label = label;
      Description = description;
      if (gesture != null && command != null)
      {
        Application.Current.MainWindow.InputBindings.Add(new KeyBinding(command, gesture));
        InputGestureText = gesture.DisplayString;
      }
      if (isCheckable)
      {
        IsChecked = false;
      }
      if (Header == "SEP")
      {
        Key = "SEP" + sepCount.ToString();
        Header = "";
        sepCount++;
        IsSeparator = true;
      }
    }

    #endregion

    public string Label { get; set; }

    public string Description { get; set; }

    public string CommandKey { get; set; }
  }
}
