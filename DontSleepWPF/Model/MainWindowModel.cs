using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DontSleepWPF.Model
{
    internal class MainWindowModel : INotifyPropertyChanged
    {
        private KeyPressor _keyPressor;
        private DateTime _stopTime;

        internal int Timeout 
        { 
            get { return AppSettings.Timeout; }
            set 
            {
                if (AppSettings.Timeout != value)
                {
                    AppSettings.Timeout = value;
                    AppSettings.Save();
                    _keyPressor.UpdateTimeout(value * 1000);
                }
            }
        }

        internal Constants.KeyPressorStatus KeyPressorStatus { get; private set; }
        public int StopTimePeriodValue { get; internal set; }

        public Constants.TimePeriod StopTimePeriodType { get; set; }

        public MainWindowModel()
        {
            _keyPressor = new KeyPressor(Timeout * 1000);
            _keyPressor.TaskStatusUpdated += UpdateKeyPressorStatus;
            _stopTime = DateTime.MinValue;
            StopTimePeriodType = Constants.TimePeriod.Second;
            StopTimePeriodValue = 0;
        }

        internal void EnableKeyPressor()
        {
            _keyPressor.Start();
        }

        internal void DisableKeyPressor()
        {
            _keyPressor.Stop();
        }

        protected void UpdateKeyPressorStatus(Constants.KeyPressorStatus status)
        {
            KeyPressorStatus = status;
            OnPropertyChanged("KeyPressorStatus");
        }

        internal void EnableStopTimeChecker()
        {
            switch (StopTimePeriodType)
            {
                case Constants.TimePeriod.Second:
                    _stopTime = DateTime.Now.AddSeconds(StopTimePeriodValue);
                    break;
                case Constants.TimePeriod.Minute:
                    _stopTime = DateTime.Now.AddMinutes(StopTimePeriodValue);
                    break;
                case Constants.TimePeriod.Hour:
                    _stopTime = DateTime.Now.AddHours(StopTimePeriodValue);
                    break;
                case Constants.TimePeriod.Day:
                    _stopTime = DateTime.Now.AddDays(StopTimePeriodValue);
                    break;
                case Constants.TimePeriod.Week:
                    _stopTime = DateTime.Now.AddDays(StopTimePeriodValue*7);
                    break;
                case Constants.TimePeriod.Month:
                    _stopTime = DateTime.Now.AddMonths(StopTimePeriodValue);
                    break;
                default:
                    break;
            }            
            _keyPressor.StopAtTime(_stopTime);
        }

        internal void DisableStopTimeChecker()
        {
            _keyPressor.CancelStopTimer();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
