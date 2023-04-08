using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    public enum MessageCode : byte
    {

        // ====== PC -> Tribometer MESSAGES =======

        // Stop motion safely
        StopMotion = 0,

        // Run the homing process
        Home = 1,

        // Move relative to the current position
        // 1 byte  : Axis
        // 4 bytes : (long) Steps to move
        MoveRel = 2,

        // Move to the specified position
        // 1 byte  : Axis
        // 4 bytes : (long) position
        MoveTo = 3,

        // Request status information to be sent
        RequestStatus = 9,

        // Start a vertical reciprocation test
        // 4 bytes : (long) Unloaded position
        // 4 bytes : (long) Loaded position
        // 4 bytes : (long) Pause unloaded (ms)
        // 4 bytes : (long) Pause loaded (ms)
        // 4 bytes : (long) Number of cycles (-1 to continue indefinitely)
        StartVerticalReciprocation = 10,

        // Start a horizontal reciprocation test
        // 4 bytes : (long) Left End position
        // 4 bytes : (long) Right end position
        // 4 bytes : (long) Pause at each end (ms)    
        // 4 bytes : (long) Number of cycles (-1 to continue indefinitely)
        // 1 byte  : NormalLoadingProfile
        // 4 bytes : (long) Unloaded position 
        // 4 bytes : (long) Loaded position 
        StartHorizontalReciprocation = 11,

        // End the current test (horizontal or vertical) at the end of the current cycle
        EndTest = 15,

        // Set a motor contor parameter
        // 1 byte  : Axis
        // 1 byte  : ControlParameter
        // x bytes : Value (4 bytes float for AccelStepper MaxSpeed and Accel)
        SetMotorControlParam = 20,   // Data[0] = Axis, Data[1] = MotorControlParam, Data[2..?] = value

        // Set a test parameter 0 - use to update during a test
        // 1 byte  : TestSettingsParameter
        // x bytes : Value
        SetTestSettingsParam = 21,


        // Return to idle state from an error state
        ClearError = 99,    // Reset to idle from an error state;

        // Perform an emergency raising of the vertical axis (to avoid force sensor overload) and stop H axis
        EmergencyRaiseUp = 100,

        // ====== Tribometer -> PC MESSAGES =======

        // OUTGOING

        // Provide the homed position for one axis (at limit switch contact)
        // 1 byte  : Axis
        // 4 bytes : (long) 
        SetDatumPosition = 128,

        // Notify of the operating state
        // 1 byte  : OperatingState
        StatusOperatingState = 140,

        // Notify of the current position
        // 4 bytes : (long) Horizontal position
        // 4 bytes : (long) Vertical position
        StatusPosition = 141,

        // Notify of the motor control parmaters
        // 1 byte  : Axis
        // 1 byte  : MotorControlParam 
        // ? bytes : Value  (4 bytes float for AccelStepper MaxSpeed and Accel)
        StatusMotorControlParam = 142,  // Data[0] = Axis, Data[1] = MotorControlParam, Data[2..?] = Value

        // Notify of reaching a point in a test cycle
        // 1 byte  : identifier
        CyclePointMark = 150,

        // Notify of end of test (number of cycles reached)
        RecipEnd = 151,

        // Notify of an error state
        // 1 byte  : Error type
        ErrorAlarm = 199,

        // Send a log message
        TextLog = 200,

        // Acknowledge receipt of an incoming message
        // 1 byte  : MessageId of received message
        // 1 byte  : Acknowledgment (ack)
        MessageResponse = 255
    }
}
