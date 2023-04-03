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
    internal class SetupViewModel : BindableBase
    {
        private readonly TribometerController _controller;

        public SetupViewModel(IContainer container)
        {
            _controller = container.Resolve<TribometerController>();

            HomeCommand = new DelegateCommand(InitiateHoming);
            ConnectTribometerCommand = new DelegateCommand(ConnectTribometer);
            ConnectSensorCommand = new DelegateCommand(ConnectSensor);
            ZeroSensorCommand = new DelegateCommand(ZeroSensor);
            ReloadSettingsCommand = new DelegateCommand(ReloadSettings);
        }

        private void ReloadSettings()
        {
        }

        private void ZeroSensor()
        {
        }

        private void ConnectSensor()
        {
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

    }
}
