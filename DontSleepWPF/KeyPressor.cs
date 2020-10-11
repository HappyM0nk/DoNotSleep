using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace DontSleepWPF
{
    internal class KeyPressor
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        private const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        private const int VK_RCONTROL = 0xA3; //Right Control key code
        private const int VK_RETURN = 0x0D; //Enter key code

        private bool _isStarted;
        private int _timeout;

        public event Action<Constants.TaskStatus> TaskStatusUpdated;

        public KeyPressor(int timeout)
        {
            _timeout = timeout;
            _isStarted = false;
            OnTaskStatusUpdated(Constants.TaskStatus.Stopped);
        }

        public void Start()
        {
            if (_isStarted)
                return;
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

        public void UpdateTimeout(int timeout)
        {
            this._timeout = timeout;
        }

        private void TaskProccess()
        {
            try
            {
                while (_isStarted)
                {
                    keybd_event(VK_RCONTROL, 0, KEYEVENTF_EXTENDEDKEY, 0);
                    keybd_event(VK_RCONTROL, 0, KEYEVENTF_KEYUP, 0);
                    Thread.Sleep(_timeout);
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
