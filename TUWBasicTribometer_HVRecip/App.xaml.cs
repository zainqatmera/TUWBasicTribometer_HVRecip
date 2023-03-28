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

            containerRegistry.RegisterSingleton<SerialPortManager>();

        }

        protected override Window CreateShell()
        {
            return Container.Resolve<SerialMonitorWindow>();
        }

        protected override void OnInitialized()
        {
            var regionManager = Container.Resolve<IRegionManager>();
            /*            regionManager.RequestNavigate(RegionName.StatusTowerRegion,
                            NavigationUri.StatusTowerView);

                        regionManager.RequestNavigate(RegionName.MainTabRegion,
                     NavigationUri.DevelopmentTabView);
                        regionManager.RequestNavigate(RegionName.MainTabRegion, NavigationUri.ManualMoveTabView);

                        var tabRegion = regionManager.Regions[RegionName.MainTabRegion];
                        tabRegion.Activate(tabRegion.Views.First());

                        var dialogService = Container.Resolve<IDialogService>();
                        dialogService.Show(NavigationUri.DialogRunningLog);*/

            var spm = Container.Resolve<SerialPortManager>();
            spm.Connect(TribometerSettings.Instance.ComPortTribometer, TribometerSettings.Instance.ComPortTribometerBaudRate);

            base.OnInitialized();
        }


    }
}
