using ImTools;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    internal class SerialPortManager : IDisposable
    {        

        SerialPort serialPort;

        public event EventHandler<string> TextReceived;
        public event EventHandler<byte[]> MessageReceived;

        byte[] connectionString = { 0, 0, 0, 0, 0, 0, 0x55, 0x54, 0x30, 0x28, 0x20 };  // Send this to initiate a messaging connection
        byte[] connectionConfirmedResponseString = { 0x55, 0x54, 0x30, 0x28, 0x21 };    // This will be received when messaging connection is ready
        int currentConfirmationPoint = 0;

        private bool isInMessagingState = false; // When in messaging state, incoming data is treated as a message format, otherwise a text format 
        private bool isWaitingForConnectionConfirmation = false;

        // Incoming Messages
        private bool isReadingMessageLength;
        private byte[] messageLengthBuffer = new byte[4];
        private int messageLegnthPosition;
        
        private byte[] incomingData;  
        private uint incomingDataReceivedLength = 0;    // How much of current message has been received 
        private uint incomingDataExpectedLength = 0;    // How long should current message be

        public SerialPortManager()
        {

        }

        public bool Connect(string comPort, int baudRate)
        {
            try
            {
                serialPort = new SerialPort(comPort, baudRate);
                serialPort.Open();
                serialPort.DataReceived += ProcessIncomingData;
            }
            catch
            {
                return false;
            }

            if (serialPort == null) { return false; }

            return serialPort.IsOpen;
        }
            
        public bool StartMessaging()
        {
            currentConfirmationPoint = 0;
            isWaitingForConnectionConfirmation = true;
            serialPort.Write(connectionString, 0, connectionString.Length);
            return true;
        }

        public bool SendCommand(byte commandId, byte[] data)
        {
            if (data == null)
            {
                var fulldata = new byte[] { 1, commandId };
                serialPort.Write(fulldata, 0, fulldata.Length);
            }
            else
            {
                if (data.Length > 253) return false;
                var fulldata = new byte[data.Length + 2];
                fulldata[0] = (byte)(data.Length + 1);
                fulldata[1] = commandId;
                data.CopyTo(fulldata, 2);
                serialPort.Write(fulldata, 0, fulldata.Length);
            }
            return true;
        }

        public void Disconnect()
        {
            if (serialPort != null)
            {
                serialPort.DataReceived -= ProcessIncomingData;
                serialPort.Close();
            }
        }

        private void ProcessIncomingData(object sender, SerialDataReceivedEventArgs e)
        {     
            if (isWaitingForConnectionConfirmation)
            {
                while (serialPort.BytesToRead > 0)
                {
                    int b = serialPort.ReadByte();
                    if (b < 0) { throw new Exception("End of stream"); }   // This should not be necessary
                    else if (b == 0)
                    {
                        currentConfirmationPoint = 0;  // Restart messaging confirmation point
                    }
                    else if ((byte)b == connectionConfirmedResponseString[currentConfirmationPoint])
                    {
                        currentConfirmationPoint++;
                        if (currentConfirmationPoint == connectionConfirmedResponseString.Length)
                        {
                            TextReceived?.Invoke(this, "Messaging Started");
                            isWaitingForConnectionConfirmation = false;
                            isInMessagingState = true;
                            isReadingMessageLength = true;
                            messageLegnthPosition = 0;
                            break;
                        }
                    } 
                    else // Bad data so restart listening for confirmation
                    {
                        currentConfirmationPoint = 0;  // Restart messaging confirmation point
                    }
                }
            }

            if (isInMessagingState)
            {
                // Process byte-by-byte for convenience
                while (serialPort.BytesToRead > 0)
                {
                    int b = serialPort.ReadByte();  // TODO: Maybe check not end of stream? -1

                    if (isReadingMessageLength)
                    {
                        messageLengthBuffer[messageLegnthPosition] = (byte)b;
                        messageLegnthPosition++;
                        if (messageLegnthPosition == 4)
                        {
                            // Message length known
                            incomingDataExpectedLength = BitConverter.ToUInt32(messageLengthBuffer, 0);
                            incomingDataReceivedLength = 0;
                            incomingData = new byte[incomingDataExpectedLength];
                            isReadingMessageLength = false;
                        }
                    }
                    else
                    {
                        incomingData[incomingDataReceivedLength] = (byte)b;
                        incomingDataReceivedLength++;
                        if (incomingDataReceivedLength == incomingDataExpectedLength)
                        {
                            // Message fully received
                            MessageReceived?.Invoke(this, incomingData);
                            messageLegnthPosition = 0;
                            isReadingMessageLength = true;
                        }
                    }
                }
            }
            else // Text state before messaging started
            { 
                int nbytes = serialPort.BytesToRead;
                if (nbytes == 0) return;

                byte[] data = new byte[nbytes]; 
                serialPort.Read(data, 0, nbytes);
                TextReceived?.Invoke(this, ASCIIEncoding.UTF8.GetString(data, 0, nbytes));
            }
        }

     
        public void Dispose()
        {
            Disconnect();
        }
    }
}
