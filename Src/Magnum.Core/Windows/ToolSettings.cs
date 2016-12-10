using Magnum.Core.Events;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Windows
{
  public class ToolSettings : SettingsBase
  {
    public ToolSettings(IEventAggregator eventAggregator)
    {
      eventAggregator.GetEvent<ToolSettingsChangeEvent>().Subscribe(SaveToolSettings);
    }

    public ToolSettings()
    {
    }

    public virtual void SaveToolSettings(ToolViewModel toolViewModel)
    {
      PreferredHeight = toolViewModel.PreferredHeight;
      PreferredWidth = toolViewModel.PreferredWidth;
      PreferredLocation = toolViewModel.PreferredLocation;
      FocusOnRibbonOnClick = toolViewModel.FocusOnRibbonOnClick;
      KeepRibbonTabActive = toolViewModel.KeepRibbonTabActive;
      Save();
    }

    [UserScopedSetting()]
    [DefaultSettingValue("200")]
    public virtual double PreferredHeight
    {
      get { return (double)this["PreferredHeight"]; }
      set { this["PreferredHeight"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("300")]
    public virtual double PreferredWidth
    {
      get { return (double)this["PreferredWidth"]; }
      set { this["PreferredWidth"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("Bottom")]
    public virtual PaneLocation PreferredLocation
    {
      get { return (PaneLocation)this["PreferredLocation"]; }
      set { this["PreferredLocation"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("true")]
    public virtual bool FocusOnRibbonOnClick
    {
      get { return (bool)this["FocusOnRibbonOnClick"]; }
      set { this["FocusOnRibbonOnClick"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("false")]
    public virtual bool KeepRibbonTabActive
    {
      get { return (bool)this["KeepRibbonTabActive"]; }
      set { this["KeepRibbonTabActive"] = value; }
    }
  }
}
