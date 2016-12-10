using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Magnum.IconLibrary
{
  /// <summary>
  /// Interaction logic for BitmapImages.xaml
  /// </summary>
  public partial class BitmapImages
  {
    public BitmapImages()
    {
    }

    /// <summary>
    /// Load a resource WPF-BitmapImage (png, bmp, ...) from embedded resource defined as 'Resource' not as 'Embedded resource'.
    /// </summary>
    /// <param name="pathInApplication">Path without starting slash</param>
    /// <param name="assembly">Usually 'Assembly.GetExecutingAssembly()'. If not mentionned, I will use the calling assembly</param>
    /// <returns></returns>
    public static BitmapImage LoadBitmapFromResource(string pathInApplication, Assembly assembly = null)
    {
      if (assembly == null)
      {
        assembly = Assembly.GetCallingAssembly();
      }

      if (pathInApplication[0] == '/')
      {
        pathInApplication = pathInApplication.Substring(1);
      }
      return new BitmapImage(new Uri(@"pack://application:,,,/" + assembly.GetName().Name + ";component/" + pathInApplication, UriKind.Absolute));
    }

    /// <summary>
    /// Load a resource WPF-BitmapImage (png only) from embedded resource key defined as 'Resource' not as 'Embedded resource'.
    /// </summary>
    /// <param name="resourceKey">Resource key</param>
    /// <returns></returns>
    public static BitmapImage LoadBitmapFromResourceKey(string resourceKey)
    {
      return new BitmapImage(new Uri(@"pack://application:,,,/Magnum.IconLibrary;component/Resources/" + resourceKey + ".png", UriKind.Absolute));
    }

    /// <summary>
    /// Load a resource WPF-BitmapImage (png only) from iconType defined as 'Resource' not as 'Embedded resource'.
    /// </summary>
    /// <param name="iconType">iconType</param>
    /// <returns></returns>
    public static BitmapImage LoadBitmapFromIconType(IconType iconType)
    {
      string resourceKey = string.Empty;
      switch (iconType)
      {
        case IconType.None:
          resourceKey = "Unavailable_32x32";
          break;
        case IconType.Game:
          resourceKey = "Game_32x32";
          break;
        case IconType.World:
          resourceKey = "World_32x32";
          break;
        case IconType.Map:
          resourceKey = "Map_32x32";
          break;
        case IconType.Room:
          break;
        case IconType.Folder:
          resourceKey = "Folder_32x32";
          break;
        case IconType.FolderOpen:
          resourceKey = "FolderOpen_38x32";
          break;
        case IconType.Character:
          break;
        case IconType.Texture:
          resourceKey = "Texture_32x32";
          break;
        case IconType.Error:
          resourceKey = "ErrorColor_16x16";
          break;
        case IconType.Message:
          resourceKey = "MessageColor_16x16";
          break;
        case IconType.Warning:
          resourceKey = "WarningColor_16x16";
          break;
        case IconType.Open:
          resourceKey = "Open_32x32";
          break;
        case IconType.Play:
          resourceKey = "Play_32x32";
          break;
        case IconType.Stop:
          resourceKey = "Stop_43x32";
          break;
        case IconType.Pause:
          resourceKey = "Pause_32x32";
          break;
        case IconType.Entity:
          resourceKey = "Entity_128x128";
          break;
        case IconType.EntityGroup:
          resourceKey = "EntityGroup_128x128";
          break;
        case IconType.Model:
          resourceKey = "Model_32x32";
          break;
      }
      return new BitmapImage(new Uri(@"pack://application:,,,/Magnum.IconLibrary;component/Resources/" + resourceKey + ".png", UriKind.Absolute));
    }
  }
}
