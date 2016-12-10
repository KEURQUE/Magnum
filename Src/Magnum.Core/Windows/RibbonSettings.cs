using Magnum.Core.Events;
using Magnum.Core.Services;
using Microsoft.Practices.Prism.Events;
using Microsoft.Windows.Controls.Ribbon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Magnum.Core.Windows
{
  public class RibbonSettings : SettingsBase
  {
    public RibbonSettings(IEventAggregator eventAggregator)
    {
      eventAggregator.GetEvent<RibbonClosingEvent>().Subscribe(SaveRibbonSettings);
    }

    private void SaveRibbonSettings(IRibbonService ribbonService)
    {
      var buttons = ribbonService.CreateQuickAccessToolbarButtonCollection();
      RibbonQuickAccessToolBar = XamlWriter.Save(buttons);
      IsMinimized = ribbonService.Ribbon.IsMinimized;
      Save();
    }

    [UserScopedSetting()]
    [DefaultSettingValue("")]
    public string RibbonQuickAccessToolBar
    {
      get { return (string)this["RibbonQuickAccessToolBar"]; }
      set { this["RibbonQuickAccessToolBar"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("false")]
    public bool IsMinimized
    {
      get { return (bool)this["IsMinimized"]; }
      set { this["IsMinimized"] = value; }
    }
  }
}
