using Magnum.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Controls.Themes
{
  /// <summary>
  /// Class DarkTheme
  /// </summary>
  public sealed class LightTheme : ITheme
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LightTheme"/> class.
    /// </summary>
    public LightTheme()
    {
      UriList = new List<Uri>
                          {
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml"),
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml"),
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml"),
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml"),
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml"),
                              new Uri("pack://application:,,,/Magnum.Controls;component/Themes/Styles/ControlsGeneric.xaml"),
                              new Uri("pack://application:,,,/Magnum.Controls;component/Themes/Styles/RibbonGeneric.xaml"),
                              new Uri("pack://application:,,,/Magnum.Controls;component/Themes/Styles/VS2012/LightTheme.xaml"),
                              new Uri("pack://application:,,,/AvalonDock.Themes.VS2012;component/LightTheme.xaml"),
                              new Uri("pack://application:,,,/Magnum.Controls;component/Themes/Styles/AvalonDockBrushes.xaml"),
                              new Uri("pack://application:,,,/Magnum.Controls;component/Themes/Styles/DevComponentsBrushes.xaml")
                          };
    }

    #region ITheme Members
    /// <summary>
    /// Lists of valid URIs which will be loaded in the theme dictionary
    /// </summary>
    /// <value>The URI list.</value>
    public IList<Uri> UriList { get; internal set; }

    /// <summary>
    /// The name of the theme - "Light"
    /// </summary>
    /// <value>The name.</value>
    public string Name
    {
      get { return "Light"; }
    }
    #endregion
  }
}
