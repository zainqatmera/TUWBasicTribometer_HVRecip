using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    internal class InfoLogViewModel : ViewModelBase, IDisposable
    {
        private readonly TribometerController _controller;

        public InfoLogViewModel(IContainer container)
        {
            _controller = container.Resolve<TribometerController>();
            _controller.InfoLogIssued += TribometerController_InfoLogIssued;

            ClearLogCommand = new DelegateCommand(() => IncomingMessages.Clear());
        }

        public void Dispose()
        {
            _controller.InfoLogIssued -= TribometerController_InfoLogIssued;
        }


        private void TribometerController_InfoLogIssued(object sender, string e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IncomingMessages.Add(e);
                SelectedLogMessage = e;
            });
        }

        private object _selectedLogMessage;
        public object SelectedLogMessage
        {
            get { return _selectedLogMessage; }
            set { SetProperty(ref _selectedLogMessage, value); }
        }

        public DelegateCommand ClearLogCommand { get; }

        private ObservableCollection<string> _incomingMessages = new ObservableCollection<string>();
        public ObservableCollection<string> IncomingMessages
        {
            get => _incomingMessages;
            set => SetProperty(ref _incomingMessages, value);
        }

    }
}
