// -----------------------------------------------------------------------
// CSharpenerPackage.cs Copyright 2020 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Gjeltema.CSharpener.Commands;
    using Gjeltema.CSharpener.ConfigPage;
    using Gjeltema.CSharpener.Utility;
    using Microsoft.VisualStudio.Shell;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideOptionPage(typeof(DialogPageProvider.General), "CSharpener", "CSharpener Options", 0, 0, true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class CSharpenerPackage : AsyncPackage
    {
        /// <summary>
        /// CSharpenerPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "ec250ee0-f916-4335-9764-17b6cd3573fc";

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            VisualStudioHelper.Initialize(this);
            await InitializeConfigurationAsync();
            await FormatCodeFileCommand.InitializeAsync(this);
            await FormatAndSortCodeFileCommand.InitializeAsync(this);
            await BreakLongCodeLinesCommand.InitializeAsync(this);
        }

        private async Task InitializeConfigurationAsync()
        {
            CSharpenerGeneralOptions optionsPage = await CSharpenerGeneralOptions.GetLiveInstanceAsync();
            optionsPage.InitializeAllConfiguration();
        }
    }
}
