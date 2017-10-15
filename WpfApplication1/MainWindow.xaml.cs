using SerialPortWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ISerialPorts ports;

        System.Windows.Threading.DispatcherTimer traffic_refresh_timer;

        public MainWindow(ISerialPorts ports)
        {
            this.ports = ports;
            DataContext = new SerialPorts_ViewModel(ports);
            InitializeComponent();

            traffic_refresh_timer = new System.Windows.Threading.DispatcherTimer(interval: new TimeSpan(0, 0, 1), priority: System.Windows.Threading.DispatcherPriority.Background, callback: OnTrafficRefresh, dispatcher:System.Windows.Threading.Dispatcher.CurrentDispatcher);
        }

        private void OnTrafficRefresh( object sender, EventArgs arg )
        {
            ((SerialPorts_ViewModel)DataContext).RefreshTraffic();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ((SerialPorts_ViewModel)DataContext).ApplyPortSettings();
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            string data_str = tbData.Text;
            byte[] hex_bytes = SPDataConverter.GetBytes(data_str, rbHex.IsChecked.Value);
            ((SerialPorts_ViewModel)DataContext).SendData(hex_bytes);
        }
    }
}
