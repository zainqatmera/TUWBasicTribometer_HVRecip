using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    public class TribometerSettings
    {
        /*        // Singleton - replaced by container singleton
                private TribometerSettings() { }   
                private static TribometerSettings _instance;
                public static TribometerSettings Instance => _instance ??= new TribometerSettings();
        */

        public TribometerSettings()
        {
        }

        public event EventHandler SettingsChanged;

        public bool LoadFromSettingsFile()
        {
            // TODO: Load settings from a settings file (yaml)
            return false;
        }

        public void NotifySettingsChanged()
        {
            SettingsChanged?.Invoke(this, EventArgs.Empty);
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
        public int? stepPosHCentre = 0;     // Steps from limit switch datum to "centre"
        public int? stepPosHLeft = -500;     // Steps from limit switch datum to "left"
        public int? stepPosHRight = 500;     // Steps from limit switch datum to "right"

        public int? stepPosVRaised = 0;
        public int? stepPosVUnloaded = 1000;
        public int? stepPosVLoaded = 1100;

        public string SaveFilePathHorizTests = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public string SaveFilePathVertTests = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Movement steps for manual contrl
        public int moveStepsHPrecisionLow = 1;
        public int moveStepsHPrecisionHigh = 10;
        public int moveStepsHNormalLow = 100;
        public int moveStepsHNormalHigh = 1000;
        public int moveStepsVPrecisionLow = 1;
        public int moveStepsVPrecisionHigh = 10;
        public int moveStepsVNormalLow = 100;
        public int moveStepsVNormalHigh = 1000;

        // Motor settings for manual control
        public float moveMaxSpeedH = 1000;
        public float moveMaxSpeedV = 1000;
        public float moveAccelH = 1000;
        public float moveAccelV = 1000;

        // Motor settings during a vertical reciprocating test
        public float vertTestMaxSpeedH = 1000;
        public float vertTestMaxSpeedV = 1000;
        public float vertTestAccelH = 1000;
        public float vertTestAccelV = 1000;

        // Motor settings during a horizontal reciprocating test
        public float horizTestMaxSpeedH = 1000;
        public float horizTestMaxSpeedV = 1000;
        public float horizTestAccelH = 1000;
        public float horizTestAccelV = 1000;

        // Sensor
        public string CalibrationFile = @"C:\Users\Administrator\Desktop\Tribometer\FT36487.cal";
        public string DaqDevice = "dev1";
        public int ForceSensorSampleRateHz = 1000;
        public int ForceSensorAveragingLevel = 16;
        public int ForceSensorFirstChannel = 0;
        public int ForceSensorMeasureInterval_ms = 100;

        // Vertical Test conditions
        public int vertTestPauseTimeUnloaded = 0;
        public int vertTestPauseTimeLoaded = 0;
        public int vertTestTargetNumberOfCycles = 10;
        public bool vertTestStopAtNumberOfCycles = true;

        // Horizontal Test conditions
        public int horizTestPauseTime = 0;
        public int horizTestTargetNumberOfCycles = 10;
        public bool horizTestStopAtNumberOfCycles = true;
        public NormalLoadingProfile horizTestNormalLoadingProfile = NormalLoadingProfile.ManualControl;
    }
}
