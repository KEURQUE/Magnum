using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Magnum.Core.ViewModels
{
  /// <summary>
  /// Class RibbonItemTemplateSelector
  /// </summary>
  public sealed class RibbonItemTemplateSelector : DataTemplateSelector
  {
    /// <summary>
    /// Gets or sets the ribbon button template.
    /// </summary>
    /// <value>The ribbon button template.</value>
    public DataTemplate ButtonTemplate { get; set; }

    /// <summary>
    /// Gets or sets the ribbon combo box template.
    /// </summary>
    /// <value>The ribbon combo box template.</value>
    public DataTemplate ComboBoxTemplate { get; set; }

    /// <summary>
    /// Gets or sets the ribbon separator template.
    /// </summary>
    /// <value>The ribbon separator template.</value>
    public DataTemplate SeparatorTemplate { get; set; }

    /// <summary>
    /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate" /> based on custom logic.
    /// Here, it looks at the item definition and determines if the toolbar item needs to be a button or combo box or a separator.
    /// </summary>
    /// <param name="item">The data object for which to select the template.</param>
    /// <param name="container">The data-bound object.</param>
    /// <returns>Returns a <see cref="T:System.Windows.DataTemplate" /> or null. The default value is null.</returns>
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      var menuItem = item as MenuItemViewModel;
      if (menuItem != null && !menuItem.IsSeparator)
      {
        if (menuItem.Children.Count > 0)
          return ComboBoxTemplate;

        return ButtonTemplate;
      }
      return SeparatorTemplate;
    }
  }
}
