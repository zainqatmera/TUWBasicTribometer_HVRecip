using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    public class TribometerController
    {
        private SerialPortManager serialPortManager;
        private OperatingState _state;
        private int currentHPos;
        private int currentVPos;

        private int currentTestCycleCount = 0;

        public int PositionH => currentHPos;
        public int PositionV => currentVPos;

        public TribometerSettings Settings { get; set; }


        public TribometerController()
        {            
        }

        public OperatingState State { 
            get => _state; 
            set { 
                if (value != _state)
                {
                    _state = value;
                    StateChanged?.Invoke(this, _state);                    
                }
            }
        }

        // Events 

        public event EventHandler<TribometerPositionEventArgs> PositionUpdated;
        public event EventHandler<OperatingState> StateChanged;
        public event EventHandler<string> InfoLogIssued;
        public event EventHandler<int> TestCycleCountUpdated;

        // Methods - General connection and messaging

        public void Connect()
        {
            if (serialPortManager == null)
            {
                serialPortManager = new SerialPortManager();
                serialPortManager.MessageReceived += SerialPortManager_MessageReceived;
                serialPortManager.TextReceived += SerialPortManager_TextReceived;

                serialPortManager.Connect(Settings.ComPortTribometer, Settings.ComPortTribometerBaudRate);
            }

            serialPortManager.StartMessaging();
        }

        public void Disconnect()
        {
            serialPortManager?.Disconnect();
        }

        public void SendCommand(MessageCode messageId, byte[] data = null)
        {           
            serialPortManager?.SendCommand((byte)messageId, data);
        }

        // Methods - Homing and setup

        public void HomeTribometer()
        {
            SendCommand(MessageCode.Home);
        }


         // Methods - Manual move

        public void MoveTo(TribometerAxis axis, int stepPosition)
        {
            SetMotorControlParamsManual();

            byte[] buffer = new byte[5];
            MemoryStream ms = new MemoryStream(buffer);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((byte)axis);
            bw.Write(stepPosition);
            SendCommand(MessageCode.MoveTo, buffer);
        }

        internal void Move(TribometerAxis axis, int moveSteps)
        {
            SetMotorControlParamsManual();

            byte[] buffer = new byte[5];
            MemoryStream ms = new MemoryStream(buffer);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((byte)axis);
            bw.Write(moveSteps);
            SendCommand(MessageCode.MoveRel, buffer);
        }



        // Methods - Handle Events

        private void SerialPortManager_TextReceived(object sender, string e)
        {
            InfoLogIssued?.Invoke(this, e);
        }

        private void SerialPortManager_MessageReceived(object sender, byte[] e)
        {
            MessageCode messageId = (MessageCode)e[0];

            switch (messageId)
            {
                case MessageCode.SetDatumPosition:
                    ProcessSetDatum(e);
                    break;
                case MessageCode.StatusPosition:
                    ProcessStatusPosition(e);
                    break;
                case MessageCode.MessageResponse:
                    ProcessMessageResponse(e);
                    break;
                case MessageCode.StatusOperatingState:
                    ProcessStatusOperatingState(e);
                    break;
                case MessageCode.CyclePointMark:
                    ProcessCyclePointMark(e);
                    break;
                default:
                    {
                        string data = "";
                        if (e.Length > 1)
                        {
                            data = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(e, 1, e.Length - 1).ToArray());
                        }
                        InfoLogIssued?.Invoke(this, $"{messageId} {data}");

                    }
                    break;
            }

        }

        private void ProcessCyclePointMark(byte[] e)
        {
            if (e[1] == 5)
            {
                currentTestCycleCount++;
                TestCycleCountUpdated?.Invoke(this, currentTestCycleCount);
            }
        }

        private void ProcessMessageResponse(byte[] e)
        {
            MemoryStream ms = new MemoryStream(e);
            BinaryReader br = new BinaryReader(ms);
            br.ReadByte();  // Message code
            MessageCode repliedMessageCode = (MessageCode)br.ReadByte();
            MessageAck response = (MessageAck)br.ReadByte();

            InfoLogIssued?.Invoke(this, $"Message: {repliedMessageCode} {response}");

        }

        private void ProcessStatusPosition(byte[] e)
        {
            MemoryStream ms = new MemoryStream(e);
            BinaryReader br = new BinaryReader(ms);
            br.ReadByte();  // Message code
            currentHPos = br.ReadInt32();
            currentVPos = br.ReadInt32();

            InfoLogIssued?.Invoke(this, $"Stopped at H: {currentHPos} V: {currentVPos}");
            PositionUpdated?.Invoke(this, new TribometerPositionEventArgs(currentHPos, currentVPos));
        }

        private void ProcessStatusOperatingState(byte[] e)
        {
            MemoryStream ms = new MemoryStream(e);
            BinaryReader br = new BinaryReader(ms);
            br.ReadByte();  // Message code
            State = (OperatingState)br.ReadByte();
        }


        private void ProcessSetDatum(byte[] e)
        {
            MemoryStream ms = new MemoryStream(e);
            BinaryReader br = new BinaryReader(ms);
            br.ReadByte();  // Message code
            TribometerAxis axis = (TribometerAxis)br.ReadByte();
            int pos = br.ReadInt32();

            InfoLogIssued?.Invoke(this, $"{axis} datum found at {pos}");
        }


        // Other outgoing commands

        void SetMotorControlParamsManual()
        {
            SetMotorControlParams(Settings.moveMaxSpeedH, Settings.moveAccelH, Settings.moveMaxSpeedV, Settings.moveAccelV);
        }

        public void SetMotorControlParams(float hSpeed, float hAccel, float vSpeed, float vAccel)
        {
            byte[] data = new byte[6];
            MemoryStream ms = new MemoryStream(data);
            BinaryWriter bw = new BinaryWriter(ms);
            
            bw.Write((byte)TribometerAxis.Horizontal);
            bw.Write((byte)MotorControlParam.MaxSpeed);
            bw.Write(hSpeed);
            SendCommand(MessageCode.SetMotorControlParam, data);

            ms.Position = 0;
            bw.Write((byte)TribometerAxis.Horizontal);
            bw.Write((byte)MotorControlParam.Accel);
            bw.Write(hAccel);
            SendCommand(MessageCode.SetMotorControlParam, data);

            ms.Position = 0;
            bw.Write((byte)TribometerAxis.Vertical);
            bw.Write((byte)MotorControlParam.MaxSpeed);
            bw.Write(vSpeed);
            SendCommand(MessageCode.SetMotorControlParam, data);

            ms.Position = 0;
            bw.Write((byte)TribometerAxis.Vertical);
            bw.Write((byte)MotorControlParam.Accel);
            bw.Write(vAccel);
            SendCommand(MessageCode.SetMotorControlParam, data);


        }

        internal void SetMotorControlParamsVertTest()
        {
            SetMotorControlParams(Settings.vertTestMaxSpeedH, Settings.vertTestAccelH, Settings.vertTestMaxSpeedV, Settings.vertTestAccelV);
        }
        internal void Stop()
        {
            SendCommand(MessageCode.StopMotion);
        }

        internal void EmergencyRaise()
        {
            SendCommand(MessageCode.EmergencyRaiseUp);
        }

        internal void StartVertRecipTest()
        {
            currentTestCycleCount = 0;
            TestCycleCountUpdated?.Invoke(this, currentTestCycleCount);

            byte[] buffer = new byte[20];
            MemoryStream ms = new MemoryStream(buffer);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(Settings.stepPosVUnloaded.Value);
            bw.Write(Settings.stepPosVLoaded.Value);
            bw.Write(Settings.vertTestPauseTimeUnloaded);
            bw.Write(Settings.vertTestPauseTimeLoaded);
            bw.Write(Settings.vertTestNumberOfCycles);

            SetMotorControlParamsVertTest();
            SendCommand(MessageCode.StartVerticalReciprocation, buffer);
        }
    }
}
