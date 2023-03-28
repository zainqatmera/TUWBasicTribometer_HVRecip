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
        Move = 2,

        // Incoming
        SetDatumPosition = 128,   // The position of the homed limit switch Data[0] = AXIS; Data[1..8] = long position

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
