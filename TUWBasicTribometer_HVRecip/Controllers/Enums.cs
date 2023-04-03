using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
   enum MessageAck
    {
       Accepted = 0,
       Unknown,
       Invalid,
       Busy
    }

    enum TribometerAxis {
        Horizontal,
        Vertical
    }

    enum OperatingState : byte
    {
        NotConnected,
        Idle,
        Homing,
        ManualMove,         // Currently performing a move
        RecipHorizontal,    // Currently reciprocating in horizontal motion
        RecipVertical,      // Currently reciprocating in vertical motion
        ErrorLimitSwitch    // Hit a limit switch during normal motion
    }

}
