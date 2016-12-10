using Magnum.Core.ViewModels;
using Magnum.IconLibrary;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Magnum.Tools.ErrorList.Models
{
  public class ErrorListItem : ViewModelBase
  {
    #region Fields
    private ErrorListItemType _itemType;
    private int _number;
    private string _description;
    private string _path;
    private int? _line;
    private int? _column;
    #endregion

    #region Constructors
    public ErrorListItem(ErrorListItemType itemType, int number, string description,
        string path = null, int? line = null, int? column = null)
    {
      _itemType = itemType;
      _number = number;
      _description = description;
      _path = path;
      _line = line;
      _column = column;
    }

    public ErrorListItem()
    {

    }
    #endregion

    #region Properties
    public ErrorListItemType ItemType
    {
      get { return _itemType; }
      set
      {
        _itemType = value;
        RaisePropertyChanged("ItemType");
      }
    }

    public int Number
    {
      get { return _number; }
      set
      {
        _number = value;
        RaisePropertyChanged("Number");
      }
    }
    public string Description
    {
      get { return _description; }
      set
      {
        _description = value;
        RaisePropertyChanged("Description");
      }
    }
    public string Path
    {
      get { return _path; }
      set
      {
        _path = value;
        RaisePropertyChanged("Path");
        RaisePropertyChanged("File");
      }
    }

    public string File
    {
      get { return System.IO.Path.GetFileName(Path); }
    }

    public int? Line
    {
      get { return _line; }
      set
      {
        _line = value;
        RaisePropertyChanged("Line");
      }
    }

    public int? Column
    {
      get { return _column; }
      set
      {
        _column = value;
        RaisePropertyChanged("Column");
      }
    }

    public BitmapSource Icon
    {
      get
      {
        return IconType.Error.ToString() == ItemType.ToString()? BitmapImages.LoadBitmapFromIconType(IconType.Error) :
          IconType.Message.ToString() == ItemType.ToString()? BitmapImages.LoadBitmapFromIconType(IconType.Message) :
          BitmapImages.LoadBitmapFromIconType(IconType.Warning);
      }
    }

    public System.Action OnClick { get; set; }
    #endregion
  }
}