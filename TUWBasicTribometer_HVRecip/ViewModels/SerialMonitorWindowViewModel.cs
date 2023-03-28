using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    internal class SerialMonitorWindowViewModel : BindableBase, IDisposable
    {
        private readonly SerialPortManager serialPortManager;

        public SerialMonitorWindowViewModel(IContainer container)
        {
            serialPortManager = container.Resolve<SerialPortManager>();
            serialPortManager.TextReceived += SerialPortManager_TextReceived;
            serialPortManager.MessageReceived += SerialPortManager_MessageReceived;
        }

        private void SerialPortManager_MessageReceived(object sender, byte[] e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var commandId = (MessageCode)e[0];
                switch (commandId)
                {
                    case MessageCode.TextLog:
                        IncomingMessages.Add(System.Text.Encoding.UTF8.GetString(new ReadOnlySpan<byte>(e, 1, e.Length - 1).ToArray()));
                        break;

                    case MessageCode.MessageResponse:
                        {
                            // Handle message responses separately
                            var ack = (MessageAck)e[2];
                            IncomingMessages.Add($"Message {e[1]} was {ack}");
                        }
                        break;

                    case MessageCode.SetDatumPosition:
                        {
                            TribometerAxis axis = (TribometerAxis)e[1];
                            long position = BitConverter.ToInt32(e, 2);
                            IncomingMessages.Add($"{axis} datum set to {position}");
                        }
                        break;

                    default:
                        IncomingMessages.Add($"Message Id: {commandId.ToString("X2")} :");
                        if (e.Length > 2)
                        {
                            IncomingMessages.Add(BitConverter.ToString(e.AsSpan(2).ToArray()));
                        }
                        break;
                }
            });
        }

        private void SerialPortManager_TextReceived(object sender, string e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IncomingMessages.Add(e);
            });
        }

        public void Dispose()
        {
            if (serialPortManager != null)
            {
                serialPortManager.TextReceived -= SerialPortManager_TextReceived;
                serialPortManager.MessageReceived -= SerialPortManager_MessageReceived;
            }
        }

        // Bindings

        private ObservableCollection<string> _incomingMessages = new ObservableCollection<string>();
        public ObservableCollection<string> IncomingMessages
        {
            get => _incomingMessages;
            set => SetProperty(ref _incomingMessages, value);
        }

        private byte _messageId = 1;
        public byte MessageId
        {
            get => _messageId;
            set => SetProperty(ref _messageId, value);
        }

        private string _messageData;
        public string MessageData
        {
            get => _messageData;
            set => SetProperty(ref _messageData, value);
        }

        private string _dataEntry1;
        public string DataEntry1
        {
            get => _dataEntry1;
            set => SetProperty(ref _dataEntry1, value);
        }

        private bool _horizontalAxisCheck;
        public bool HorizontalAxisCheck
        {
            get => _horizontalAxisCheck;
            set => SetProperty(ref _horizontalAxisCheck, value);
        }

        private DelegateCommand _sendMessageCommand;
        public DelegateCommand SendMessageCommand => _sendMessageCommand ??= new DelegateCommand(SendMessage);

        private DelegateCommand _sendHandshakeCommand;
        public DelegateCommand SendHandshakeCommand => _sendHandshakeCommand ??= new DelegateCommand(SendHandshake);

        private DelegateCommand _moveCommand;
        public DelegateCommand MoveCommand => _moveCommand ??= new DelegateCommand(SendMove);



        // Methods

        private void SendMessage()
        {
            if (String.IsNullOrEmpty(MessageData))
                serialPortManager.SendCommand(MessageId, null);
            else
            {
                byte[] bytes = UTF8Encoding.UTF8.GetBytes(MessageData);
                serialPortManager.SendCommand(MessageId, bytes);
            }
        }

        private void SendMove()
        {
            try
            {
                long targetPosition = Convert.ToInt64(DataEntry1);
                byte axisByte = (byte)(HorizontalAxisCheck ? 0 : 1);
                byte[] buffer = new byte[9];
                MemoryStream ms = new MemoryStream(buffer);
                ms.WriteByte(axisByte);
                ms.Write(BitConverter.GetBytes(targetPosition), 0, 8);
                serialPortManager.SendCommand((byte)MessageCode.Move, buffer);
            }
            catch { }
        }

        private void SendHandshake()
        {
            serialPortManager.StartMessaging();
        }

    }
}
