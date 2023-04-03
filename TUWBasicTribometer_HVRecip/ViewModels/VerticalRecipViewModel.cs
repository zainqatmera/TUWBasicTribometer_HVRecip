using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    internal class VerticalRecipViewModel : BindableBase
    {


        private int _upperPosition;
        public int UpperPosition
        {
            get => _upperPosition;
            set => SetProperty(ref _upperPosition, value);
        }

        private int _lowerPosition;
        public int LowerPosition { 
            get => _lowerPosition;
            set => SetProperty(ref _lowerPosition, value);
        }

    }
}
