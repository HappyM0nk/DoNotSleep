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

        public int Timeout 
        { 
            get { return _model.Timeout; }
            set 
            { 
                if (_model.Timeout!=value)
                {
                    _model.Timeout = value;
                    OnPropertyChanged("Timeout");
                }
            }
        }

        public DateTime StopTime
        {
            get { return _model.StopTime; }
            set
            {
                if (_model.StopTime != value)
                {
                    _model.StopTime = value;
                    OnPropertyChanged("StopTime");
                }
            }
        }

        public bool StopSwitcherAvailable { get; set; }
        //{
        //    get { return _model.StopPlanned; }
        //    set
        //    {
        //        if (_model.StopPlanned != value)
        //        {
        //            _model.StopPlanned = value;
        //            OnPropertyChanged("StopPlanned");
        //        }
        //    }
        //}

        public Brush KeyPressorStatusColor { get; private set; }

        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }


        public MainWindowVM(MainWindowModel model)
        {
            _model = model;
            _model.PropertyChanged += ModelPropertyChanged;
            StartCommand = new RelayCommand(param => this.StartCommandHandler());
            StopCommand = new RelayCommand(param => this.StopCommandHandler());
            KeyPressorStatusColor = Brushes.LightGray;
        }

        private void StartCommandHandler()
        {
            _model.StartKeyPressor();
        }

        private void StopCommandHandler()
        {
            _model.StopKeyPressor();
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName== "KeyPressorStatus")
            {
                UpdateKeyPressorStatus(_model.KeyPressorStatus);
            }
        }

        private void UpdateKeyPressorStatus(Constants.TaskStatus status)
        {
            switch (status)
            {
                case Constants.TaskStatus.Runned:
                    KeyPressorStatusColor = Brushes.LightGreen;
                    break;
                case Constants.TaskStatus.Stopped:
                    KeyPressorStatusColor = Brushes.Orange;
                    break;
                case Constants.TaskStatus.Faulted:
                    KeyPressorStatusColor = Brushes.Red;
                    break;
                default:
                    KeyPressorStatusColor = Brushes.LightGray;
                    break;
            }
            OnPropertyChanged("KeyPressorStatusColor");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
