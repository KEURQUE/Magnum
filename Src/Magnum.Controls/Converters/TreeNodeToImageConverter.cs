using Magnum.Core.Services;
using Magnum.IconLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Magnum.Controls.Converters
{
  [ValueConversion(typeof(ITreeNodeItem), typeof(ImageSource))]
  public class TreeNodeToImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      var viewModel = value as ITreeNodeItem;
      if (viewModel == null)
        return Binding.DoNothing;

      if (viewModel.Icon != null)
        return viewModel.Icon;
      else
      {
        if (!viewModel.IsLeaf)
        {
          if (viewModel.IsExpanded)
            return BitmapImages.LoadBitmapFromIconType(IconType.FolderOpen);
          else
            return BitmapImages.LoadBitmapFromIconType(IconType.Folder);
        }
        else
        {
          var source = BitmapImages.LoadBitmapFromIconType(IconType.None);
          return source ?? Binding.DoNothing;
        }
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
