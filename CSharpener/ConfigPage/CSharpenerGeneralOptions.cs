// --------------------------------------------------------------------
// CSharpenerGeneralOptions.cs Copyright 2020 Craig Gjeltema
// --------------------------------------------------------------------

namespace Gjeltema.CSharpener.ConfigPage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using Gjeltema.CSharpener.Logic;

    internal sealed class CSharpenerGeneralOptions : BaseOptionModel<CSharpenerGeneralOptions>
    {
        private const string DefaultSettingsFilePath = "CSharpener\\Settings.txt";
        private readonly string defaultSettingsFullFilePath;

        public CSharpenerGeneralOptions()
        {
            string commonAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            defaultSettingsFullFilePath = Path.Combine(commonAppFolder, DefaultSettingsFilePath);
            SettingsFilePath = defaultSettingsFullFilePath;
        }

        [Category("Settings File Location")]
        [DisplayName("Settings File Location")]
        [Description("Specifies the location of the settings file for CSharpener")]
        public string SettingsFilePath { get; set; }

        public void InitializeAllConfiguration()
        {
            string configFilePath = SettingsFilePath;

            if (!File.Exists(configFilePath))
            {
                try
                {
                    string directory = Path.GetDirectoryName(configFilePath);
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                    File.WriteAllText(configFilePath, CSharpenerConfigSettings.DefaultConfigurationFile);
                }
                catch
                {
                    SetDefaultSettingsFilePath();
                }
            }

            IList<string> optionsFileContents = File.ReadAllLines(configFilePath);
            CSharpenerConfigSettings.InitializeSettings(optionsFileContents);
        }

        private void SetDefaultSettingsFilePath()
        {
            SettingsFilePath = defaultSettingsFullFilePath;
            File.WriteAllText(SettingsFilePath, CSharpenerConfigSettings.DefaultConfigurationFile);
        }
    }
}
