using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DontSleepWPF
{
    internal class StopTimeChecker
    {
        private DateTime _stopTime;
        private bool _isStarted;

        public event Action<Constants.TaskStatus> TaskStatusUpdated;

        public StopTimeChecker()
        {
            _isStarted = false;
            OnTaskStatusUpdated(Constants.TaskStatus.Stopped);
        }

        public void Start(DateTime stopTime)
        {
            _stopTime = stopTime;

            if (_isStarted)
                return;
            if (DateTime.Now > _stopTime)
                OnTaskStatusUpdated(Constants.TaskStatus.Faulted);

            _isStarted = true;
            try
            {
                var task = Task.Factory.StartNew(() => TaskProccess());                
                OnTaskStatusUpdated(Constants.TaskStatus.Runned);
            }
            catch (Exception e)
            {
                OnTaskStatusUpdated(Constants.TaskStatus.Faulted);
            }
        }

        public void Stop()
        {
            if (!_isStarted)
                return;
            _isStarted = false;
            OnTaskStatusUpdated(Constants.TaskStatus.Stopped);
        }

        private void TaskProccess()
        {
            try
            {
                while (_isStarted)
                {
                    if (DateTime.Now > _stopTime)
                    {
                        OnTaskStatusUpdated(Constants.TaskStatus.Finished);
                        _isStarted = false;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception e)
            {
                OnTaskStatusUpdated(Constants.TaskStatus.Faulted);
            }
        }

        private void OnTaskStatusUpdated(Constants.TaskStatus status)
        {
            TaskStatusUpdated?.Invoke(status);
        }
    }
}
