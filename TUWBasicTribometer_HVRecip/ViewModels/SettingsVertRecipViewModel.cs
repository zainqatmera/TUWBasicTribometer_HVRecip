﻿using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    public class SettingsVertRecipViewModel : ViewModelBase
    {
        TribometerSettings _settings;
        TribometerController _controller;

        public string Title => "Vertical Test";

        public SettingsVertRecipViewModel(IContainer container) 
        {
            _settings = container.Resolve<TribometerSettings>();
            _controller = container.Resolve<TribometerController>();

            MaxSpeedH = _settings.vertTestMaxSpeedH;
            AccelH = _settings.vertTestAccelH;
            MaxSpeedV = _settings.vertTestMaxSpeedV;
            AccelV = _settings.vertTestAccelV;
            PauseTimeUnloaded = _settings.vertTestPauseTimeUnloaded;
            PauseTimeLoaded = _settings.vertTestPauseTimeLoaded;

            _controller.StateChanged += _controller_StateChanged;

            UploadSettingsImmediateCommand = new DelegateCommand(UploadSettingsImmediate, CanUploadSettingsImmediate);
        }

        private void _controller_StateChanged(object sender, OperatingState e)
        {
            UploadSettingsImmediateCommand.RaiseCanExecuteChanged();
        }

        private bool CanUploadSettingsImmediate()
        {
            return _controller.State == OperatingState.RecipVertical;
        }

        private void UploadSettingsImmediate()
        {
            if (_controller.State == OperatingState.RecipVertical)
            {
                _controller.SetMotorControlParamsVertTest();
                byte[] buffer = new byte[5];
                MemoryStream ms = new MemoryStream(buffer);
                BinaryWriter bw = new BinaryWriter(ms);

                bw.Write((byte)TestSettingsParameter.PauseTimeUnloaded);
                bw.Write(PauseTimeUnloaded);
                _controller.SendCommand(MessageCode.SetTestSettingsParam, buffer);

                ms.Position = 0;
                bw.Write((byte)TestSettingsParameter.PauseTimeLoaded);
                bw.Write(PauseTimeLoaded);
                _controller.SendCommand(MessageCode.SetTestSettingsParam, buffer);

            }
        }

        // Bindings

        public DelegateCommand UploadSettingsImmediateCommand { get; set; }

        private float _maxSpeedH;
        public float MaxSpeedH
        {
            get { return _maxSpeedH; }
            set {
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

        private int _pauseTimeUnloaded;
        public int PauseTimeUnloaded
        {
            get { return _pauseTimeUnloaded; }
            set { 
                SetProperty(ref _pauseTimeUnloaded, value); 
                _settings.vertTestPauseTimeUnloaded = value;
            }
        }

        private int _pauseTimeLoaded;
        public int PauseTimeLoaded
        {
            get { return _pauseTimeLoaded; }
            set
            {
                SetProperty(ref _pauseTimeLoaded, value);
                _settings.vertTestPauseTimeLoaded = value;
            }
        }

    }
}
