using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SerialPortWrapper.ISerialPorts ports;
        void App_Startup(object sender, StartupEventArgs e)
        {
            // ports = new SerialPortWrapper.HardwareSerialPorts();
            ports = new SerialPortWrapper.MockSerialPorts();
            MainWindow mainWindow = new MainWindow(ports);
            mainWindow.Show();
        }
    }
}
