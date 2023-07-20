using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DontSleepWPF
{
    internal class Bootstarpper
    {
        internal void Run()
        {
            AppSettings.Init();
            InitMainWindow();
        }

        protected void InitMainWindow()
        {
            var model = new Model.MainWindowModel();
            var mainWindow = new View.MainWindow();
            mainWindow.DataContext = new ViewModel.MainWindowVM(model);
            mainWindow.Show();
        }
    }
}
