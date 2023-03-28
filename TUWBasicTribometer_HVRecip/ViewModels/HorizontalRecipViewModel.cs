using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    public class HorizontalRecipViewModel : BindableBase
    {
        /*        private bool _temp;
                public bool Temp
                {
                    get => _temp;   
                    set { SetProperty(ref _temp, value); }
                }*/

        private bool _allowChanges;
        public bool AllowChanges
        {
            get => _allowChanges;
            set { SetProperty(ref _allowChanges, value); }
        }

        private double _targetLoad;
        public double TargetLoad
        {
            get => _targetLoad;
            set { SetProperty(ref _targetLoad, value); }
        }


    }
}
