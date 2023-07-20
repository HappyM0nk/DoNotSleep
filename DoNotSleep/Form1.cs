using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DontSleep
{
    public partial class Form1 : Form
    {
        private KeyPressor keyPressor;

        public Form1()
        {
            InitializeComponent();
            keyPressor = new KeyPressor();
            keyPressor.TaskStatusUpdated += UpdateStatus;
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            keyPressor.Start();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            keyPressor.Stop();
        }

        private void UpdateStatus(Constants.TaskStatus status)
        {
            switch (status)
            {
                case Constants.TaskStatus.Runned:
                    statusPanel.BackColor = Color.LightGreen;
                    break;
                case Constants.TaskStatus.Stopped:
                    statusPanel.BackColor = Color.Orange;
                    break;
                case Constants.TaskStatus.Faulted:
                    statusPanel.BackColor = Color.Red;
                    break;
                default:
                    statusPanel.BackColor = Color.Gray;
                    break;
            }
        }
    }
}
