using System.Configuration;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Magnum.Core.Events;

namespace Magnum.Core.Windows
{
  public class WindowPositionSettings : SettingsBase
  {
    public WindowPositionSettings(IEventAggregator eventAggregator)
    {
      eventAggregator.GetEvent<WindowClosingEvent>().Subscribe(SaveWindowPositions);
    }

    private void SaveWindowPositions(Window window)
    {
      Left = window.Left;
      Top = window.Top;
      Height = window.Height;
      Width = window.Width;
      State = window.WindowState;
      Save();
    }

    [UserScopedSetting()]
    [DefaultSettingValue("0")]
    public double Left
    {
      get { return (double)this["Left"]; }
      set { this["Left"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("0")]
    public double Top
    {
      get { return (double)this["Top"]; }
      set { this["Top"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("600")]
    public double Height
    {
      get { return (double)this["Height"]; }
      set { this["Height"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("800")]
    public double Width
    {
      get { return (double)this["Width"]; }
      set { this["Width"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("Maximized")]
    public WindowState State
    {
      get { return (WindowState)this["State"]; }
      set { this["State"] = value; }
    }
  }
}
