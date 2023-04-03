using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip
{
    internal class PrismRegion
    {
        public const string MainContentRegion = nameof(MainContentRegion);
        public const string LogContentRegion = nameof(LogContentRegion);
        public const string StatusRegion = nameof(StatusRegion);
        public const string SettingsTabControlRegion = nameof(SettingsTabControlRegion);
    }

    internal class PrismNavigationUri
    {
        public const string Setup = nameof(Setup);
        public const string Manual = nameof(Manual);
        public const string HorizRecip = nameof(HorizRecip);
        public const string VertRecip = nameof(VertRecip);
        public const string Settings = nameof(Settings);
        public const string Log = nameof(Log);    
        public const string Status = nameof(Status);    

        public const string ManualSettings = nameof(ManualSettings);
        public const string VertRecipSettings = nameof(VertRecipSettings);
    }

}
