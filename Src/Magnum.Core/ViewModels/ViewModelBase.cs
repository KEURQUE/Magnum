using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.ViewModels
{
  /// <summary>
  /// The view model base class
  /// </summary>
  public class ViewModelBase : INotifyPropertyChanged
  {
    #region INotifyPropertyChanged Members
    /// <summary>
    /// Event handler that gets triggered when a property changes
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Should be called when a property value has changed
    /// </summary>
    /// <param name="propertyName">The property name</param>
    protected virtual void RaisePropertyChanged(string propertyName)
    {
      var handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
  }
}
