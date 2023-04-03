using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    public class SettingsVertRecipViewModel : BindableBase
    {

        public SettingsVertRecipViewModel() 
        {
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
