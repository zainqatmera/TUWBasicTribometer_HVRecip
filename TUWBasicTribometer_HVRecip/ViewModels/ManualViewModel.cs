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
        private readonly TribometerSettings _settings;

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
                        moveSteps = sgn * 1;
                    else
                        moveSteps = sgn * 1;
                    _controller.Move(TribometerAxis.Horizontal, moveSteps);
                    break;
                case 'U': 
                case 'D':
                    if (amountCode == '1')
                        moveSteps = sgn * 1;
                    else
                        moveSteps = sgn * 1;
                    _controller.Move(TribometerAxis.Vertical, moveSteps);
                    break;
            }

        }

        private void MoveAxisTo(string obj)
        {
            switch (obj)
            {
                case "HC":
                    _controller.MoveTo(TribometerAxis.Horizontal, _settings.stepPosHCentre); 
                    break;
/*                case "H":
                    _controller.MoveTo(TribometerAxis.Horizontal, GotoTextHorizontal);
                    break;
                case "V":
                    _controller.MoveTo(TribometerAxis.Vertical, GotoPositionVertical);
                    break;
*/
            }
        }

        // Bindings

        public DelegateCommand<string> MoveCommand { get; set; }
        public DelegateCommand<string> MoveToCommand { get; set; }
        public DelegateCommand<string> SetMark { get; set; }


        // Inputs 

        private double gotoPositionHorizontal = 0;
        public double GotoPositionHorizontal
        {
            get => gotoPositionHorizontal;
            set => SetProperty(ref gotoPositionHorizontal, value);
        }
               
        private double _gotoPositionVertical = 0;
        public double GotoPositionVertical
        {
            get => _gotoPositionVertical;
            set => SetProperty(ref _gotoPositionVertical, value);
        }

        private bool _isVerticalPrecisionMode;
        public bool IsVerticalPrecisionMode
        {
            get { return _isVerticalPrecisionMode; }
            set { SetProperty(ref _isVerticalPrecisionMode, value); }
        }
                
        private bool _isHorizontalPrecisionMode;
        public bool IsHorizontalPrecisionMode
        {
            get { return _isHorizontalPrecisionMode; }
            set { SetProperty(ref _isHorizontalPrecisionMode, value); }
        }


        // Outputs

        private int _stepHorizontal;
        public int StepHorizontal
        {
            get { return _stepHorizontal; }
            set { SetProperty(ref _stepHorizontal, value); }
        }

        private int _stepVertical;
        public int StepVertical
        {
            get { return _stepVertical; }
            set { SetProperty(ref _stepVertical, value); }
        }

        private string _posHorizontal_mm;
        public string PosHorizontal_mm
        {
            get { return _posHorizontal_mm; }
            set { SetProperty(ref _posHorizontal_mm, value); }
        }

        private string _posVertical_mm;
        public string PosVertical_mm
        {
            get { return _posVertical_mm; }
            set { SetProperty(ref _posVertical_mm, value); }
        }

        private bool _useStepForGotoHorizontal;
        public bool StepForGotoHorizontal
        {
            get { return _useStepForGotoHorizontal; }
            set { SetProperty(ref _useStepForGotoHorizontal, value); }
        }

        private bool _useStepForGotoVertical;
        public bool StepForGotoVertical
        {
            get { return _useStepForGotoVertical;}
            set { SetProperty(ref _useStepForGotoVertical, value); }
        }

        private int _stepHCentre;
        public int StepHCentre
        {
            get { return _stepHCentre; }
            set { SetProperty(ref _stepHCentre, value); }
        }

        private int _stepHLeft;
        public int StepHLeft
        {
            get { return _stepHLeft; }
            set { SetProperty(ref _stepHLeft, value); }
        }

        private int _stepHRight;
        public int StepHRight
        {
            get { return _stepHRight; }
            set { SetProperty(ref _stepHRight, value); }
        }

        private int _stepVRaise;
        public int StepVRaise
        {
            get { return _stepVRaise; }
            set { SetProperty(ref _stepVRaise, value); }
        }

        private int _stepVLower;
        public int StepVLower
        {
            get { return _stepVLower; }
            set { SetProperty(ref _stepVLower, value); }
        }

        private int _stepVLoaded;
        public int StepVLoaded
        {
            get { return _stepVLoaded; }
            set { SetProperty(ref _stepVLoaded, value); }
        }





    }
}
