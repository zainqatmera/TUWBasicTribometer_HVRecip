using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly IContainer container;
        private readonly IRegionManager regionManager;
        private readonly TribometerController tribometerController;

        public MainWindowViewModel(IContainer container, IRegionManager regionManager )
        {
            this.container = container;
            this.regionManager = regionManager;

            tribometerController = container.Resolve<TribometerController>();

            SetupCommand = new DelegateCommand(NavigateToSetup);
            ManualCommand = new DelegateCommand(NavigateToManual);
            VertRecipCommand = new DelegateCommand(NavigateToVert);
            HorizRecipCommand = new DelegateCommand(NavigateToHoriz);
            SettingsCommand = new DelegateCommand(NavigateToSettings);
            StopCommand = new DelegateCommand(StopTribometer);
            RaiseCommand = new DelegateCommand(RaiseTribometer);
        }

        private void NavigateToSetup()
        {
            SetSelection("Setup");
            regionManager.RequestNavigate(PrismRegion.MainContentRegion, PrismNavigationUri.Setup);
        }


        private void NavigateToManual()
        {
            SetSelection("Manual");
            regionManager.RequestNavigate(PrismRegion.MainContentRegion, PrismNavigationUri.Manual);
        }

        private void NavigateToVert()
        {
            SetSelection("Vert");
            regionManager.RequestNavigate(PrismRegion.MainContentRegion, PrismNavigationUri.VertRecip);
        }

        private void NavigateToHoriz()
        {
            SetSelection("Horiz");
            regionManager.RequestNavigate(PrismRegion.MainContentRegion, PrismNavigationUri.HorizRecip);
        }

        private void NavigateToSettings()
        {
            SetSelection("Settings");
            regionManager.RequestNavigate(PrismRegion.MainContentRegion, PrismNavigationUri.Settings);
        }

        private void StopTribometer()
        {
            tribometerController.Stop();
        }

        private void RaiseTribometer()
        {
            tribometerController.EmergencyRaise();
        }

        private void SetSelection(string selectedPage)
        {
            SetupWeight = selectedPage == "Setup" ? FontWeights.Bold : FontWeights.Normal;
            ManualWeight = selectedPage == "Manual" ? FontWeights.Bold : FontWeights.Normal;
            VertWeight = selectedPage == "Vert" ? FontWeights.Bold : FontWeights.Normal;
            HorizWeight = selectedPage == "Horiz" ? FontWeights.Bold : FontWeights.Normal;
            SettingsWeight = selectedPage == "Settings" ? FontWeights.Bold : FontWeights.Normal;
        }



        // Bindings
        public DelegateCommand SetupCommand { get; private set; }
        public DelegateCommand ManualCommand { get; private set; }
        public DelegateCommand VertRecipCommand { get; private set; }
        public DelegateCommand HorizRecipCommand { get; private set; }
        public DelegateCommand SettingsCommand { get; private set; }
        public DelegateCommand StopCommand { get; private set; }
        public DelegateCommand RaiseCommand { get; private set; }

        private FontWeight _setupWeight = FontWeights.Normal;
        public FontWeight SetupWeight
        {
            get { return _setupWeight; }
            set { SetProperty(ref _setupWeight, value); }
        }

        private FontWeight _manualWeight = FontWeights.Normal;
        public FontWeight ManualWeight
        {
            get { return _manualWeight; }
            set { SetProperty(ref _manualWeight, value); }
        }

        private FontWeight _vertWeight = FontWeights.Normal;
        public FontWeight VertWeight
        {
            get { return _vertWeight; }
            set { SetProperty(ref _vertWeight, value); }
        }

        private FontWeight _horizWeight = FontWeights.Normal;
        public FontWeight HorizWeight
        {
            get { return _horizWeight; }
            set { SetProperty(ref _horizWeight, value); }
        }

        private FontWeight _settingsWeight = FontWeights.Normal;
        public FontWeight SettingsWeight
        {
            get { return _settingsWeight; }
            set { SetProperty(ref _settingsWeight, value); }
        }


    }
}
