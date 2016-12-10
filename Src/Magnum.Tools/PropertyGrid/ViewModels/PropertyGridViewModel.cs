using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using Magnum.IconLibrary;
using Magnum.Tools.PropertyGrid.Models;
using Magnum.Tools.PropertyGrid.Views;
using Microsoft.Practices.Unity;

namespace Magnum.Tools.PropertyGrid.ViewModels
{
  public class PropertyGridViewModel : ToolViewModel, IPropertyGrid
  {
    #region Fields
    private readonly PropertyGridModel _model;
    private readonly PropertyGridView _view;
    #endregion

    #region Constructors
    public PropertyGridViewModel(IUnityContainer container, WorkspaceBase workspace)
      : base(container, workspace)
    {
      PreferredLocation = PaneLocation.Right;
      PreferredWidth = 390;
      Name = "Properties";
      Title = "Properties";
      ContentId = "Properties";
      RibbonHeader = "PROPERTIES";
      Description = "Shows properties of selected objects in the editor";
      IsVisible = false;
      IconSource = BitmapImages.LoadBitmapFromResourceKey("Error_16x16");

      _model = new PropertyGridModel(Name, container, "Standard", Description);
      _model.IsSearchBoxVisible = false;
      _model.IsBreadcrumbVisible = false;
      Model = _model;

      _view = new PropertyGridView();
      _view.DataContext = _model;
      View = _view;
    }
    #endregion

    public object SelectedObject
    {
      get
      {
        return (Model as PropertyGridModel).SelectedObject;
      }
      set
      {
        (Model as PropertyGridModel).SelectedObject = value;
      }
    }

    public string SelectedObjectName
    {
      get
      {
        return (Model as PropertyGridModel).SelectedObjectName;
      }
      set
      {
        (Model as PropertyGridModel).SelectedObjectName = value;
      }
    }

    public bool HasASelectedObject
    {
      get
      {
        return (Model as PropertyGridModel).HasASelectedObject;
      }
      set
      {
        (Model as PropertyGridModel).HasASelectedObject = value;
      }
    }
  }
}
