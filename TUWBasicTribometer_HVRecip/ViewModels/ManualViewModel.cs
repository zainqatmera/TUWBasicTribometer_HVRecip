using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    internal class ManualViewModel : ViewModelBase
    {
        private readonly TribometerController _controller;
        private readonly TribometerSettings _settings;

        public ManualViewModel(IContainer container)
        {
            _controller = container.Resolve<TribometerController>();
            _settings = container.Resolve<TribometerSettings>();

            MoveCommand = new DelegateCommand<string>(MoveAxis);
            MoveToCommand = new DelegateCommand<string>(MoveAxisTo);
            SetMarkCommand = new DelegateCommand<string>(SetMark);

            _settings.SettingsChanged += Settings_SettingsChanged;
            _controller.PositionUpdated += Controller_PositionUpdated;

            if (_controller.State != OperatingState.NotConnected)
            {
                _controller.SendCommand(MessageCode.RequestStatus);
            }
            
            Settings_SettingsChanged(this, EventArgs.Empty);
        }

        private void Controller_PositionUpdated(object sender, TribometerPositionEventArgs e)
        {
            StepHorizontal = e.HorizontalStepPosition ?? int.MaxValue;
            StepVertical = e.VerticalStepPosition ?? int.MaxValue;
        }

        private void Settings_SettingsChanged(object sender, EventArgs e)
        {
            StepHCentre = _settings.stepPosHCentre?.ToString() ?? "-";
            StepHLeft = _settings.stepPosHLeft?.ToString() ?? "-";
            StepHRight = _settings.stepPosHRight?.ToString() ?? "-";
            StepVRaise = _settings.stepPosVRaised?.ToString() ?? "-";
            StepVUnloaded = _settings.stepPosVUnloaded?.ToString() ?? "-";
            StepVLoaded = _settings.stepPosVLoaded?.ToString() ?? "-";
        }

        private void SetMark(string obj)
        {
            switch (obj)
            {
                case "HCentre":
                    _settings.stepPosHCentre = _controller.PositionH;
                    break;
                case "HLeft":
                    _settings.stepPosHLeft = _controller.PositionH;
                    break;
                case "HRight":
                    _settings.stepPosHRight = _controller.PositionH;
                    break;
                case "VRaised":
                    _settings.stepPosVRaised = _controller.PositionV;
                    break;
                case "VUnloaded":
                    _settings.stepPosVUnloaded = _controller.PositionV;
                    break;
                case "VLoaded":
                    _settings.stepPosVLoaded = _controller.PositionV;
                    break;
            }
            _settings.NotifySettingsChanged();
        }

        // Methods

        private void MoveAxis(string obj)
        {
            char axis = obj[0];
            int amountCode = Convert.ToInt32(obj.Substring(1));
            int stepsToMove = 0;
           
            if (axis == 'H')
            {
                if (IsHorizontalPrecisionMode)
                {
                    switch (amountCode)
                    {
                        case -2: stepsToMove = -_settings.moveStepsHPrecisionHigh; break;
                        case -1: stepsToMove = -_settings.moveStepsHPrecisionLow; break;
                        case 1: stepsToMove = _settings.moveStepsHPrecisionLow; break;
                        case 2: stepsToMove = _settings.moveStepsHPrecisionHigh; break;
                    }
                }
                else
                {
                    switch (amountCode)
                    {
                        case -2: stepsToMove = -_settings.moveStepsHNormalHigh; break;
                        case -1: stepsToMove = -_settings.moveStepsHNormalLow; break;
                        case 1: stepsToMove = _settings.moveStepsHNormalLow; break;
                        case 2: stepsToMove = _settings.moveStepsHNormalHigh; break;
                    }
                }
                _controller.Move(TribometerAxis.Horizontal, stepsToMove);
            }
            else
            {
                if (IsVerticalPrecisionMode)
                {
                    switch (amountCode)
                    {
                        case -2: stepsToMove = -_settings.moveStepsVPrecisionHigh; break;
                        case -1: stepsToMove = -_settings.moveStepsVPrecisionLow; break;
                        case 1: stepsToMove = _settings.moveStepsVPrecisionLow; break;
                        case 2: stepsToMove = _settings.moveStepsVPrecisionHigh; break;
                    }
                }
                else
                {
                    switch (amountCode)
                    {
                        case -2: stepsToMove = -_settings.moveStepsVNormalHigh; break;
                        case -1: stepsToMove = -_settings.moveStepsVNormalLow; break;
                        case 1: stepsToMove = _settings.moveStepsVNormalLow; break;
                        case 2: stepsToMove = _settings.moveStepsVNormalHigh; break;
                    }
                }
                _controller.Move(TribometerAxis.Vertical, stepsToMove);
            }

        }

        private void MoveAxisTo(string obj)
        {
            switch (obj)
            {
                case "HCentre":
                    if (_settings.stepPosHCentre.HasValue)
                    {
                        _controller.MoveTo(TribometerAxis.Horizontal, _settings.stepPosHCentre.Value);
                    }
                    break;
                case "HLeft":
                    if (_settings.stepPosHLeft.HasValue)
                    {
                        _controller.MoveTo(TribometerAxis.Horizontal, _settings.stepPosHLeft.Value);
                    }
                    break;
                case "HRight":
                    if (_settings.stepPosHLeft.HasValue)
                    {
                        _controller.MoveTo(TribometerAxis.Horizontal, _settings.stepPosHRight.Value);
                    }
                    break;
                case "VRaise":
                    if (_settings.stepPosVRaised.HasValue)
                    {
                        _controller.MoveTo(TribometerAxis.Vertical, _settings.stepPosVRaised.Value);
                    }
                    break;
                case "VLower":
                    if (_settings.stepPosVRaised.HasValue)
                    {
                        _controller.MoveTo(TribometerAxis.Vertical, _settings.stepPosVUnloaded.Value);
                    }
                    break;
                case "VLoad":
                    if (_settings.stepPosVRaised.HasValue)
                    {
                        _controller.MoveTo(TribometerAxis.Vertical, _settings.stepPosVLoaded.Value);
                    }
                    break;
                case "HGoto":
                    {
                        _controller.MoveTo(TribometerAxis.Horizontal, (int)GotoPositionHorizontal);
                    }
                    break;
                case "VGoto":
                    {
                        _controller.MoveTo(TribometerAxis.Vertical, (int)GotoPositionVertical);
                    }
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
        public DelegateCommand<string> SetMarkCommand { get; set; }


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

        private bool _isVerticalNormalMode = true;
        public bool IsVerticalNormalMode
        {
            get { return _isVerticalNormalMode; }
            set { SetProperty(ref _isVerticalNormalMode, value); }
        }
                
        private bool _isHorizontalNormalMode = true;
        public bool IsHorizontalNormalMode
        {
            get { return _isHorizontalNormalMode; }
            set { SetProperty(ref _isHorizontalNormalMode, value); }
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

        private bool _useStepForGotoHorizontal = true;   // Goto value entry is step number (otherwise mm)
        public bool StepForGotoHorizontal
        {
            get { return _useStepForGotoHorizontal; }
            set { SetProperty(ref _useStepForGotoHorizontal, value); }
        }

        private bool _useStepForGotoVertical = true;   // Goto value entry is step number (otherwise mm)
        public bool StepForGotoVertical
        {
            get { return _useStepForGotoVertical;}
            set { SetProperty(ref _useStepForGotoVertical, value); }
        }

        private string _stepHCentre;
        public string StepHCentre
        {
            get { return _stepHCentre; }
            set { SetProperty(ref _stepHCentre, value); }
        }

        private string _stepHLeft;
        public string StepHLeft
        {
            get { return _stepHLeft; }
            set { SetProperty(ref _stepHLeft, value); }
        }

        private string _stepHRight;
        public string StepHRight
        {
            get { return _stepHRight; }
            set { SetProperty(ref _stepHRight, value); }
        }

        private string _stepVRaise;
        public string StepVRaise
        {
            get { return _stepVRaise; }
            set { SetProperty(ref _stepVRaise, value); }
        }

        private string _stepVUnloaded;
        public string StepVUnloaded
        {
            get { return _stepVUnloaded; }
            set { SetProperty(ref _stepVUnloaded, value); }
        }

        private string _stepVLoaded;
        public string StepVLoaded
        {
            get { return _stepVLoaded; }
            set { SetProperty(ref _stepVLoaded, value); }
        }





    }
}
