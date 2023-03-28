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
        public event EventHandler<string> TextReceived;        

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

        }


         // Methods - Manual move

        public void MoveTo(TribometerAxis axis, int stepPosition)
        {

        }


       // Methods - Handle Events

        private void SerialPortManager_TextReceived(object sender, string e)
        {
            TextReceived?.Invoke(this, e);
        }

        private void SerialPortManager_MessageReceived(object sender, byte[] e)
        {
            throw new NotImplementedException();
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
