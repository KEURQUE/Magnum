using Magnum.Core.Services;
using Magnum.Tools.SelectionDrivenEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Tools.SelectionDrivenEditor.ViewModels
{
  public class SelectionSnapshot
  {
    private SelectionDrivenEditorViewModel _viewModel;

    private List<ITreeNodeItem> _dataPaneSelection;
    private List<ITreeNodeItem> _editorPaneSelection;
    private List<ITreeNodeItem> _contextualPaneSelection;

    public SelectionSnapshot(SelectionDrivenEditorViewModel viewModel)
    {
      _viewModel = viewModel;

      _dataPaneSelection = viewModel.DataPane.FindItems();
      _editorPaneSelection = viewModel.EditorPane != null ? viewModel.EditorPane.FindItems() : new List<ITreeNodeItem>();
      _contextualPaneSelection = viewModel.ContextualPane != null ? viewModel.ContextualPane.FindItems() : new List<ITreeNodeItem>();
    }

    public SelectionDrivenEditorViewModel ViewModel { get { return _viewModel; } }

    public List<ITreeNodeItem> DataPaneSelection { get { return _dataPaneSelection; } }
    public List<ITreeNodeItem> EditorPaneSelection { get { return _editorPaneSelection; } }
    public List<ITreeNodeItem> ContextualPaneSelection { get { return _contextualPaneSelection; } }
  }
}
