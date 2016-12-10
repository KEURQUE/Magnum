using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Magnum.Core.Services
{
  public interface IUndoableCommand : ICommand, IDisposable
  {
    ICommandManager Manager { get; set; }

    bool Initialize();

    void Undo(object parameter);

    void Redo(object parameter);

    void RaiseCanExecuteChanged();

    bool IsNewCommand { get; }
  }
}
