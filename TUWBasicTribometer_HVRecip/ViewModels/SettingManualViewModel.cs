using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    public class SettingManualViewModel : BindableBase
    {
        TribometerController _controller;
        TribometerSettings _settings;   

        public SettingManualViewModel(IContainer container)
        {
            _controller = container.Resolve<TribometerController>();    
            _settings = container.Resolve<TribometerSettings>();

            StepsNormalLargeH = _settings.moveStepsHNormalHigh;
            StepsNormalSmallH = _settings.moveStepsHNormalLow;
            StepsPrecisionLargeH = _settings.moveStepsHPrecisionHigh;
            StepsPrecisionSmallH = _settings.moveStepsHPrecisionLow;
            StepsNormalLargeV = _settings.moveStepsVNormalHigh;
            StepsNormalSmallV = _settings.moveStepsVNormalLow;
            StepsPrecisionLargeV = _settings.moveStepsVPrecisionHigh;
            StepsPrecisionSmallV = _settings.moveStepsVPrecisionLow;

            MaxSpeedH = _settings.moveMaxSpeedH;
            MaxSpeedV = _settings.moveMaxSpeedV;
            AccelH = _settings.moveAccelH;
            AccelV = _settings.moveAccelV;

            UpdateHorizontalSettingsCommand = new DelegateCommand(UpdateHorizontalSettings);
            UpdateVerticalSettingsCommand = new DelegateCommand(UpdateVerticalSettings);
        }

        private void UpdateHorizontalSettings()
        {
            // TODO
        }

        private void UpdateVerticalSettings()
        {
            // TODO
        }

        public DelegateCommand UpdateHorizontalSettingsCommand { get; }
        public DelegateCommand UpdateVerticalSettingsCommand { get; }


        public string Title { get; } = "Manual";

        private int _stepsNormalSmallV;
        public int StepsNormalSmallV
        {
            get { return _stepsNormalSmallV; }
            set 
            { 
                SetProperty(ref _stepsNormalSmallV, value); 
                _settings.moveStepsVNormalLow = value;
            }
        }

        private int _stepsNormalLargeV;
        public int StepsNormalLargeV
        {
            get { return _stepsNormalLargeV; }
            set
            {
                SetProperty(ref _stepsNormalLargeV, value);
                _settings.moveStepsVNormalHigh = value;
            }
        }

        private int _stepsPrecisionSmallV;
        public int StepsPrecisionSmallV
        {
            get { return _stepsPrecisionSmallV; }
            set
            {
                SetProperty(ref _stepsPrecisionSmallV, value);
                _settings.moveStepsHPrecisionLow = value;
            }
        }

        private int _stepsPrecisionLargeV;
        public int StepsPrecisionLargeV
        {
            get { return _stepsPrecisionLargeV; }
            set
            {
                SetProperty(ref _stepsPrecisionLargeV, value);
                _settings.moveStepsVPrecisionHigh = value;
            }
        }

        private int _stepsNormalSmallH;
        public int StepsNormalSmallH
        {
            get { return _stepsNormalSmallH; }
            set
            {
                SetProperty(ref _stepsNormalSmallH, value);
                _settings.moveStepsHNormalLow = value;
            }
        }

        private int _stepsNormalLargeH;
        public int StepsNormalLargeH
        {
            get { return _stepsNormalLargeH; }
            set
            {
                SetProperty(ref _stepsNormalLargeH, value);
                _settings.moveStepsHNormalHigh = value;
            }
        }

        private int _stepsPrecisionSmallH;
        public int StepsPrecisionSmallH
        {
            get { return _stepsPrecisionSmallH; }
            set
            {
                SetProperty(ref _stepsPrecisionSmallH, value);
                _settings.moveStepsHPrecisionLow = value;
            }
        }

        private int _stepsPrecisionLargeH;
        public int StepsPrecisionLargeH
        {
            get { return _stepsPrecisionLargeH; }
            set
            {
                SetProperty(ref _stepsPrecisionLargeH, value);
                _settings.moveStepsHPrecisionHigh = value;
            }
        }

        private float _maxSpeedH;
        public float MaxSpeedH
        {
            get { return _maxSpeedH; }
            set
            {
                SetProperty(ref _maxSpeedH, value);
                _settings.moveMaxSpeedH = value;
            }
        }

        private float _accelH;
        public float AccelH
        {
            get { return _accelH; }
            set
            {
                SetProperty(ref _accelH, value);
                _settings.moveAccelH = value;
            }
        }

        private float _maxSpeedV;
        public float MaxSpeedV
        {
            get { return _maxSpeedV; }
            set { 
                SetProperty(ref _maxSpeedV, value);
                _settings.moveMaxSpeedV = value;
            }
        }

        private float _accelV;
        public float AccelV
        {
            get { return _accelV; }
            set
            {
                SetProperty(ref _accelV, value);
                _settings.moveAccelV = value;
            }
        }
    }
}
