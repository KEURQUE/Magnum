using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Magnum.Controls.Converters
{
  public class IsCheckedToWhiteBrushConverter : MarkupExtension, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var s = (bool)value;
      return s == true ? Brushes.White : Application.Current.MainWindow.FindResource("TextBrush") as SolidColorBrush;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }
  }
}
