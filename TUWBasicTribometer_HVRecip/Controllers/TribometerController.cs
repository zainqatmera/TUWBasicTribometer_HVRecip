using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    internal class TribometerController
    {
        private SerialPortManager serialPortManager;
        private TribometerState _state;

        public TribometerController()
        {

        }

        public TribometerState State { 
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

        public event EventHandler<TribometerState> StateChanged;
        public event EventHandler<string> InfoLogIssued;        

        // Methods - General connection and messaging

        public void Connect()
        {
            if (serialPortManager == null)
            {
                serialPortManager = new SerialPortManager();
                serialPortManager.MessageReceived += SerialPortManager_MessageReceived;
                serialPortManager.TextReceived += SerialPortManager_TextReceived;

                serialPortManager.Connect(TribometerSettings.Instance.ComPortTribometer, TribometerSettings.Instance.ComPortTribometerBaudRate);
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
            byte[] buffer = new byte[5];
            MemoryStream ms = new MemoryStream(buffer);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((byte)axis);
            bw.Write(stepPosition);
            SendCommand(MessageCode.MoveTo, buffer);
        }

        internal void Move(TribometerAxis axis, int moveSteps)
        {
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
            int hpos = br.ReadInt32();
            int vpos = br.ReadInt32();

            InfoLogIssued?.Invoke(this, $"Stopped at H: {hpos} V: {vpos}");
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

        internal void Stop()
        {

        }

        internal void EmergencyRaise()
        {
        }



        // Nested 

        public enum TribometerState
        {
            NotConnected,
            Idle,
            ManualMove,   // Currently moving
            ReciprocatingH,
            ReciprocatingV
        }

        

    }
}
