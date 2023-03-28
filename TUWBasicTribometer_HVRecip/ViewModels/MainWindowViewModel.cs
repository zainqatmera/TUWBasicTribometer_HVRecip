using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        private readonly IContainer container;
        private readonly IRegionManager regionManager;

        public MainWindowViewModel(IContainer container, IRegionManager regionManager )
        {
            this.container = container;
            this.regionManager = regionManager;

            MainNavigationCommand = new DelegateCommand<string>(MainNavigation);
        }

        private void MainNavigation(string obj)
        {
            switch (obj)
            {
                case "Setup":
                    regionManager.RequestNavigate(PrismRegion.LogContentRegion, PrismNavigationUri.Log); 
                    regionManager.RequestNavigate(PrismRegion.MainContentRegion, PrismNavigationUri.Setup); 
                    break;
            }

        }



        // Bindings
        public DelegateCommand<string> MainNavigationCommand { get; private set; }

    }
}
