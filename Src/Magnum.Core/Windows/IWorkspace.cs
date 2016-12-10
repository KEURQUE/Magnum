using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shell;

namespace Magnum.Core.Windows
{
  /// <summary>
  /// Interface IWorkspace
  /// </summary>
  public interface IWorkspace
  {
    /// <summary>
    /// The list of documents which are open in the workspace
    /// </summary>
    ObservableCollection<DocumentViewModel> Documents { get; set; }

    /// <summary>
    /// The list of tools that are available in the workspace
    /// </summary>
    ObservableCollection<ToolViewModel> Tools { get; set; }

    /// <summary>
    /// The list of searchable items that are available in the workspace
    /// </summary>
    ObservableCollection<ISearchable> SearchableItems { get; set; }

    /// <summary>
    /// The current document which is active in the workspace
    /// </summary>
    DocumentViewModel ActiveDocument { get; set; }

    /// <summary>
    /// The last tool that was active in the workspace
    /// </summary>
    ToolViewModel LastActiveTool { get; set; }

    /// <summary>
    /// The current tool which is active in the workspace
    /// </summary>
    ToolViewModel ActiveTool { get; set; }

    /// <summary>
    /// Gets the title of the application.
    /// </summary>
    /// <value>The title.</value>
    string Title { get; }

    /// <summary>
    /// Gets the icon of the application.
    /// </summary>
    /// <value>The icon.</value>
    ImageSource Icon { get; }

    /// <summary>
    /// Is the main window busy.
    /// </summary>
    /// <value>The main window is busy or not.</value>
    bool IsBusy { get; set; }

    /// <summary>
    /// Is the main window showing a message box
    /// </summary>
    /// <value>The main window is showing a message box or not.</value>
    bool IsMessageBoxShowing { get; set; }
    
    /// <summary>
    /// Is the tools library opened
    /// </summary>
    /// <value>The tools library opened or not.</value>
    bool IsToolsLibraryOpened { get; set; }

    /// <summary>
    /// Is the quick library opened
    /// </summary>
    /// <value>The quick library opened or not.</value>
    bool IsQuickLaunchOpened { get; set; }

    /// <summary>
    /// Closing this instance.
    /// </summary>
    /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
    /// <returns><c>true</c> if the application is closing, <c>false</c> otherwise</returns>
    bool Closing(CancelEventArgs e);
  }
}
