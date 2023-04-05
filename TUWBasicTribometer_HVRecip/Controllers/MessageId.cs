using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    public enum MessageCode : byte
    {
        StopMotion = 0,
        Home = 1,
        MoveRel = 2,    // Data[0] = Axis, Data[1...4] move steps
        MoveTo = 3,     // Data[0] = Axis, Data[1...4] move to position

        RequestStatus = 9,


        StartVerticalReciprocation = 10,        // Data[0...3] MinPosition (Unloaded), Data[4...7] MaxPosition (Loaded)

        EndTest = 15,

        SetMotorControlParam = 20,   // Data[0] = Axis, Data[1] = MotorControlParam, Data[2..?] = value


        ClearError = 99,    // Reset to idle from an error state;
        EmergencyRaiseUp = 100,  // Raise the vertical axis to avoid force sensor overload, and stop h

        // OUTGOING
        SetDatumPosition = 128,   // The position of the homed limit switch Data[0] = AXIS; Data[1..8] = long position

        StatusOperatingState = 140,     // Send when the operating state is changed  data[0] = operating state
        StatusPosition = 141,   // Data[0..3] = Horizontal position, Data[4..7] = Vertical position
        StatusMotorControlParam = 142,  // Data[0] = Axis, Data[1] = MotorControlParam, Data[2..?] = Value

        CyclePointMark = 150,   // Mark a point in reciprocation cycle: data[0] = marker identifier
        RecipEnd = 151,         // Reciprocating motion has ended

        ErrorAlarm = 199,       // An error state has been raised ; Data[0] = ErrorAlarmType
        TextLog = 200,

        MessageResponse = 255   // Data[0] = MessageId of received message, Data[1] = Ack



        /*        HorizontalMove = 0x10,              // Data: Int64 new postion
                HorizontalSetDatum = 0x11,          // Data: -
                HorizontalSetMaxSpeed = 0x12,       // Data: Float, steps per second
                HorizontalSetAccel = 0x13,          // Data: Float, steps per second/second
                HorizontalSetAmplitude = 0x14,      // Data: Int64, number of steps amplitude (half of stroke length)
                HorizontalStart = 0x15,             // Data: Int, number of cycles
                HorizontalStop = 0x16,
                VerticalMove = 0x20,
                VerticalSetDatum = 0x21,
                VerticalSetMaxSpeed = 0x22,
                VerticalSetAccel = 0x23
        */
    }
}
