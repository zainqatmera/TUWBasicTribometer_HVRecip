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
    internal class ManualViewModel : BindableBase
    {
        private readonly TribometerController _controller;


        public ManualViewModel(IContainer container)
        {
            _controller = container.Resolve<TribometerController>();

            MoveCommand = new DelegateCommand<string>(MoveAxis);
            MoveToCommand = new DelegateCommand<string>(MoveAxisTo);
        }

        // Methods

        private void MoveAxis(string obj)
        {
            char direction = obj[0];
            char amountCode =  obj[1];
           
            int moveSteps = 0;
            int sgn = 1;  // Positive or negative move direction 

            if (direction == 'L' || direction == 'U')
            {
                sgn = -1;
            }

            switch (direction)
            {
                case 'L': 
                case 'R':
                    if (amountCode == '1')
                        moveSteps = sgn * StepsPerMoveHLow;
                    else
                        moveSteps = sgn * StepsPerMoveHHigh;
                    _controller.Move(TribometerAxis.Horizontal, moveSteps);
                    break;
                case 'U': 
                case 'D':
                    if (amountCode == '1')
                        moveSteps = sgn * StepsPerMoveVLow;
                    else
                        moveSteps = sgn * StepsPerMoveVHigh;
                    _controller.Move(TribometerAxis.Vertical, moveSteps);
                    break;
            }

        }

        private void MoveAxisTo(string obj)
        {
            switch (obj)
            {
                case "HC":
                    _controller.MoveTo(TribometerAxis.Horizontal, TribometerSettings.Instance.stepsOffsetToHCentre); 
                    break;
                case "H":
                    _controller.MoveTo(TribometerAxis.Horizontal, SetGotoHorizontal);
                    break;
                case "V":
                    _controller.MoveTo(TribometerAxis.Vertical, SetGotoVertical);
                    break;

            }
        }

        // Bindings

        public DelegateCommand<string> MoveCommand { get; set; }
        public DelegateCommand<string> MoveToCommand { get; set; }

        private int _stepsPerMoveHLow = 10;
        public int StepsPerMoveHLow
        {
            get => _stepsPerMoveHLow;
            set => SetProperty(ref _stepsPerMoveHLow, value);
        }

        private int _stepsPerMoveHHigh = 100;
        public int StepsPerMoveHHigh
        {
            get => _stepsPerMoveHHigh;
            set => SetProperty(ref _stepsPerMoveHHigh, value);
        }

        private int _stepsPerMoveVLow = 10;
        public int StepsPerMoveVLow
        {
            get => _stepsPerMoveVLow;
            set => SetProperty(ref _stepsPerMoveVLow, value);
        }

        private int _stepsPerMoveVHigh = 100;
        public int StepsPerMoveVHigh
        {
            get => _stepsPerMoveVHigh;
            set => SetProperty(ref _stepsPerMoveVHigh, value);
        }

        private int _setGotoHorizontal = 0;
        public int SetGotoHorizontal
        {
            get => _setGotoHorizontal;
            set => SetProperty(ref _setGotoHorizontal, value);
        }
               
        private int _setGotoVertical = 0;
        public int SetGotoVertical
        {
            get => _setGotoVertical;
            set => SetProperty(ref _setGotoVertical, value);
        }

    }
}
