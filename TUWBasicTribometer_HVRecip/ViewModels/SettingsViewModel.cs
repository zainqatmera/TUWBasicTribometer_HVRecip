using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        public SettingsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _regionManager.RequestNavigate(PrismRegion.SettingsTabControlRegion, PrismNavigationUri.ManualSettings);
            _regionManager.RequestNavigate(PrismRegion.SettingsTabControlRegion, PrismNavigationUri.VertRecipSettings);

            var tabRegion = _regionManager.Regions[PrismRegion.SettingsTabControlRegion];
            tabRegion.Activate(tabRegion.Views.First());
        }
    }
}
