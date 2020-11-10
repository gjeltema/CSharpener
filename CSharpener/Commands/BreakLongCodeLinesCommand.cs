// -----------------------------------------------------------------------
// BreakLongCodeLinesCommand.cs Copyright 2020 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener.Commands
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Gjeltema.CSharpener.Utility;
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.Shell;
    using Task = System.Threading.Tasks.Task;

    internal sealed class BreakLongCodeLinesCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0768;
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("726762de-4df1-4336-bc5b-7021347f4eca");
        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakLongCodeLinesCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private BreakLongCodeLinesCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static BreakLongCodeLinesCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider => package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new BreakLongCodeLinesCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private async void Execute(object sender, EventArgs e) => await FormatCodeAsync();

        private async Task FormatCodeAsync()
        {
            try
            {
                Document activeDocument = await VisualStudioHelper.GetActiveDocumentAsync();
                if (VisualStudioHelper.FileIsExcludedType(activeDocument.FilePath))
                    return;
                SyntaxNode root = await VisualStudioHelper.GetDocumentRootAsync(activeDocument);
                bool isCSharpDocument = root.Language == VisualStudioHelper.CSharpLanguageName;
                if (!isCSharpDocument)
                    return;

                var longLineFormatter = new LongLineFormatter(CSharpenerConfigSettings.LengthOfLineToBreakOn);
                SyntaxNode formattedRoot = longLineFormatter.Visit(root);

                Document newDocument = activeDocument.WithSyntaxRoot(formattedRoot);
                bool success = await VisualStudioHelper.ApplyDocumentChangesAsync(newDocument);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
