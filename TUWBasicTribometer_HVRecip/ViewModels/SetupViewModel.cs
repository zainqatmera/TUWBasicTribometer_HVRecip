using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    internal class SetupViewModel : BindableBase
    {
        private readonly TribometerController _controller;
        private readonly ForceSensor _forceSensor;
        public SetupViewModel(IContainer container)
        {
            _controller = container.Resolve<TribometerController>();
            _forceSensor = container.Resolve<ForceSensor>();

            HomeCommand = new DelegateCommand(InitiateHoming);
            ConnectTribometerCommand = new DelegateCommand(ConnectTribometer);
            ConnectSensorCommand = new DelegateCommand(ConnectSensor);
            ZeroSensorCommand = new DelegateCommand(ZeroSensor);
            ReloadSettingsCommand = new DelegateCommand(ReloadSettings);
            ClearErrorsCommand = new DelegateCommand(ClearErrors);
        }

        private void ClearErrors()
        {
            _controller.SendCommand(MessageCode.ClearError);
        }

        private void ReloadSettings()
        {
        }

        private void ZeroSensor()
        {
            _forceSensor.SetZeroNow();
        }

        private void ConnectSensor()
        {            
            var success = _forceSensor.StartSystem();
            if (!success)
            {
                MessageBox.Show(_forceSensor.ErrorMessage);
            }
        }

        private void InitiateHoming()
        {
            _controller.HomeTribometer();
        }

        private void ConnectTribometer()
        {
            _controller.Connect();
        }

        // Bindings

        public DelegateCommand HomeCommand { get; private set; }
        public DelegateCommand ReloadSettingsCommand { get; private set; }
        public DelegateCommand ConnectTribometerCommand { get; private set; }
        public DelegateCommand ConnectSensorCommand { get; private set; }
        public DelegateCommand ZeroSensorCommand { get; private set; }
        public DelegateCommand ClearErrorsCommand { get; private set; }

    }
}
