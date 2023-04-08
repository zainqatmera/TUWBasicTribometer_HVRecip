using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    internal class SettingsHorizRecipViewModel : ViewModelBase
    {
        TribometerSettings _settings;
        TribometerController _controller;

        public string Title => "Horizontal Test";

        public SettingsHorizRecipViewModel(IContainer container)
        {
            _settings = container.Resolve<TribometerSettings>();
            _controller = container.Resolve<TribometerController>();

            MaxSpeedH = _settings.horizTestMaxSpeedH;
            AccelH = _settings.horizTestAccelH;
            MaxSpeedV = _settings.horizTestMaxSpeedV;
            AccelV = _settings.horizTestAccelV;
            PauseTime = _settings.horizTestPauseTime;

            _controller.StateChanged += _controller_StateChanged;

            UploadSettingsImmediateCommand = new DelegateCommand(UploadSettingsImmediate, CanUploadSettingsImmediate);
        }

        private void _controller_StateChanged(object sender, OperatingState e)
        {
            UploadSettingsImmediateCommand.RaiseCanExecuteChanged();
        }

        private bool CanUploadSettingsImmediate()
        {
            return _controller.State == OperatingState.RecipHorizontal;
        }

        private void UploadSettingsImmediate()
        {
            if (_controller.State == OperatingState.RecipHorizontal)
            {
                _controller.SetMotorControlParamsVertTest();
                byte[] buffer = new byte[5];
                MemoryStream ms = new MemoryStream(buffer);
                BinaryWriter bw = new BinaryWriter(ms);

                bw.Write((byte)TestSettingsParameter.PauseTimeHorizontalEnd);
                bw.Write(PauseTime);
                _controller.SendCommand(MessageCode.SetTestSettingsParam, buffer);

            }
        }


        public DelegateCommand UploadSettingsImmediateCommand { get; set; }

        private float _maxSpeedH;
        public float MaxSpeedH
        {
            get { return _maxSpeedH; }
            set
            {
                SetProperty(ref _maxSpeedH, value);
                _settings.vertTestMaxSpeedH = value;
            }
        }

        private float _accelH;
        public float AccelH
        {
            get { return _accelH; }
            set
            {
                SetProperty(ref _accelH, value);
                _settings.vertTestAccelH = value;
            }
        }

        private float _maxSpeedV;
        public float MaxSpeedV
        {
            get { return _maxSpeedV; }
            set
            {
                SetProperty(ref _maxSpeedV, value);
                _settings.vertTestMaxSpeedV = value;
            }
        }

        private float _accelV;
        public float AccelV
        {
            get { return _accelV; }
            set
            {
                SetProperty(ref _accelV, value);
                _settings.vertTestAccelV = value;
            }
        }

        private int _pauseTime;
        public int PauseTime
        {
            get { return _pauseTime; }
            set { SetProperty(ref _pauseTime, value); }
        }
    }
}
