using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    public class SettingManualViewModel : BindableBase
    {
        private int _stepsNormalSmallV;
        public int StepsNormalSmallV
        {
            get { return _stepsNormalSmallV; }
            set { SetProperty(ref _stepsNormalSmallV, value); }
        }

        private int _stepsNormalLargeV;
        public int StepsNormalLargeV
        {
            get { return _stepsNormalLargeV; }
            set { SetProperty(ref _stepsNormalLargeV, value); }
        }

        private int _stepsPrecisionSmallV;
        public int StepsPrecisionSmallV
        {
            get { return _stepsPrecisionSmallV; }
            set { SetProperty(ref _stepsPrecisionSmallV, value); }
        }

        private int _stepsPrecisionLargeV;
        public int StepsPrecisionLargeV
        {
            get { return _stepsPrecisionLargeV; }
            set { SetProperty(ref _stepsPrecisionLargeV, value); }
        }

        private int _stepsNormalSmallH;
        public int StepsNormalSmallH
        {
            get { return _stepsNormalSmallH; }
            set { SetProperty(ref _stepsNormalSmallH, value); }
        }

        private int _stepsNormalLargeH;
        public int StepsNormalLargeH
        {
            get { return _stepsNormalLargeH; }
            set { SetProperty(ref _stepsNormalLargeH, value); }
        }

        private int _stepsPrecisionSmallH;
        public int StepsPrecisionSmallH
        {
            get { return _stepsPrecisionSmallH; }
            set { SetProperty(ref _stepsPrecisionSmallH, value); }
        }

        private int _stepsPrecisionLargeH;
        public int StepsPrecisionLargeH
        {
            get { return _stepsPrecisionLargeH; }
            set { SetProperty(ref _stepsPrecisionLargeH, value); }
        }

        private float _maxSpeedH;
        public float MaxSpeedH
        {
            get { return _maxSpeedH; }
            set { SetProperty(ref _maxSpeedH, value); }
        }

        private float _accelH;
        public float AccelH
        {
            get { return _accelH; }
            set { SetProperty(ref _accelH, value); }
        }

        private float _maxSpeedV;
        public float MaxSpeedV
        {
            get { return _maxSpeedV; }
            set { SetProperty(ref _maxSpeedV, value); }
        }

        private float _accelV;
        public float AccelV
        {
            get { return _accelV; }
            set { SetProperty(ref _accelV, value); }
        }
    }
}
