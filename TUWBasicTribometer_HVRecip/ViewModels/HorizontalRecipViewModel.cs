using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    public class HorizontalRecipViewModel : BindableBase
    {
        private readonly TribometerController _controller;
        private readonly TribometerSettings _settings;

        private int adjustLoadMoveSteps = 1;

        Timer testTimer;
        DateTime startTime;

        public HorizontalRecipViewModel(IContainer container)
        {
            _controller = container.Resolve<TribometerController>();
            _settings = container.Resolve<TribometerSettings>();

            _controller.TestCycleCountUpdated += _controller_TestCycleCountUpdated;
            _controller.StateChanged += _controller_StateChanged;

            IsReciprocating = false;

            LoadingProfilesAvailable = new ObservableCollection<NormalLoadingProfile>() { NormalLoadingProfile.ManualControl, NormalLoadingProfile.LoadBeforeStart };

            SavePath = _settings.SaveFilePath;
            IsFixedNumberOfCycles = _settings.horizTestStopAtNumberOfCycles;
            IsManualEnd = !_settings.horizTestStopAtNumberOfCycles;
            TargetNumberOfCycles = _settings.horizTestTargetNumberOfCycles;
            SelectedLoadingProfile = _settings.horizTestNormalLoadingProfile;

            StartCommand = new DelegateCommand(StartTest, CanStartTest);
            EndCommand = new DelegateCommand(EndTest, CanEndTest);

            LoadCommand = new DelegateCommand(ApplyNormalLoad, CanChangeLoad);
            UnloadCommand = new DelegateCommand(RemoveNormalLoad, CanChangeLoad);
            IncreaseLoadCommand = new DelegateCommand(IncreaseNormalLoad, CanChangeLoad);
            DecreaseLoadCommand = new DelegateCommand(DecreaseNormalLoad, CanChangeLoad);


        }

        // Commands and CanExecutes

        private bool CanChangeLoad()
        {
            var state = _controller.State;
            return state == OperatingState.Idle || state == OperatingState.RecipHorizontal;
        }

        private bool CanEndTest()
        {
            return _controller.State == OperatingState.RecipHorizontal;
        }

        private bool CanStartTest()
        {
            return _controller.State == OperatingState.Idle;
        }

        private void ApplyNormalLoad()
        {
            if (_settings.stepPosVLoaded.HasValue)
                _controller.MoveTo(TribometerAxis.Vertical, _settings.stepPosVLoaded.Value);
        }

        private void RemoveNormalLoad()
        {
            if (_settings.stepPosVUnloaded.HasValue)
                _controller.MoveTo(TribometerAxis.Vertical, _settings.stepPosVUnloaded.Value);
        }

        private void IncreaseNormalLoad()
        {
            _controller.Move(TribometerAxis.Vertical, adjustLoadMoveSteps);
        }

        private void DecreaseNormalLoad()
        {
            _controller.Move(TribometerAxis.Vertical, -adjustLoadMoveSteps);
        }

        private void StartTest()
        {
            if (!(_settings.stepPosVLoaded.HasValue && _settings.stepPosVUnloaded.HasValue
                && _settings.stepPosHLeft.HasValue && _settings.stepPosHRight.HasValue))
            {
                MessageBox.Show("Define loaded/unloaded and left/right points first");
                return;
            }

            _controller.StartHorizRecipTest();
        }

        private void EndTest()
        {
            _controller.EndTest();
        }


        // Event Handling

        private void _controller_StateChanged(object sender, OperatingState e)
        {
            if (e == OperatingState.RecipHorizontal)
            {
                IsReciprocating = true;          
            } else
            {
                if (IsReciprocating)
                {
                    HandleEndOfTest();
                }
                IsReciprocating = false;
            }

            StartCommand?.RaiseCanExecuteChanged();
            EndCommand?.RaiseCanExecuteChanged();
            LoadCommand?.RaiseCanExecuteChanged();
            UnloadCommand?.RaiseCanExecuteChanged();
            IncreaseLoadCommand?.RaiseCanExecuteChanged();
            DecreaseLoadCommand?.RaiseCanExecuteChanged();
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
       

        // Bindings
        
        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand EndCommand { get; set; }
        public DelegateCommand DecreaseLoadCommand { get; set; }
        public DelegateCommand IncreaseLoadCommand { get; set; }
        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand UnloadCommand { get; set; }

        private bool _isReciprocating;
        public bool IsReciprocating
        {
            get => _isReciprocating;
            set => SetProperty(ref _isReciprocating, value);
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

        private NormalLoadingProfile _selectedLoadingProfile;
        public NormalLoadingProfile SelectedLoadingProfile
        {
            get { return _selectedLoadingProfile; }
            set { SetProperty(ref _selectedLoadingProfile, value); }
        }

        private ObservableCollection<NormalLoadingProfile> _loadingProfilesAvailable;
        public ObservableCollection<NormalLoadingProfile> LoadingProfilesAvailable
        {
            get { return _loadingProfilesAvailable; }
            set { SetProperty(ref _loadingProfilesAvailable, value); }
        }

        private bool _isManualEnd = true;
        public bool IsManualEnd
        {
            get { return _isManualEnd; }
            set { 
                SetProperty(ref _isManualEnd, value);
                _settings.horizTestStopAtNumberOfCycles = !value;
            }
        }

        private bool _isFixedNumberOfCycles;
        public bool IsFixedNumberOfCycles
        {
            get { return _isFixedNumberOfCycles; }
            set
            {
                SetProperty(ref _isFixedNumberOfCycles, value);
                _settings.horizTestStopAtNumberOfCycles = value;
            }
        }

        private int _targetNumberOfCycles;
        public int TargetNumberOfCycles
        {
            get { return _targetNumberOfCycles; }
            set
            {
                if (value < 1) value = 1;
                SetProperty(ref _targetNumberOfCycles, value);
                _settings.horizTestTargetNumberOfCycles = value;
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
