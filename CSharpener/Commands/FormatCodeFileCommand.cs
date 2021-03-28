// -----------------------------------------------------------------------
// FormatCodeFileCommand.cs Copyright 2020 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener.Commands
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.IO;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Gjeltema.CSharpener.Logic.Usings;
    using Gjeltema.CSharpener.Utility;
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.Shell;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class FormatCodeFileCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0766;
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("726762de-4df1-4336-bc5b-7021347f4eca");
        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatCodeFileCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private FormatCodeFileCommand(AsyncPackage package, OleMenuCommandService commandService)
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
        public static FormatCodeFileCommand Instance
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
            Instance = new FormatCodeFileCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private async void Execute(object sender, EventArgs e) 
            => await FormatCodeAsync();

        private async Task FormatCodeAsync()
        {
            //** Keep this commented code around - will likely need this CompilationUnitSyntax for other things.
            // var cus = (CompilationUnitSyntax)rootNode;

            try
            {
                Document activeDocument = await VisualStudioHelper.GetActiveDocumentAsync();
                if (VisualStudioHelper.FileIsExcludedType(activeDocument.FilePath))
                    return;

                SyntaxNode root = await VisualStudioHelper.GetDocumentRootAsync(activeDocument);
                bool isCSharpDocument = root.Language == VisualStudioHelper.CSharpLanguageName;
                if (!isCSharpDocument)
                    return;

                var regionRemover = new RegionRemover();
                SyntaxNode regionRoot = regionRemover.Visit(root);

                var usingsPlacer = new UsingsPlacer();
                SyntaxNode usingsRoot = usingsPlacer.ProcessUsings(regionRoot);

                string fileName = Path.GetFileName(activeDocument.FilePath);
                var fhf = new FileHeaderFormatter();
                SyntaxNode fhfRoot = fhf.AddHeader(usingsRoot, fileName);

                var ebf = new ExpressionBodiedFormatter();
                SyntaxNode ebfRoot = ebf.Visit(fhfRoot);

                var newLineFormatter = new NewlineFormatter();
                SyntaxNode formattedRoot = newLineFormatter.Visit(ebfRoot);

                Document newDocument = activeDocument.WithSyntaxRoot(formattedRoot);
                bool success = await VisualStudioHelper.ApplyDocumentChangesAsync(newDocument);

                await VisualStudioHelper.InvokeCommandAsync(VisualStudioHelper.RunDefaultCodeCleanup);
                await VisualStudioHelper.InvokeCommandAsync(VisualStudioHelper.FormatDocumentCommandName);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
