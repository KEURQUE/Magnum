using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Magnum.Controls.Converters
{
  public class IntNullOr0ToVisibilityConverter : MarkupExtension, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int? s = (int)value;
      return s.HasValue && s != 0 ? Visibility.Visible : Visibility.Collapsed;
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
