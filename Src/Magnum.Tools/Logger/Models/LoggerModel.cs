using Magnum.Core.Models;
using Magnum.Core.Services;
using Microsoft.Practices.Unity;
using System.ComponentModel;

namespace Magnum.Tools.Logger.Models
{
  internal class LoggerModel : ToolModel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ToolModel"/> class.
    /// </summary>
    public LoggerModel(string displayName, IUnityContainer container, string category, string description, bool isPinned = true, string shortcut = null)
      : base(displayName, container, category, description, isPinned, shortcut)
    {
    }

    private string _text;

    public string Text
    {
      get { return _text; }
    }

    public void AddLog(ILoggerService logger)
    {
      _text += logger.Message + "\n";
      RaisePropertyChanged("Text");
    }
  }
}