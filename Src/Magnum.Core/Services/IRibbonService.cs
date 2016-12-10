using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Microsoft.Windows.Controls.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Magnum.Core.Services
{
  [Serializable]
  public class QuickAccessToolbarButtonCollection : List<QuickAccessToolbarButton> { }

  [Serializable]
  public class QuickAccessToolbarButton
  {
    public ImageSource LargeImageSource { get; set; }
    public ImageSource SmallImageSource { get; set; }
    public string Label { get; set; }
    public object ToolTip { get; set; }
    public string ToolTipDescription { get; set; }
    public string KeyTip { get; set; }
    public string CommandKey { get; set; }
  }

  /// <summary>
  /// Interface IRibbonService - the application's ribbon is returned by this service
  /// </summary>
  public interface IRibbonService
  {
    /// <summary>
    /// Gets the ribbon of the application.
    /// </summary>
    /// <value>The ribbon.</value>
    Ribbon Ribbon { get; }

    /// <summary>
    /// Adds the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns><c>true</c> if successfully added, <c>false</c> otherwise</returns>
    bool Add(RibbonItemViewModel item);

    /// <summary>
    /// Removes the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if successfully removed, <c>false</c> otherwise</returns>
    bool Remove(string key);

    /// <summary>
    /// Gets the specified ribbon tab using the key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>RibbonItemViewModel.</returns>
    RibbonItemViewModel Get(string key);

    QuickAccessToolbarButtonCollection CreateQuickAccessToolbarButtonCollection();
  }
}
