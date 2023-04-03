using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUWBasicTribometer_HVRecip.Controllers;

namespace TUWBasicTribometer_HVRecip.ViewModels
{
    internal class StatusViewModel : BindableBase
    {
        public StatusViewModel() 
        { 
        }

        private OperatingState _currentOperatingState;
        public OperatingState CurrentOperatingState
        {
            get { return _currentOperatingState; }
            set { SetProperty(ref _currentOperatingState, value); }
        }

        private string _hPos_mm;
        public string HPos_mm
        {
            get { return _hPos_mm; }
            set { SetProperty(ref _hPos_mm, value); }
        }

        private int _hPos_step;
        public int HPos_step
        {
            get { return _hPos_step; }
            set { SetProperty(ref _hPos_step, value); }
        }

        private string _vPos_mm;
        public string VPos_mm
        {
            get { return _vPos_mm; }
            set { SetProperty(ref _vPos_mm, value); }
        }

        private int _vPos_step;
        public int VPos_step
        {
            get { return _vPos_step; }
            set { SetProperty(ref _vPos_step, value); }
        }

        private string _fx;
        public string Fx
        {
            get { return _fx; }
            set { SetProperty(ref _fx, value); }
        }

        private string _fy;
        public string Fy
        {
            get { return _fy; }
            set { SetProperty(ref _fy, value); }
        }

        private string _fz;
        public string Fz
        {
            get { return _fz; }
            set { SetProperty(ref _fz, value); }
        }

        private string _tx;
        public string Tx
        {
            get { return _tx; }
            set { SetProperty(ref _tx, value); }
        }

        private string _ty;
        public string Ty
        {
            get { return _ty; }
            set { SetProperty(ref _ty, value); }
        }

        private string _tz;
        public string Tz
        {
            get { return _tz; }
            set { SetProperty(ref _tz, value); }
        }

        private bool _isTribometerConnected;
        public bool IsTribometerConnected
        {
            get { return _isTribometerConnected; }
            set { SetProperty(ref _isTribometerConnected, value); }
        }

        private bool _isForceSensorDisconnected;
        public bool IsForceSensorDisconnected
        {
            get { return _isForceSensorDisconnected; }
            set { SetProperty(ref _isForceSensorDisconnected, value); }
        }

    }
}
