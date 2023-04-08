using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TUWBasicTribometer_HVRecip.Controllers;
using TUWBasicTribometer_HVRecip.ViewModels;
using TUWBasicTribometer_HVRecip.Views;

namespace TUWBasicTribometer_HVRecip
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            ViewModelLocationProvider.Register<SerialMonitorWindow, SerialMonitorWindowViewModel>();

            containerRegistry.RegisterForNavigation<InfoLogView, InfoLogViewModel>(PrismNavigationUri.Log);
            containerRegistry.RegisterForNavigation<StatusView, StatusViewModel>(PrismNavigationUri.Status);

            containerRegistry.RegisterForNavigation<SetupView, SetupViewModel>(PrismNavigationUri.Setup);
            containerRegistry.RegisterForNavigation<ManualView, ManualViewModel>(PrismNavigationUri.Manual);
            containerRegistry.RegisterForNavigation<HorizontalRecipView, HorizontalRecipViewModel>(PrismNavigationUri.HorizRecip);
            containerRegistry.RegisterForNavigation<VerticalRecipView, VerticalRecipViewModel>(PrismNavigationUri.VertRecip);
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>(PrismNavigationUri.Settings);

            containerRegistry.RegisterForNavigation<SettingManualView, SettingManualViewModel>(PrismNavigationUri.ManualSettings);
            containerRegistry.RegisterForNavigation<SettingsVertRecipView, SettingsVertRecipViewModel>(PrismNavigationUri.VertRecipSettings);
            containerRegistry.RegisterForNavigation<SettingsHorizRecipView, SettingsHorizRecipViewModel>(PrismNavigationUri.HorizRecipSettings);

          //  containerRegistry.RegisterSingleton<SerialPortManager>();
            containerRegistry.RegisterSingleton<TribometerController>();
            containerRegistry.RegisterSingleton<TribometerSettings>();
            containerRegistry.RegisterSingleton<ForceSensor>();

        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate(PrismRegion.MainContentRegion, PrismNavigationUri.Settings);
            regionManager.RequestNavigate(PrismRegion.MainContentRegion, PrismNavigationUri.Setup);
            regionManager.RequestNavigate(PrismRegion.LogContentRegion, PrismNavigationUri.Log);
            regionManager.RequestNavigate(PrismRegion.StatusRegion, PrismNavigationUri.Status);

            regionManager.RequestNavigate(PrismRegion.SettingsTabControlRegion, PrismNavigationUri.ManualSettings);
            regionManager.RequestNavigate(PrismRegion.SettingsTabControlRegion, PrismNavigationUri.VertRecipSettings);
            regionManager.RequestNavigate(PrismRegion.SettingsTabControlRegion, PrismNavigationUri.HorizRecipSettings);

            var tabRegion = regionManager.Regions[PrismRegion.SettingsTabControlRegion];
            tabRegion.Activate(tabRegion.Views.First());


            var settings = Container.Resolve<TribometerSettings>();
            var controller = Container.Resolve<TribometerController>();
            var forceSensor = Container.Resolve<ForceSensor>(); 

            controller._settings = settings;
            forceSensor.Settings = settings;

            // This can be used to test the file writing without having the sensor connected
            //forceSensor.Fake();

            controller.SetForceSensor(forceSensor);

            base.OnInitialized();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

    }
}
