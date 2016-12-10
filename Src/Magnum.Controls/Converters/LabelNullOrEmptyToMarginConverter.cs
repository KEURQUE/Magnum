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
  public class LabelNullOrEmptyToMarginConverter : MarkupExtension, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var s = value as string;
      return string.IsNullOrEmpty(s) ? new Thickness(9, 0, 0, 0) : new Thickness(1, 0, 1, 0);
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
