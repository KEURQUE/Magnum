using Magnum.Core;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Models
{
  public abstract class DocumentModel : ViewModelBase, IModel
  {
    #region Fields
    protected bool _IsDirty;
    #endregion

    #region Constructors
    #endregion

    #region Properties
    #endregion

    #region Methods
    #endregion

    #region Events
    #endregion

    #region IModel Members
    /// <summary>
    /// The document location - could be a file location/server object etc.
    /// </summary>
    public virtual object Location { get; set; }

    /// <summary>
    /// Is the document dirty - does it need to be saved?
    /// </summary>
    public virtual bool IsDirty
    {
      get { return _IsDirty; }
      set
      {
        _IsDirty = value;
        RaisePropertyChanged("IsDirty");
      }
    }
    #endregion
  }
}
