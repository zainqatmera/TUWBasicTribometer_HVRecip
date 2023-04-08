using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    public class TribometerSettings
    {
        /*        // Singleton - replaced by container singleton
                private TribometerSettings() { }   
                private static TribometerSettings _instance;
                public static TribometerSettings Instance => _instance ??= new TribometerSettings();
        */

        const string settingsFileName = "TribometerSettings.ini";

        public TribometerSettings()
        {
            LoadFromSettingsFile();
        }

        public event EventHandler SettingsChanged;

        public void LoadFromSettingsFile()
        {
            if (!File.Exists(settingsFileName)) { return; }

            StreamReader sr = new StreamReader(settingsFileName);
            
            while (!sr.EndOfStream)
            {
                var curline = sr.ReadLine();
                int seperator = curline.IndexOf('=');
                if (seperator > 0)
                {
                    var item = curline.Substring(0, seperator).TrimEnd();
                    var val = curline.Substring(seperator + 1);

                    switch (item)
                    {
                        case "HorizStepsPerRevolution": HorizStepsPerRevolution = Convert.ToInt32(val); break;
                        case "HorizMovementPerRevolution_um": HorizMovementPerRevolution_um = Convert.ToDouble(val); break;
                        case "VertStepsPerRevolution": VertStepsPerRevolution = Convert.ToInt32(val); break;
                        case "VertMovementPerRevolution_um": VertMovementPerRevolution_um = Convert.ToDouble(val); break;
                        case "HorizMovementRange_um": HorizMovementRange_um = Convert.ToDouble(val); break;
                        case "VertMovementRange_um": VertMovementRange_um = Convert.ToDouble(val); break;
                        case "ComPortTribometer": ComPortTribometer = val.Trim(); break;
                        case "stepPosHCentre": stepPosHCentre = Convert.ToInt32(val); break;
                        case "stepPosHLeft": stepPosHLeft = Convert.ToInt32(val); break;
                        case "stepPosHRight": stepPosHRight = Convert.ToInt32(val); break;
                        case "stepPosVRaised": stepPosVRaised = Convert.ToInt32(val); break;
                        case "stepPosVUnloaded": stepPosVUnloaded = Convert.ToInt32(val); break;
                        case "stepPosVLoaded": stepPosVLoaded = Convert.ToInt32(val); break;
                        case "SaveFilePathHorizTests": SaveFilePathHorizTests = val.Trim(); break;
                        case "SaveFilePathVertTests": SaveFilePathVertTests = val.Trim(); break;
                        case "moveStepsHPrecisionLow": moveStepsHPrecisionLow = Convert.ToInt32(val); break;
                        case "moveStepsHPrecisionHigh": moveStepsHPrecisionHigh = Convert.ToInt32(val); break;
                        case "moveStepsHNormalLow": moveStepsHNormalLow = Convert.ToInt32(val); break;
                        case "moveStepsHNormalHigh": moveStepsHNormalHigh = Convert.ToInt32(val); break;
                        case "moveStepsVPrecisionLow": moveStepsVPrecisionLow = Convert.ToInt32(val); break;
                        case "moveStepsVPrecisionHigh": moveStepsVPrecisionHigh = Convert.ToInt32(val); break;
                        case "moveStepsVNormalLow": moveStepsVNormalLow = Convert.ToInt32(val); break;
                        case "moveStepsVNormalHigh": moveStepsVNormalHigh = Convert.ToInt32(val); break;
                        case "moveMaxSpeedH": moveMaxSpeedH = Convert.ToSingle(val); break;
                        case "moveMaxSpeedV": moveMaxSpeedV = Convert.ToSingle(val); break;
                        case "moveAccelH": moveAccelH = Convert.ToSingle(val); break;
                        case "moveAccelV": moveAccelV = Convert.ToSingle(val); break;
                        case "vertTestMaxSpeedH": vertTestMaxSpeedH = Convert.ToSingle(val); break;
                        case "vertTestMaxSpeedV": vertTestMaxSpeedV = Convert.ToSingle(val); break;
                        case "vertTestAccelH": vertTestAccelH = Convert.ToSingle(val); break;
                        case "vertTestAccelV": vertTestAccelV = Convert.ToSingle(val); break;
                        case "horizTestMaxSpeedH": horizTestMaxSpeedH = Convert.ToSingle(val); break;
                        case "horizTestMaxSpeedV": horizTestMaxSpeedV = Convert.ToSingle(val); break;
                        case "horizTestAccelH": horizTestAccelH = Convert.ToSingle(val); break;
                        case "horizTestAccelV": horizTestAccelV = Convert.ToSingle(val); break;
                        case "CalibrationFile": CalibrationFile = val.Trim(); break;
                        case "DaqDevice": DaqDevice = val.Trim(); break;
                        case "ForceSensorSampleRateHz": ForceSensorSampleRateHz = Convert.ToInt32(val); break;
                        case "ForceSensorAveragingLevel": ForceSensorAveragingLevel = Convert.ToInt32(val); break;
                        case "ForceSensorFirstChannel": ForceSensorFirstChannel = Convert.ToInt32(val); break;
                        case "ForceSensorMeasureInterval_ms": ForceSensorMeasureInterval_ms = Convert.ToInt32(val); break;
                        case "vertTestPauseTimeUnloaded": vertTestPauseTimeUnloaded = Convert.ToInt32(val); break;
                        case "vertTestPauseTimeLoaded": vertTestPauseTimeLoaded = Convert.ToInt32(val); break;
                        case "vertTestTargetNumberOfCycles": vertTestTargetNumberOfCycles = Convert.ToInt32(val); break;
                        case "vertTestStopAtNumberOfCycles": vertTestStopAtNumberOfCycles = Convert.ToBoolean(val); break;
                        case "horizTestPauseTime": horizTestPauseTime = Convert.ToInt32(val); break;
                        case "horizTestTargetNumberOfCycles": horizTestTargetNumberOfCycles = Convert.ToInt32(val); break;
                        case "horizTestStopAtNumberOfCycles": horizTestStopAtNumberOfCycles = Convert.ToBoolean(val); break;
                        case "horizTestNormalLoadingProfile": horizTestNormalLoadingProfile = (NormalLoadingProfile)Convert.ToInt32(val); break;
                    }
                }
            }
        }

        public void SaveSettings()
        {
            StreamWriter sw = new StreamWriter(settingsFileName);
            sw.WriteLine($"HorizStepsPerRevolution = {HorizStepsPerRevolution}");
            sw.WriteLine($"HorizMovementPerRevolution_um = {HorizMovementPerRevolution_um}");
            sw.WriteLine($"VertStepsPerRevolution = {VertStepsPerRevolution}");
            sw.WriteLine($"VertMovementPerRevolution_um = {VertMovementPerRevolution_um}");
            sw.WriteLine($"HorizMovementRange_um = {HorizMovementRange_um}");
            sw.WriteLine($"VertMovementRange_um = {VertMovementRange_um}");
            sw.WriteLine($"ComPortTribometer = {ComPortTribometer}");
            // TODO: Reinstate these lines only when homing datum is used - they will be useless to save until the datum is applied
            //sw.WriteLine($"stepPosHCentre = {stepPosHCentre}");
            //sw.WriteLine($"stepPosHLeft = {stepPosHLeft}");
            //sw.WriteLine($"stepPosHRight = {stepPosHRight}");
            //sw.WriteLine($"stepPosVRaised = {stepPosVRaised}");
            //sw.WriteLine($"stepPosVUnloaded = {stepPosVUnloaded}");
            //sw.WriteLine($"stepPosVLoaded = {stepPosVLoaded}");
            sw.WriteLine($"SaveFilePathHorizTests = {SaveFilePathHorizTests}");
            sw.WriteLine($"SaveFilePathVertTests = {SaveFilePathVertTests}");
            sw.WriteLine($"moveStepsHPrecisionLow = {moveStepsHPrecisionLow}");
            sw.WriteLine($"moveStepsHPrecisionHigh = {moveStepsHPrecisionHigh}");
            sw.WriteLine($"moveStepsHNormalLow = {moveStepsHNormalLow}");
            sw.WriteLine($"moveStepsHNormalHigh = {moveStepsHNormalHigh}");
            sw.WriteLine($"moveStepsVPrecisionLow = {moveStepsVPrecisionLow}");
            sw.WriteLine($"moveStepsVPrecisionHigh = {moveStepsVPrecisionHigh}");
            sw.WriteLine($"moveStepsVNormalLow = {moveStepsVNormalLow}");
            sw.WriteLine($"moveStepsVNormalHigh = {moveStepsVNormalHigh}");
            sw.WriteLine($"moveMaxSpeedH = {moveMaxSpeedH}");
            sw.WriteLine($"moveMaxSpeedV = {moveMaxSpeedV}");
            sw.WriteLine($"moveAccelH = {moveAccelH}");
            sw.WriteLine($"moveAccelV = {moveAccelV}");
            sw.WriteLine($"vertTestMaxSpeedH = {vertTestMaxSpeedH}");
            sw.WriteLine($"vertTestMaxSpeedV = {vertTestMaxSpeedV}");
            sw.WriteLine($"vertTestAccelH = {vertTestAccelH}");
            sw.WriteLine($"vertTestAccelV = {vertTestAccelV}");
            sw.WriteLine($"horizTestMaxSpeedH = {horizTestMaxSpeedH}");
            sw.WriteLine($"horizTestMaxSpeedV = {horizTestMaxSpeedV}");
            sw.WriteLine($"horizTestAccelH = {horizTestAccelH}");
            sw.WriteLine($"horizTestAccelV = {horizTestAccelV}");
            sw.WriteLine($"CalibrationFile = {CalibrationFile}");
            sw.WriteLine($"DaqDevice = {DaqDevice}");
            sw.WriteLine($"ForceSensorSampleRateHz = {ForceSensorSampleRateHz}");
            sw.WriteLine($"ForceSensorAveragingLevel = {ForceSensorAveragingLevel}");
            sw.WriteLine($"ForceSensorFirstChannel = {ForceSensorFirstChannel}");
            sw.WriteLine($"ForceSensorMeasureInterval_ms = {ForceSensorMeasureInterval_ms}");
            sw.WriteLine($"vertTestPauseTimeUnloaded = {vertTestPauseTimeUnloaded}");
            sw.WriteLine($"vertTestPauseTimeLoaded = {vertTestPauseTimeLoaded}");
            sw.WriteLine($"vertTestTargetNumberOfCycles = {vertTestTargetNumberOfCycles}");
            sw.WriteLine($"vertTestStopAtNumberOfCycles = {vertTestStopAtNumberOfCycles}");
            sw.WriteLine($"horizTestPauseTime = {horizTestPauseTime}");
            sw.WriteLine($"horizTestTargetNumberOfCycles = {horizTestTargetNumberOfCycles}");
            sw.WriteLine($"horizTestStopAtNumberOfCycles = {horizTestStopAtNumberOfCycles}");
            sw.WriteLine($"horizTestNormalLoadingProfile = {(byte)horizTestNormalLoadingProfile}");
            sw.Close();
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
        // TODO: We need to set these based on homing values and modify all code so the value sent are offset by the homing datums (in firmware or software?)
        public int? stepPosHCentre = null;     // Steps from limit switch datum to "centre"
        public int? stepPosHLeft = null;     // Steps from limit switch datum to "left"
        public int? stepPosHRight = null;     // Steps from limit switch datum to "right"

        public int? stepPosVRaised = null;
        public int? stepPosVUnloaded = null;
        public int? stepPosVLoaded = null;

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
        public int ForceSensorMeasureInterval_ms = 50;  // How often updates are provided, and also saved to file

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
