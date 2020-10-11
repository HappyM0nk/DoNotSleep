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
        private StopTimeChecker _stopTimeChecker;

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

        internal DateTime StopTime
        {
            get { return AppSettings.StopTime; }
            set
            {
                if (AppSettings.StopTime != value)
                {
                    AppSettings.StopTime = value;
                    AppSettings.Save();
                }
            }
        }

        internal Constants.TaskStatus KeyPressorStatus { get; private set; }

        public MainWindowModel()
        {
            _keyPressor = new KeyPressor(Timeout * 1000);
            _keyPressor.TaskStatusUpdated += UpdateStatus;
            _stopTimeChecker = new StopTimeChecker();
            _stopTimeChecker.TaskStatusUpdated += ;
        }

        internal void StartKeyPressor()
        {
            _keyPressor.Start();
        }

        internal void StopKeyPressor()
        {
            _keyPressor.Stop();
        }

        protected void UpdateStatus(Constants.TaskStatus status)
        {
            KeyPressorStatus = status;
            OnPropertyChanged("KeyPressorStatus");
        }

        protected void StartStopTimeChecker()
        {
            _stopTimeChecker.Start(StopTime);
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
