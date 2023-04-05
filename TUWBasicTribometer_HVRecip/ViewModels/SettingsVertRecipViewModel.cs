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
    public class SettingsVertRecipViewModel : BindableBase
    {
        TribometerSettings _settings;

        public string Title => "Vertical Recip";

        public SettingsVertRecipViewModel(IContainer container) 
        {
            _settings = container.Resolve<TribometerSettings>();

            UploadSettingsImmediateCommand = new DelegateCommand(UploadSettingsImmediate);
        }

        private void UploadSettingsImmediate()
        {
        }

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

    }
}
