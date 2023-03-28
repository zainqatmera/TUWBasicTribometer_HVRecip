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
            ConnectCommand = new DelegateCommand(ConnectTribometer);
            
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
        public DelegateCommand ConnectCommand { get; private set; }

    }
}
