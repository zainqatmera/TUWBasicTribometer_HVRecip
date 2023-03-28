using System;
using System.Collections.Generic;
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

        }


       // Methods - Handle Events

        private void SerialPortManager_TextReceived(object sender, string e)
        {
            InfoLogIssued?.Invoke(this, e);
        }

        private void SerialPortManager_MessageReceived(object sender, byte[] e)
        {
            MessageCode messageId = (MessageCode)e[0];
            string data = "";
            if (e.Length > 1) {
                data = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(e,1, e.Length - 1).ToArray());
            }
            InfoLogIssued?.Invoke(this, $"{messageId} {data}");
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
