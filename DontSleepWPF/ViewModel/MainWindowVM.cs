using DontSleepWPF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;

namespace DontSleepWPF.ViewModel
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        private MainWindowModel _model;

        public string Timeout 
        { 
            get { return _model.Timeout.ToString(); }
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = "0";
                }
                int intValue;
                if (int.TryParse(value, out intValue))
                {
                    if (intValue < 0)
                        intValue = Math.Abs(intValue);
                    if (_model.Timeout != intValue)
                    {
                        _model.Timeout = intValue;
                        OnPropertyChanged("Timeout");
                    }
                }
                
            }
        }

        public string StopTimePeriodValue
        {
            get { return _model.StopTimePeriodValue.ToString(); }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = "0";
                }
                int sValue;
                if (int.TryParse(value, out sValue))
                {
                    if (sValue < 0)
                        sValue = Math.Abs(sValue);
                    if (_model.StopTimePeriodValue != sValue)
                    {
                        _model.StopTimePeriodValue = sValue;
                        OnPropertyChanged("StopTimePeriodValue");
                    }
                }
                OnPropertyChanged("StopTimePeriodValue");
            }
        }

        private bool _IsStopTimeChecked;
        public bool StopTimeSwitched
        { 
            get { return _IsStopTimeChecked; }
            set
            {
                if (value == _IsStopTimeChecked)
                    return;
                _IsStopTimeChecked = value;
                OnPropertyChanged("StopTimeSwitched");
                OnPropertyChanged("IsStopTimePeriodEnabled");
                if (value == true)
                {
                    _model.EnableStopTimeChecker();
                }
                else
                {
                    _model.DisableStopTimeChecker();
                }
            }
        }

        public Constants.TimePeriod SelectedStopTimePeriod 
        { 
            get { return _model.StopTimePeriodType; }
            set
            {
                if (_model.StopTimePeriodType != value)
                {
                    _model.StopTimePeriodType = value;
                    OnPropertyChanged("SelectedStopTimePeriod");
                }
            }
        }

        public bool IsStopTimePeriodEnabled
        {
            get { return !_IsStopTimeChecked; }
        }

        public Brush KeyPressorStatusColor { get; private set; }


        //public ICommand StartCommand { get; private set; }
        //public ICommand StopCommand { get; private set; }
        public ICommand CheckedCommand { get; private set; }

        public Dictionary<Constants.TimePeriod, string> TimePeriods { get; private set; }

        public MainWindowVM(MainWindowModel model)
        {
            _model = model;
            _model.PropertyChanged += ModelPropertyChanged;
            //StartCommand = new RelayCommand(param => this.StartCommandHandler());
            //StopCommand = new RelayCommand(param => this.StopCommandHandler());
            CheckedCommand = new RelayCommand(param => CheckedCommandHandler(param));
            KeyPressorStatusColor = Brushes.LightGray;
            TimePeriods = new Dictionary<Constants.TimePeriod, string>()
            {
                {Constants.TimePeriod.Second, "seconds" },
                {Constants.TimePeriod.Minute, "minutes" },
                {Constants.TimePeriod.Hour, "hours" },
                {Constants.TimePeriod.Day, "days" },
                {Constants.TimePeriod.Week, "weeks" },
                {Constants.TimePeriod.Month, "months" }
            };
        }

        //private void StartCommandHandler()
        //{
        //    _model.EnableKeyPressor();
        //}

        //private void StopCommandHandler()
        //{
        //    _model.DisableKeyPressor();
        //}

        private void CheckedCommandHandler(object state)
        {
            if ((bool)state)
            {
                _model.EnableKeyPressor();
            }
            else
            {
                _model.DisableKeyPressor();
            }
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName== "KeyPressorStatus")
            {
                UpdateKeyPressorStatus(_model.KeyPressorStatus);
            }
        }        

        private void UpdateKeyPressorStatus(Constants.KeyPressorStatus status)
        {
            switch (status)
            {
                case Constants.KeyPressorStatus.Runned:
                    KeyPressorStatusColor = Brushes.LightGreen;
                    OnPropertyChanged("KeyPressorStatusColor");
                    break;
                case Constants.KeyPressorStatus.Stopped:
                    KeyPressorStatusColor = Brushes.Orange;
                    OnPropertyChanged("KeyPressorStatusColor");
                    break;
                case Constants.KeyPressorStatus.Faulted:
                    KeyPressorStatusColor = Brushes.Red;
                    OnPropertyChanged("KeyPressorStatusColor");
                    break;
                case Constants.KeyPressorStatus.StopTimerStarted:
                    _IsStopTimeChecked = true;
                    OnPropertyChanged("StopTimeSwitched");
                    OnPropertyChanged("IsStopTimePeriodEnabled");
                    break;
                case Constants.KeyPressorStatus.StopTimerStopped:
                    _IsStopTimeChecked = false;
                    OnPropertyChanged("StopTimeSwitched");
                    OnPropertyChanged("IsStopTimePeriodEnabled");
                    break;
                default:
                    KeyPressorStatusColor = Brushes.LightGray;
                    OnPropertyChanged("KeyPressorStatusColor");
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
