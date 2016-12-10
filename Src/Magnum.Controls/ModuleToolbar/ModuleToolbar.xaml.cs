using Magnum.Core.Models;
using Magnum.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Magnum.Controls.ModuleToolbar
{
  /// <summary>
  /// Interaction logic for ModuleToolbar.xaml
  /// </summary>
  public partial class ModuleToolbar : UserControl
  {
    public ModuleToolbar()
    {
      InitializeComponent();
    }

    private ToolModel Model
    {
      get { return DataContext as ToolModel; }
    }

    public virtual void _SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      OnPreviewKeyDown();
    }

    private void OnPreviewKeyDown()
    {
    }

    public virtual void _SearchBox_Search(object sender, RoutedEventArgs e)
    {
      if (Model != null)
        Model.FilterText = _SearchBox.Text;
      OnSearch();
    }

    private void OnSearch()
    {
    }
  }
}
