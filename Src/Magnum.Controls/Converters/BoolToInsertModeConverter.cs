using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Magnum.Controls.Converters
{
  public class BoolToInsertModeConverter : IValueConverter
  {
    #region IValueConverter Members
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      bool? actValue = value as bool?;
      if (actValue == null)
        return null;

      return actValue == false ? "INS" : "OVR";
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
