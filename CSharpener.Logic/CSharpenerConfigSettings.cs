// --------------------------------------------------------------------
// CSharpenerConfigSettings.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic
{
    using System;
    using System.Collections.Generic;

    public static class CSharpenerConfigSettings
    {
        public const string AccessModifierSectionName = "ACCESSMODIFIER_SORT_ORDER";
        public const string DefaultConfigurationFile =
@"V 1.0

ACCESSMODIFIER_SORT_ORDER
public
internal
protected
protectedinternal
private
privateprotected
ACCESSMODIFIER_SORT_ORDER_END

KIND_SORT_ORDER
enum
delegate
eventField
field
event
constructor
destructor
indexer
property
operator
method
struct
class
interface
KIND_SORT_ORDER_END

MODIFIER_SORT_ORDER
kind
extern
const
readonly
accessibility
static
name
numberofmethodargs
MODIFIER_SORT_ORDER_END

LINE_BREAK_LENGTH
150
LINE_BREAK_LENGTH_END
";
        public const string HeaderSectionName = "HEADER";
        public const string KindSortOrderSectionName = "KIND_SORT_ORDER";
        public const string LineBreakLengthName = "LINE_BREAK_LENGTH";
        public const string ModifierSectionName = "MODIFIER_SORT_ORDER";
        private const string SectionEnding = "_END";
        private static IList<string> configurationItems;

        public static int LengthOfLineToBreakOn { get; private set; }

        public static IList<string> GetConfigItemsForSection(string configurationSectionName)
        {
            IList<string> configItems = new List<string>();
            int indexOfSection = GetStartingIndex(configurationSectionName);
            if (indexOfSection == -1 || indexOfSection == configurationItems.Count - 1)
                return configItems;

            string sectionEnd = configurationSectionName + SectionEnding;
            int configLineIndex = indexOfSection + 1;
            string configLine = configurationItems[configLineIndex].Trim();
            while (configLine != sectionEnd)
            {
                configItems.Add(configLine);
                configLineIndex++;
                configLine = configurationItems[configLineIndex].Trim();
            }
            return configItems;
        }

        public static string GetHeaderTemplate()
        {
            IList<string> headerConfig = GetConfigItemsForSection(HeaderSectionName);
            if (headerConfig.Count == 0)
                return string.Empty;

            string headerTemplate = string.Join(Environment.NewLine, headerConfig);
            return headerTemplate;
        }

        public static void InitializeSettings(IList<string> configurationItems)
        {
            CSharpenerConfigSettings.configurationItems = configurationItems;
            InitializeLengthOfLine();
        }

        private static int GetStartingIndex(string configurationSectionName)
            => configurationItems.IndexOf(configurationSectionName);

        private static void InitializeLengthOfLine()
        {
            IList<string> lineLengthRaw = GetConfigItemsForSection(LineBreakLengthName);
            if (lineLengthRaw.Count != 1)
                return;

            bool ableToParse = int.TryParse(lineLengthRaw[0], out int lengthOfLine);
            LengthOfLineToBreakOn = ableToParse ? lengthOfLine : 150;
        }
    }
}
