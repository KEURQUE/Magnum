using Magnum.Core;
using Magnum.Core.Models;
using Magnum.Core.Services;
using Magnum.Core.ViewModels;
using Magnum.Core.Windows;
using System;

namespace Magnum.Tools.TextDocument.ViewModels
{
  /// <summary>
  /// Class TextViewModel
  /// </summary>
  public class TextViewModel : DocumentViewModel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentViewModel" /> class.
    /// </summary>
    /// <param name="workspace">The injected workspace.</param>
    /// <param name="commandManager">The injected command manager.</param>
    /// <param name="logger">The injected logger.</param>
    public TextViewModel(WorkspaceBase workspace, ICommandManager commandManager, ILoggerService logger)
      : base(workspace, commandManager, logger)
    {
      OpenCommand = commandManager.GetCommand("NEW");
    }

    /// <summary>
    /// The title of the document
    /// </summary>
    /// <value>The tool tip.</value>
    public override string Tooltip
    {
      get { return Model.Location as String; }
      protected set { base.Tooltip = value; }
    }

    /// <summary>
    /// The content ID - unique value for each document. For TextViewModels, the contentID is "FILE:##:" + location of the file.
    /// </summary>
    /// <value>The content id.</value>
    public override string ContentId
    {
      get { return "FILE:##:" + Tooltip; }
    }
  }
}