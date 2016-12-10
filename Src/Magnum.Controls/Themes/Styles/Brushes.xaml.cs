using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Magnum.Controls.Themes.Styles
{
  /// <summary>
  /// Interaction logic for Brushes.xaml
  /// </summary>
  public partial class Brushes
  {
    public Brushes()
    {
    }

    /// <summary>
    /// Load a resource WPF-BitmapImage (png only) from embedded resource key defined as 'Resource' not as 'Embedded resource'.
    /// </summary>
    /// <param name="resourceKey">Resource key</param>
    /// <returns></returns>
    public static SolidColorBrush LoadBrushFromResourceKey(string resourceKey)
    {
      if (Application.Current.MainWindow != null && Application.Current.MainWindow.TryFindResource(resourceKey) != null)
        return Application.Current.MainWindow.FindResource(resourceKey) as SolidColorBrush;
      else
        return new SolidColorBrush();
    }

    public static System.Drawing.Color ConvertBrushToDrawingColor(SolidColorBrush br)
    {
      return System.Drawing.Color.FromArgb(br.Color.A, br.Color.R, br.Color.G, br.Color.B);
    }
  }
}
