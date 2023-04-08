using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using static ImTools.ImMap;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    public class TribometerController
    {
        private SerialPortManager serialPortManager;
        public TribometerSettings _settings { get; set; }

        private OperatingState _state;
        private int currentHPos;
        private int currentVPos;

        private int currentTestCycleCount = 0;

        // Properties

        public int PositionH => currentHPos;
        public int PositionV => currentVPos;

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
        public event EventHandler<TribometerAxis> TestStartedNotification;  // Test started with the specified axis reciprocating


        // Constructor

        public TribometerController()
        {
        }


        // Methods - General connection and messaging

        public void Connect()
        {
            if (serialPortManager == null)
            {
                serialPortManager = new SerialPortManager();
                serialPortManager.MessageReceived += SerialPortManager_MessageReceived;
                serialPortManager.TextReceived += SerialPortManager_TextReceived;

                serialPortManager.Connect(_settings.ComPortTribometer, _settings.ComPortTribometerBaudRate);
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
                case MessageCode.StartHorizontalReciprocation:
                    TestStartedNotification?.Invoke(this, TribometerAxis.Horizontal);
                    break;
                case MessageCode.StartVerticalReciprocation:
                    TestStartedNotification?.Invoke(this, TribometerAxis.Vertical);
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

        // Process Incoming Messages

        private void ProcessCyclePointMark(byte[] e)
        {
            if (e[1] == 1)
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

        // Outgoing Messages - Main control and set up

        public void HomeTribometer()
        {
            SendCommand(MessageCode.Home);
        }

        public void Stop()
        {
            SendCommand(MessageCode.StopMotion);
        }

        public void EmergencyRaise()
        {
            SendCommand(MessageCode.EmergencyRaiseUp);
        }

        // Outgoing Messages - Manual move / Stop

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

        internal void AdjustLoadToPosition(int value)
        {
            byte[] bytes = new byte[4];
            MemoryStream ms = new MemoryStream(bytes);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(value);
            SendCommand(MessageCode.AdjustLoadMoveVerticalTo, bytes);
        }

        internal void AdjustLoad(int stepsToMove)
        {
            byte[] bytes = new byte[4];
            MemoryStream ms = new MemoryStream(bytes);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(stepsToMove);
            SendCommand(MessageCode.AdjustLoadMoveVertical, bytes);
        }

        // Outgoing Messages - settings

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

        public void SetMotorControlParamsManual()
        {
            SetMotorControlParams(_settings.moveMaxSpeedH, _settings.moveAccelH, _settings.moveMaxSpeedV, _settings.moveAccelV);
        }

        internal void SetMotorControlParamsVertTest()
        {
            SetMotorControlParams(_settings.vertTestMaxSpeedH, _settings.vertTestAccelH, _settings.vertTestMaxSpeedV, _settings.vertTestAccelV);
        }

        internal void SetMotorControlParamsHorizTest()
        {
            SetMotorControlParams(_settings.horizTestMaxSpeedH, _settings.horizTestAccelH, _settings.horizTestMaxSpeedV, _settings.horizTestAccelV);
        }

        // Outgoing messages - tests

        internal void StartVertRecipTest()
        {
            currentTestCycleCount = 0;
            TestCycleCountUpdated?.Invoke(this, currentTestCycleCount);

            byte[] buffer = new byte[20];
            MemoryStream ms = new MemoryStream(buffer);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(_settings.stepPosVUnloaded.Value);
            bw.Write(_settings.stepPosVLoaded.Value);
            bw.Write(_settings.vertTestPauseTimeUnloaded);
            bw.Write(_settings.vertTestPauseTimeLoaded);
            bw.Write(_settings.vertTestStopAtNumberOfCycles ? _settings.vertTestTargetNumberOfCycles : -1);

            SetMotorControlParamsVertTest();
            SendCommand(MessageCode.StartVerticalReciprocation, buffer);
        }

        internal void StartHorizRecipTest()
        {
            currentTestCycleCount = 0;
            TestCycleCountUpdated?.Invoke(this, currentTestCycleCount);

            byte[] buffer = new byte[25];
            MemoryStream ms = new MemoryStream(buffer);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(_settings.stepPosHLeft.Value);
            bw.Write(_settings.stepPosHRight.Value);
            bw.Write(_settings.horizTestPauseTime);
            bw.Write(_settings.vertTestStopAtNumberOfCycles ? _settings.horizTestTargetNumberOfCycles : -1);
            bw.Write((byte)_settings.horizTestNormalLoadingProfile);
            bw.Write(_settings.stepPosVUnloaded.Value);
            bw.Write(_settings.stepPosVLoaded.Value);

            SetMotorControlParamsHorizTest();
            SendCommand(MessageCode.StartHorizontalReciprocation, buffer);

        }

        internal void EndTest()
        {
            SendCommand(MessageCode.EndTest);
        }


    }
}
