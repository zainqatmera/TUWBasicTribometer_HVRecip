using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NationalInstruments.DAQmx;
using ATICombinedDAQFT;
using System.Threading;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    public class ForceSensor : IDisposable
    {
        FTSystem ftSystem;

        public TribometerSettings Settings { get; set; }

        bool IsSampling => samplingTimer != null;

        Timer samplingTimer = null;

        public string ErrorMessage { get; private set; }

        public event EventHandler ForceSensorError;  // Raised when force sensor unable to read data
        public event EventHandler<ForceSensorEventArgs> ForceSensorDataAvailable;

        private double[] ForceTorqueValuesRaw = new double[6];
        private double[] ForceTorqueZeroNegativeOffsets = new double[6];        // The negative of the offsets so use addition for actual value from raw

        public ForceSensor()
        {
            for (int i = 0; i < 6; i++) ForceTorqueZeroNegativeOffsets[i] = 0;
        }

        public bool StartSystem()
        {
            ftSystem = new FTSystem();
            int status = ftSystem.LoadCalibrationFile(Settings.CalibrationFile, 1);
            if (status != 0)
            {
                ErrorMessage = "Unable to load calibration file";
                return false;
            }

            status = ftSystem.SetForceUnits("N");
            if (status != 0)
            {
                ErrorMessage = "Unable to set force units";
                return false;
            }

            status = ftSystem.SetTorqueUnits("N-m");
            if (status != 0) 
            {
                ErrorMessage = "Unable to set torque units";
                return false;
            }

            ftSystem.SetConnectionMode(ConnectionType.REFERENCED_SINGLE_ENDED);

            status = ftSystem.StartSingleSampleAcquisition(Settings.DaqDevice, Settings.ForceSensorSampleRateHz,
                Settings.ForceSensorAveragingLevel, Settings.ForceSensorFirstChannel, ftSystem.GetTempCompEnabled());
            if (status != 0)
            {
                ErrorMessage = "Unable to start Force Sensor Sampling";
                return false;
            }

            samplingTimer = new Timer(SamplingTimerTick);
            samplingTimer.Change(Settings.ForceSensorMeasureInterval_ms, Settings.ForceSensorMeasureInterval_ms); 
            return true;
        }

        public void SetZeroNow()
        {
            for (int i = 0; i < 6; i++)
                ForceTorqueZeroNegativeOffsets[i] = -ForceTorqueValuesRaw[i]; 
        }

        // Set the force values for zero datum 
        public void SetZeroDatum(double[] zeroOffset)
        {
            for (int i = 0; i < 6; i++)
                ForceTorqueZeroNegativeOffsets[i] = -zeroOffset[i];
        }

        private void SamplingTimerTick(object state)
        {
            int status = ftSystem.ReadSingleFTRecord(ForceTorqueValuesRaw);
            if (status != 0)
            {
                samplingTimer.Dispose();
                samplingTimer = null;
                ForceSensorError?.Invoke(this, null);
                return;
            }

            ForceSensorEventArgs e = new ForceSensorEventArgs();
            for (int i = 0; i < 6; i++) 
                e.FTValues[i] = ForceTorqueValuesRaw[i] + ForceTorqueZeroNegativeOffsets[i];            
            ForceSensorDataAvailable?.Invoke(this, e);
        }

        public void Dispose()
        {
            samplingTimer.Dispose();
        }
    }
}
