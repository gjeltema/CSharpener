
namespace Gjeltema.CSharpener.ConfigPage
{
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// A base class for a DialogPage to show in Tools -> Options.
    /// </summary>
    internal class BaseOptionPage<T> : DialogPage where T : BaseOptionModel<T>, new()
    {
        private readonly BaseOptionModel<T> model;

        public BaseOptionPage()
        {
            model = ThreadHelper.JoinableTaskFactory.Run(BaseOptionModel<T>.CreateAsync);
        }

        public override object AutomationObject => model;

        public override void LoadSettingsFromStorage()
            => model.Load();

        public override void SaveSettingsToStorage()
            => model.Save();
    }
}
