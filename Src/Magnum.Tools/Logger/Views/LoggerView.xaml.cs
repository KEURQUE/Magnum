using Magnum.Core.Models;
using Magnum.Core.Views;
using Magnum.Tools.Logger.Models;
using Magnum.Tools.Logger.ViewModels;
using System;
using System.IO;
using System.Windows.Controls;

namespace Magnum.Tools.Logger.Views
{
  /// <summary>
  /// Interaction logic for LoggerView.xaml
  /// </summary>
  public partial class LoggerView : UserControl, IDocumentView
  {
    public LoggerView()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      FileStream fs = null;
      try
      {
        //Opens a text tile.
        fs = new FileStream(@"C:\temp\data.txt", FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        string line;

        //A value is read from the file and output to the console.
        line = sr.ReadLine();
        Console.WriteLine(line);
      }
      catch (FileNotFoundException ex)
      {
        Console.WriteLine("[Data File Missing] {0}", ex);
        throw new FileNotFoundException(@"[data.txt not in c:\temp directory]", ex);
      }
    }
  }
}