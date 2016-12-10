using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.IconLibrary;
using Magnum.Tools.SelectionDrivenEditor.ViewModels;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Magnum.Tools.SelectionDrivenEditor.Models
{
  public class ObjectTreeNode : TreeNodeViewModel
  {
    #region Fields

    private IEditorPane _Editor;

    #endregion

    #region Constructors

    public ObjectTreeNode()
    : base(){ }

    public ObjectTreeNode(IEditorPane editor, string title = "", int id = -1, IItemType itemType = IItemType.Unknown, BitmapImage icon = null, IUnityContainer container = null, IEnumerable<ITreeNodeItem> children = null)
     : base(title, id, itemType, icon, container, children)
    {
      _Editor = editor;

      Children.CollectionChanged += Children_CollectionChanged;
    }

    #endregion

    #region Properties

    [Browsable(false)]
    public SelectionDrivenEditorViewModel ViewModel
    {
      get { return Editor.ViewModel; }
    }

    [Browsable(false)]
    public IEditorPane Editor
    {
      get { return _Editor; }
      set
      {
        _Editor = value;
      }
    }

    public override bool IsSelected
    {
      get
      {
        return base.IsSelected;
      }
      set
      {
        base.IsSelected = value;

        if (value && !Editor.Selection.Contains(this))
          Editor.Selection.Add(this);
        else
          Editor.Selection.Remove(this);
      }
    }

    #endregion
    
    #region Methods

    #endregion

    #region Events

    void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {

    }

    #endregion
  }
}
