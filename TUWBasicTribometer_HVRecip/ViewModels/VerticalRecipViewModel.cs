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
    public class VerticalRecipViewModel : ViewModelBase
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
            _controller.StateChanged += _controller_StateChanged;
            _controller.TestStartedNotification += _controller_TestStartedNotification;

            SavePath = _settings.SaveFilePathVertTests;
            IsFixedNumberOfCycles = _settings.vertTestStopAtNumberOfCycles;
            IsManualEnd = !IsFixedNumberOfCycles;
            TargetNumberOfCycles = _settings.vertTestTargetNumberOfCycles;

            IsInTest = false;

            StartCommand = new DelegateCommand(StartTest, CanStartTest);
            EndCommand = new DelegateCommand(EndTest, CanEndTest);
        }

        // Commands and CanExecutes

        private bool CanStartTest()
        {
            return _controller.State == OperatingState.Idle;
        }

        private bool CanEndTest()
        {
            return _controller.State == OperatingState.RecipVertical;
        }

        private void StartTest()
        {
            if (!(_settings.stepPosVLoaded.HasValue && _settings.stepPosVUnloaded.HasValue))
            {
                MessageBox.Show("Define loaded and unloaded points first");
                return;
            }

            _controller.StartVertRecipTest(TestName);
        }

        private void EndTest()
        {
            _controller.EndTest();
        }


        // Event Handling

        private void _controller_StateChanged(object sender, OperatingState e)
        {
            if (e == OperatingState.RecipVertical)
            {
                IsInTest = true;
            }
            else
            {
                if (IsInTest)
                {
                    HandleEndOfTest();
                }
                IsInTest = false;
            }

            StartCommand?.RaiseCanExecuteChanged();
            EndCommand?.RaiseCanExecuteChanged();
        }

        private void HandleEndOfTest()
        {
            testTimer.Dispose();
            MessageBox.Show("End of test");            
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
                // Start Test Time at first mark
                startTime = DateTime.Now;
                testTimer = new Timer(TestTimerTick);
                testTimer.Change(0, 1000);
            }
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

        private void _controller_TestStartedNotification(object sender, TribometerAxis e)
        {
            if (e == TribometerAxis.Vertical)
            {
                startTime = DateTime.Now;
                testTimer = new Timer(TestTimerTick);
                testTimer.Change(0, 1000);
            }
        }

        // Bindings

        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand EndCommand { get; set; }

        private bool _isInTest;
        public bool IsInTest
        {
            get => _isInTest; 
            set => SetProperty(ref _isInTest, value);
        }

        private string _testName;
        public string TestName
        {
            get { return _testName; }
            set { SetProperty(ref _testName, value); }
        }

        private string _savePath;
        public string SavePath
        {
            get { return _savePath; }
            set { SetProperty(ref _savePath, value); }
        }

        private bool _isManualEnd = true;
        public bool IsManualEnd
        {
            get { return _isManualEnd; }
            set
            {
                SetProperty(ref _isManualEnd, value);
                _settings.vertTestStopAtNumberOfCycles = !value;
            }
        }

        private bool _isFixedNumberOfCycles;
        public bool IsFixedNumberOfCycles
        {
            get { return _isFixedNumberOfCycles; }
            set { 
                SetProperty(ref _isFixedNumberOfCycles, value);
                _settings.vertTestStopAtNumberOfCycles = value;
            }
        }

        private int _targetNumberOfCycles;
        public int TargetNumberOfCycles
        {
            get { return _targetNumberOfCycles; }
            set {
                if (value < 1) value = 1;
                SetProperty(ref _targetNumberOfCycles, value);
                _settings.vertTestTargetNumberOfCycles = value;
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
