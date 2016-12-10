using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Magnum.Core.Services
{
  public interface ISearchable
  {
    string AssignedTool { get; set; }

    string DisplayName { get; set; }

    string Category { get; set; }

    string Description { get; set; }

    ImageSource IconSource { get; set; }

    IUndoableCommand OpenCommand { get; set; }

    void Open();
  }
}
