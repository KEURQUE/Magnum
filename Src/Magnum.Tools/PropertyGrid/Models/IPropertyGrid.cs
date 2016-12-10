using Magnum.Controls.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Tools.PropertyGrid.Models
{
  public interface IPropertyGrid
  {
    object SelectedObject { get; set; }
    bool HasASelectedObject { get; set; }
  }
}
