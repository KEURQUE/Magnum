using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Magnum.Core.Services
{
  public interface ITreeNodeItem
  {
    string Title { get; }

    IItemType ItemType { get; }

    BitmapSource Icon { get; }

    bool IsSelected { get; set; }

    ContextMenu ContextMenu { get; set; }

    bool IsCriteriaMatched(string criteria);

    void Add(ITreeNodeItem newItem);

    void ApplyCriteria(string criteria, Stack<ITreeNodeItem> ancestors);

    ObservableCollection<ITreeNodeItem> Children { get; }

    bool IsExpanded { get; set; }

    bool IsMatch { get; set; }

    bool IsLeaf { get; }
  }
}
