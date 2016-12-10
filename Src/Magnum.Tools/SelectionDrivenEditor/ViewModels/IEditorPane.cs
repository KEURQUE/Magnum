using Magnum.Core.Services;
using Magnum.Tools.SelectionDrivenEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Tools.SelectionDrivenEditor.ViewModels
{
  public interface IEditorPane
  {
    SelectionDrivenEditorViewModel ViewModel { get; set; }

    ObservableCollection<ObjectTreeNode> Items { get; set; }

    ObservableCollection<ITreeNodeItem> Selection { get; set; }

    SelectionDrivenEditor.ViewModels.SelectionDrivenEditorViewModel.EditorTypeEnum EditorType { get; set; }

    void SetSelection(IEnumerable<ITreeNodeItem> objectsToSelect, SelectionSnapshot snapshot);

    List<ITreeNodeItem> FindItems(List<ITreeNodeItem> foundItems = null, ITreeNodeItem currentItem = null);
  }
}
