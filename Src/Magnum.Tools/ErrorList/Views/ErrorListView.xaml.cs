using Magnum.Core.Models;
using Magnum.Core.Views;
using Magnum.Tools.ErrorList.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace Magnum.Tools.ErrorList.Views
{
  /// <summary>
  /// Interaction logic for ErrorListView.xaml
  /// </summary>
  public partial class ErrorListView : UserControl, IDocumentView
  {
    public ErrorListView()
    {
      InitializeComponent();
    }
  }
}