using Magnum.Core.Events;
using Magnum.Core.Services;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Windows
{
  public class ThemeSettings : SettingsBase
  {
    public ThemeSettings(IEventAggregator eventAggregator)
    {
      eventAggregator.GetEvent<ThemeChangeEvent>().Subscribe(NewSelectedTheme);
    }

    private void NewSelectedTheme(ITheme theme)
    {
      this.SelectedTheme = theme.Name;
      this.Save();
    }

    [UserScopedSetting()]
    [DefaultSettingValue("Dark")]
    public string SelectedTheme
    {
      get { return (string)this["SelectedTheme"]; }
      set { this["SelectedTheme"] = value; }
    }
  }
}
