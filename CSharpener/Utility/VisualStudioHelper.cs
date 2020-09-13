// --------------------------------------------------------------------
// VisualStudioHelper.cs Copyright 2020 Craig Gjeltema
// --------------------------------------------------------------------

namespace Gjeltema.CSharpener.Utility
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.LanguageServices;
    using Microsoft.VisualStudio.Shell;

    internal static class VisualStudioHelper
    {
        public const string CSharpLanguageName = "C#";
        public const string FormatDocumentCommandName = "Edit.FormatDocument";
        public const string RemoveAndSortUsingsCommandName = "EditorContextMenus.CodeWindow.RemoveAndSort";
        public const string RunDefaultCodeCleanup = "EditorContextMenus.FileHealthIndicator.RunDefaultCodeCleanup";
        private static AsyncPackage package;

        internal async static Task<bool> ApplyDocumentChangesAsync(Document document)
        {
            if (document == null)
                return false;

            VisualStudioWorkspace workspace = await GetVisualStudioWorkspaceAsync();
            Solution newSolution = document.Project.Solution;
            if (workspace.CanApplyChange(ApplyChangesKind.ChangeDocument))
            {
                return workspace.TryApplyChanges(newSolution);
            }
            return false;
        }

        internal static bool FileIsExcludedType(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return false;
            }

            if (filePath.EndsWith(".Designer.cs", StringComparison.OrdinalIgnoreCase) || filePath.EndsWith(".Generated.cs", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        internal async static Task<Document> GetActiveDocumentAsync()
        {
            EnvDTE80.DTE2 dte = await GetDTEServiceAsync();
            EnvDTE.Document activeDocument = dte?.ActiveDocument;
            if (activeDocument == null)
                return null;

            VisualStudioWorkspace workspace = await GetVisualStudioWorkspaceAsync();
            ImmutableArray<DocumentId> documentIds = workspace.CurrentSolution.GetDocumentIdsWithFilePath(activeDocument.FullName);
            if (documentIds.Length == 0)
                return null;
            DocumentId documentId = documentIds[0];
            Document document = workspace.CurrentSolution.GetDocument(documentId);

            return document;
        }

        internal async static Task<SyntaxNode> GetDocumentRootAsync(Document document)
        {
            SyntaxTree syntaxTree = await document.GetSyntaxTreeAsync();
            SyntaxNode root = await syntaxTree.GetRootAsync();
            return root;
        }

        internal static void Initialize(AsyncPackage asyncPackage)
            => package = asyncPackage;

        internal async static System.Threading.Tasks.Task InvokeCommandAsync(string commandName)
        {
            EnvDTE80.DTE2 ide = await GetDTEServiceAsync();
            EnvDTE.Command command = ide.Commands.OfType<EnvDTE.Command>().FirstOrDefault(x => x.Name.Equals(commandName));
            ide.ExecuteCommand(command.Name);
        }

        private async static Task<IComponentModel> GetComponentModelAsync()
        {
            var componentModel = (IComponentModel)await package.GetServiceAsync(typeof(SComponentModel));
            return componentModel;
        }

        private async static Task<EnvDTE80.DTE2> GetDTEServiceAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            return dte;
        }

        private async static Task<VisualStudioWorkspace> GetVisualStudioWorkspaceAsync()
        {
            IComponentModel componentModel = await GetComponentModelAsync();
            VisualStudioWorkspace visualStudioWorkspace = componentModel.GetService<VisualStudioWorkspace>();
            return visualStudioWorkspace;
        }
    }
}
