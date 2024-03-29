﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Magnum.Controls.Converters
{
  [ValueConversion(typeof(string), typeof(Visibility))]
  public class BoolToVisibilityCollapsedConverter : IValueConverter
  {
    public static BoolToVisibilityCollapsedConverter Instance = new BoolToVisibilityCollapsedConverter();

    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool && (bool)value)
        return Visibility.Visible;
      else return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if ((Visibility)value != Visibility.Collapsed)
        return true;
      return false;
    }

    #endregion
  }
}
