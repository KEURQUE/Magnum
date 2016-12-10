using System.Configuration;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Magnum.Core.Events;
using Magnum.Core.Services;

namespace Magnum.Core.Windows
{
  public class StatusBarSettings : SettingsBase
  {
    public StatusBarSettings(IEventAggregator eventAggregator)
    {
      eventAggregator.GetEvent<StatusBarClosingEvent>().Subscribe(SaveStatusBarSettings);
    }

    private void SaveStatusBarSettings(IStatusBarService statusBar)
    {
      ShowStatusBarBackground = statusBar.ShowStatusBarBackground;
      Save();
    }

    [UserScopedSetting()]
    [DefaultSettingValue("true")]
    public bool ShowStatusBarBackground
    {
      get { return (bool)this["ShowStatusBarBackground"]; }
      set { this["ShowStatusBarBackground"] = value; }
    }
  }
}
