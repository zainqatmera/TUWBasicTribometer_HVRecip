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

        public int HorizStepsPerRevolution = 200;
        public double HorizMovementPerRevolution_um = 4000;

        public int VertStepsPerRevolution = 200;
        public double VertMovementPerRevolution_um = 2000;

        public double HorizMovementRange_um = 100000;
        public double VertMovementRange_um = 100000;

        public string ComPortTribometer = "COM4";
        public int ComPortTribometerBaudRate = 115200;
    }
}
