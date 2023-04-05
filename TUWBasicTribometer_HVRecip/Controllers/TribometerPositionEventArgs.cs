using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    public class TribometerPositionEventArgs : EventArgs
    {
        public int? HorizontalStepPosition { get; }
        public int? VerticalStepPosition { get; }

        public TribometerPositionEventArgs(int? horizontalStepPosition, int? verticalStepPosition)
        {
            HorizontalStepPosition = horizontalStepPosition;
            VerticalStepPosition = verticalStepPosition;
        }

    }
}
