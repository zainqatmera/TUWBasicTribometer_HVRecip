using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TUWBasicTribometer_HVRecip.Controllers
{
    public enum MessageAck
    {
       Accepted = 0,
       Unknown,
       Invalid,
       Busy
    }

    public enum TribometerAxis {
        Horizontal,
        Vertical
    }

    public enum OperatingState : byte
    {
        [Description("Not Connected")]
        NotConnected,

        Idle,
        Homing,

        [Description("Moving")]
        ManualMove,         // Currently performing a move

        [Description("In Test (Horiz)")]
        RecipHorizontal,    // Currently reciprocating in horizontal motion

        [Description("In Test (Vert)")]
        RecipVertical,      // Currently reciprocating in vertical motion

        [Description("Error (Limit Switch)")]
        ErrorLimitSwitch    // Hit a limit switch during normal motion
    }

    public enum MotorControlParam : byte
    {
        MaxSpeed,
        Accel
    };
}
