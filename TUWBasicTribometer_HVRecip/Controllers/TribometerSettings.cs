using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    internal class TribometerSettings
    {
        // Singleton
        private TribometerSettings() { }   
        private static TribometerSettings _instance;
        public static TribometerSettings Instance => _instance ??= new TribometerSettings();

        public bool LoadFromSettingsFile()
        {
            // TODO: Load settings from a settings file (yaml)
            return false;
        }

        // Design Fixed
        public int HorizStepsPerRevolution = 200;
        public double HorizMovementPerRevolution_um = 4000;

        public int VertStepsPerRevolution = 200;
        public double VertMovementPerRevolution_um = 2000;

        public double HorizMovementRange_um = 100000;
        public double VertMovementRange_um = 100000;

        // Tribometer Connection
        public string ComPortTribometer = "COM4";
        public int ComPortTribometerBaudRate = 115200;

        // User-settings 
        public int stepPosHCentre = 1000;     // Steps from limit switch datum to "centre"

        // Movement steps for manual contrl
        public int moveStepsHPrecisionLow = 1;
        public int moveStepsHPrecisionHigh = 10;
        public int moveStepsHNormalLow = 100;
        public int moveStepsHNormalHigh = 1000;
        public int moveStepsVPrecisionLow = 1;
        public int moveStepsVPrecisionHigh = 10;
        public int moveStepsVNormalLow = 100;
        public int moveStepsVNormalHigh = 1000;

        // Motor settings for maual control
        public float moveMaxSpeedH = 1000;
        public float moveMaxSpeedV = 1000;
        public float moveAccelH = 1000;
        public float moveAccelV = 1000;

        // Motor settings during a vertical reciprocating test
        public float vertTestMaxSpeedH = 1000;
        public float vertTestMaxSpeedV = 1000;
        public float vertTestAccelH = 1000;
        public float vertTestAccelV = 1000;

    }
}
