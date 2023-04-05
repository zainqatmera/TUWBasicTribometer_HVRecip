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

          //  containerRegistry.RegisterSingleton<SerialPortManager>();
            containerRegistry.RegisterSingleton<TribometerController>();
            containerRegistry.RegisterSingleton<TribometerSettings>();
            containerRegistry.RegisterSingleton<ForceSensor>();

        }

        protected override Window CreateShell()
        {
            //return Container.Resolve<SerialMonitorWindow>();
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate(PrismRegion.MainContentRegion, PrismNavigationUri.Setup);
            regionManager.RequestNavigate(PrismRegion.LogContentRegion, PrismNavigationUri.Log);
            regionManager.RequestNavigate(PrismRegion.StatusRegion, PrismNavigationUri.Status);

            var settings = Container.Resolve<TribometerSettings>();
            var controller = Container.Resolve<TribometerController>();
            var forceSensor = Container.Resolve<ForceSensor>(); 

            controller.Settings = settings;
            forceSensor.Settings = settings;


            /*            regionManager.RequestNavigate(RegionName.StatusTowerRegion,
                            NavigationUri.StatusTowerView);

                        regionManager.RequestNavigate(RegionName.MainTabRegion,
                     NavigationUri.DevelopmentTabView);
                        regionManager.RequestNavigate(RegionName.MainTabRegion, NavigationUri.ManualMoveTabView);

                        var tabRegion = regionManager.Regions[RegionName.MainTabRegion];
                        tabRegion.Activate(tabRegion.Views.First());

                        var dialogService = Container.Resolve<IDialogService>();
                        dialogService.Show(NavigationUri.DialogRunningLog);*/

/*            var spm = Container.Resolve<SerialPortManager>();
            spm.Connect(TribometerSettings.Instance.ComPortTribometer, TribometerSettings.Instance.ComPortTribometerBaudRate);
*/
            base.OnInitialized();
        }


    }
}
