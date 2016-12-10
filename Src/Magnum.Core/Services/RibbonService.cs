using Magnum.Core.Collections;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Microsoft.Windows.Controls.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Magnum.Core.Services
{
  /// <summary>
  /// The ribbon service
  /// </summary>
  public sealed class RibbonService : IRibbonService
  {
    /// <summary>
    /// The dictionary of ribbon tabs
    /// </summary>
    private static readonly Dictionary<string, RibbonItemViewModel> RibbonDictionary = new Dictionary<string, RibbonItemViewModel>();

    #region IToolBarService Members
    public QuickAccessToolbarButtonCollection CreateQuickAccessToolbarButtonCollection()
    {
      QuickAccessToolbarButtonCollection buttons = new QuickAccessToolbarButtonCollection();

      foreach (RibbonButton rButton in Ribbon.QuickAccessToolBar.Items)
      {
        QuickAccessToolbarButton qaButton = new QuickAccessToolbarButton()
        {
          Label = rButton.Label,
          KeyTip = rButton.KeyTip,
          LargeImageSource = rButton.LargeImageSource,
          SmallImageSource = rButton.SmallImageSource,
          ToolTip = rButton.ToolTip,
          ToolTipDescription = rButton.ToolTipDescription,
          CommandKey = rButton.ContentStringFormat
        };
        buttons.Add(qaButton);
      }

      return buttons;
    }

    /// <summary>
    /// Adds a new ribbon tab to the application
    /// </summary>
    /// <param name="item">The ribbon tab to add</param>
    /// <returns>true, if successful - false, otherwise</returns>
    public bool Add(RibbonItemViewModel item)
    {
      if (!RibbonDictionary.ContainsKey(item.Key))
      {
        RibbonDictionary.Add(item.Key, item);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Removes a ribbon tab from the application
    /// </summary>
    /// <param name="key">The key of the ribbon tab to remove</param>
    /// <returns>true, if successful - false, otherwise</returns>
    public bool Remove(string key)
    {
      if (RibbonDictionary.ContainsKey(key))
      {
        RibbonDictionary.Remove(key);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Gets the ribbon tab with this key
    /// </summary>
    /// <param name="key">The key of the ribbon tab</param>
    /// <returns>A RibbonItemViewModel or null if not found</returns>
    public RibbonItemViewModel Get(string key)
    {
      if (RibbonDictionary.ContainsKey(key))
      {
        return RibbonDictionary[key];
      }
      return null;
    }

    /// <summary>
    /// The ribbon which will be used in the application
    /// </summary>
    Ribbon _ribbon;
    public Ribbon Ribbon
    {
      get
      {
        if (_ribbon == null)
        {
          var ribbon = new Ribbon();
          ribbon.ShowQuickAccessToolBarOnTop = false;
          IAddChild child = ribbon;
          foreach (var tabs in RibbonDictionary.OrderBy(x => x.Value.Priority))
          {
            var rb = new RibbonTab();
            rb.Header = tabs.Value.Header;
            rb.KeyTip = tabs.Value.KeyTip;

            var t = Application.Current.MainWindow.FindResource("ribbonItemTemplateSelector") as DataTemplateSelector;

            foreach (RibbonItemViewModel group in tabs.Value.Children)
            {
              var rg = new RibbonGroup();
              rg.Header = group.Header;
              rg.ToolTip = group.KeyTip;
              IAddChild tabChild = rb;

              rg.SetValue(ItemsControl.ItemTemplateSelectorProperty, t);
              rg.ItemsSource = group.Children;
              tabChild.AddChild(rg);
            }

            /*if (tabs.Value.IsContextualTabGroup)
            {
              var cg = new RibbonContextualTabGroup();
              cg.Header = tabs.Value;
              cg.Visibility = System.Windows.Visibility.Visible;
              cg.Background = System.Windows.Media.Brushes.Blue;
              rb.ContextualTabGroupHeader = cg.Header;
              ribbon.ContextualTabGroups.Add(cg);
            }*/

            if (tabs.Value.IsContextualTabGroup)
              rb.Visibility = System.Windows.Visibility.Collapsed;

            child.AddChild(rb);
          }
          _ribbon = ribbon;
        }
        return _ribbon;
      }
    }

    #endregion
  }
}
