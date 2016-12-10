using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Magnum.Tools.ResourceBrowser.Events;
using Magnum.Tools.ResourceBrowser.Models;
using Magnum.Tools.ResourceBrowser.ViewModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Tools.ResourceBrowser
{
  public class ResourceBrowserToolSettings : ToolSettings
  {
    public ResourceBrowserToolSettings(IEventAggregator eventAggregator)
      : base()
    {
      eventAggregator.GetEvent<ResourceBrowserSettingsChangeEvent>().Subscribe(SaveToolSettings);
    }

    public void SaveToolSettings(ResourceBrowserViewModel resourceBrowserViewModel)
    {
      DataPaneWidth = (resourceBrowserViewModel.Model as ResourceBrowserModel).DataPaneWidth;
      base.SaveToolSettings(resourceBrowserViewModel);
    }

    [UserScopedSetting()]
    [DefaultSettingValue("350")]
    public double DataPaneWidth
    {
      get { return (double)this["DataPaneWidth"]; }
      set { this["DataPaneWidth"] = value; }
    }

    [UserScopedSetting()]
    [DefaultSettingValue("Center")]
    public override PaneLocation PreferredLocation
    {
      get { return (PaneLocation)this["PreferredLocation"]; }
      set { this["PreferredLocation"] = value; }
    }
  }
}
