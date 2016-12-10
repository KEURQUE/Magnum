using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Magnum.Core.Services
{
  public enum EngineStates
  {
    NotLoaded = 0,
    Stopped = 1,
    Playing = 2,
    Paused = 3
  }

  public interface IStatusBarService
  {
    bool Animation(Image image);
    bool Clear();
    void UpdateEngineInfo();
    bool FreezeOutput();
    bool IsFrozen { get; }
    bool ShowStatusBarBackground { get; set; }
    string Text { get; set; }
    string MousePosition { get; set; }
    string CurrentLayerName { get; set; }
    string SelectedTheme { get; set; }
    EngineStates EngineState { get; set; }
    Brush Foreground { get; set; }
    Brush Background { get; set; }
    Brush BorderColor { get; set; }
    bool? InsertMode { get; set; }
    int? LineNumber { get; set; }
    int? CharPosition { get; set; }
    int? ColPosition { get; set; }
    BitmapImage EngineImage { get; set; }
  }
}
