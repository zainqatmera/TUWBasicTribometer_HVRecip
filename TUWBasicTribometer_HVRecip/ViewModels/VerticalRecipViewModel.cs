using DryIoc;
using NationalInstruments.DAQmx;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TUWBasicTribometer_HVRecip.Controllers;
using TUWBasicTribometer_HVRecip.Properties;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    public class VerticalRecipViewModel : BindableBase
    {
        private readonly TribometerController _controller;
        private readonly TribometerSettings _settings;

        Timer testTimer;
        DateTime startTime;

        public VerticalRecipViewModel(IContainer container)
        {
            _controller = container.Resolve<TribometerController>();
            _settings = container.Resolve<TribometerSettings>();

            _controller.TestCycleCountUpdated += _controller_TestCycleCountUpdated;

            StartCommand = new DelegateCommand(StartTest);
            EndCommand = new DelegateCommand(EndTest);
        }

        private void _controller_TestCycleCountUpdated(object sender, int e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (e < 0) { NumberOfCyclesCompleted = 0; }
                else { NumberOfCyclesCompleted = e; }
            }));

            if (e == 0)
            {
                StartTestTimer(); 
            }
        }

        private void StartTestTimer()
        {
            startTime = DateTime.Now;
            testTimer = new Timer(TestTimerTick);
            testTimer.Change(0, 1000);
        }

        private void TestTimerTick(object state)
        {
            TimeSpan timeSpan = DateTime.Now - startTime;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (timeSpan.TotalMinutes > 60)
                    TimeElapsed = timeSpan.ToString("hh':'mm':'ss");
                else
                    TimeElapsed = timeSpan.ToString("mm':'ss");
            }));
        }

        private void StartTest()
        {
            if (!(_settings.stepPosVLoaded.HasValue && _settings.stepPosVUnloaded.HasValue))
            {
                MessageBox.Show("Define upper and lower points first");
                return;
            }

            
            _controller.StartVertRecipTest();
        }

        private void EndTest()
        {
            _controller.SendCommand(MessageCode.EndTest);
        }

        private bool _isReciprocating;
        public bool IsReciprocating
        {
            get => _isReciprocating; 
            set => SetProperty(ref _isReciprocating, value);
        }


        public VerticalRecipViewModel()
        {
            IsReciprocating = false;
           
            MoveToRaisedCommand = new DelegateCommand(MoveToRaised).ObservesCanExecute(() => !IsReciprocating && _settings.stepPosVRaised.HasValue);
            MoveToUnloadedCommand = new DelegateCommand(MoveToUnloaded).ObservesCanExecute(() => !IsReciprocating && _settings.stepPosVUnloaded.HasValue);
            MoveToLoadedCommand = new DelegateCommand(MoveToLoaded).ObservesCanExecute(() => !IsReciprocating && _settings.stepPosVLoaded.HasValue);
            StartCommand = new DelegateCommand(StartRecip).ObservesCanExecute(() => !IsReciprocating 
                                                    && _settings.stepPosVUnloaded.HasValue 
                                                    && _settings.stepPosVLoaded.HasValue
                                                    && _controller.State == OperatingState.Idle);
            EndCommand = new DelegateCommand(EndRecip).ObservesCanExecute(() => IsReciprocating);
        }

        private void EndRecip()
        {
            IsReciprocating = false;           
        }

        private void StartRecip()
        {
            IsReciprocating = true;
        }

        private void MoveToRaised()
        {
            _controller.MoveTo(TribometerAxis.Vertical, _settings.stepPosVRaised.Value);
        }

        private void MoveToUnloaded()
        {
            _controller.MoveTo(TribometerAxis.Vertical, _settings.stepPosVUnloaded.Value);
        }

        private void MoveToLoaded()
        {
            _controller.MoveTo(TribometerAxis.Vertical, _settings.stepPosVLoaded.Value);
        }


        public DelegateCommand MoveToRaisedCommand { get; set; }
        public DelegateCommand MoveToUnloadedCommand { get; set; }
        public DelegateCommand MoveToLoadedCommand { get; set; }
        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand EndCommand { get; set; }

        private int _pauseTimeUnloaded;
        public int PauseTimeUnloaded
        {
            get => _pauseTimeUnloaded;
            set
            {
                SetProperty(ref _pauseTimeUnloaded, value);
                _settings.vertTestPauseTimeUnloaded = value;
            }
        }

        private int _pauseTimeLoaded;
        public int PauseTimeLoaded { 
            get => _pauseTimeLoaded;
            set
            {
                SetProperty(ref _pauseTimeLoaded, value);
                _settings.vertTestPauseTimeLoaded = value;
            }
        }

        private bool _isFixedNumberOfCycles;
        public bool IsFixedNumberOfCycles
        {
            get { return _isFixedNumberOfCycles; }
            set { 
                SetProperty(ref _isFixedNumberOfCycles, value); 
                _settings.vertTestNumberOfCycles = value ? TargetNumberOfCycles : -1;
            }
        }

        private int _targetNumberOfCycles;
        public int TargetNumberOfCycles
        {
            get { return _targetNumberOfCycles; }
            set { 
                SetProperty(ref _targetNumberOfCycles, value);
                _settings.vertTestNumberOfCycles = IsFixedNumberOfCycles ? value : -1;
            }
        }

        private int _numberOfCyclesCompleted;
        public int NumberOfCyclesCompleted
        {
            get { return _numberOfCyclesCompleted; }
            set { SetProperty(ref _numberOfCyclesCompleted, value); }
        }

        private string _timeElapsed;
        public string TimeElapsed
        {
            get { return _timeElapsed; }
            set { SetProperty(ref _timeElapsed, value); }
        }

        private string _normalForce;
        public string NormalForce
        {
            get { return _normalForce; }
            set { SetProperty(ref _normalForce, value); }
        }
    }
}
