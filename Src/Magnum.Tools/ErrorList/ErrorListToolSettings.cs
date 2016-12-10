using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Magnum.Tools.ErrorList.Models;
using Magnum.Tools.ErrorList.ViewModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Tools.ErrorList
{
  public class ErrorListToolSettings : ToolSettings
  {
    public ErrorListToolSettings(IEventAggregator eventAggregator)
      : base()
    {
      eventAggregator.GetEvent<ErrorListSettingsChangeEvent>().Subscribe(SaveToolSettings);
    }

    public void SaveToolSettings(ErrorListViewModel errorListViewModel)
    {
      ShowErrors = (errorListViewModel.Model as ErrorListModel).ShowErrors;
      ShowWarnings = (errorListViewModel.Model as ErrorListModel).ShowWarnings;
      ShowMessages = (errorListViewModel.Model as ErrorListModel).ShowMessages;
      base.SaveToolSettings(errorListViewModel);
    }

    [UserScopedSetting()]
    [DefaultSettingValue("true")]
    public bool ShowErrors
    {
      get { return (bool)this["ShowErrors"]; }
      set { this["ShowErrors"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("true")]
    public bool ShowWarnings
    {
      get { return (bool)this["ShowWarnings"]; }
      set { this["ShowWarnings"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("true")]
    public bool ShowMessages
    {
      get { return (bool)this["ShowMessages"]; }
      set { this["ShowMessages"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("390")]
    public override double PreferredWidth
    {
      get { return (double)this["PreferredWidth"]; }
      set { this["PreferredWidth"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("Bottom")]
    public override PaneLocation PreferredLocation
    {
      get { return (PaneLocation)this["PreferredLocation"]; }
      set { this["PreferredLocation"] = value; }
    }
  }
}
