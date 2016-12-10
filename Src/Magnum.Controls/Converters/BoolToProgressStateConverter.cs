using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Shell;

namespace Magnum.Controls.Converters
{
  public class BoolToProgressStateConverter : IValueConverter
  {
    #region IValueConverter Members
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      bool? actValue = value as bool?;
      if (actValue == null)
        return null;

      return actValue == false ? TaskbarItemProgressState.None : TaskbarItemProgressState.Normal;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
