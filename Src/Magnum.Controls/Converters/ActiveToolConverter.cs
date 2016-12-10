using Magnum.Core.Models;
using Magnum.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Magnum.Controls.Converters
{
  public class ActiveToolConverter : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is ToolViewModel)
        return value;

      return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is ToolViewModel)
        return value;

      return Binding.DoNothing;
    }

    #endregion
  }
}
