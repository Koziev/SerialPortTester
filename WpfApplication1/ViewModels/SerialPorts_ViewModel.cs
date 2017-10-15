using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace WpfApplication1
{
    class SerialPorts_ViewModel : INotifyPropertyChanged
    {
        private SerialPortWrapper.ISerialPorts ports;
        private SerialPortWrapper.IPortListener listener;

        // фоновый воркер для отправки данных в порт
        private readonly BackgroundWorker worker = new BackgroundWorker();


        class SendDataParams
        {
            public SerialPortWrapper.ISerialPort port;
            public byte[] data;
        }


        class PortEvent
        {
            public string port_name;
            public DateTime event_time;
            public SerialPortWrapper.TrafficDirection direction;
            public byte[] data;

            public override string ToString()
            {
                return $"{port_name} {event_time} {direction} {BitConverter.ToString(data)}";
            }
        }

        // очередь для накопления событий в текущем порте
        private System.Collections.Concurrent.ConcurrentQueue<PortEvent> port_events_queue;

        private ObservableCollection<string> traffic_events = new ObservableCollection<string>();
        public ObservableCollection<string> TrafficEvents
        {
            get
            {
                return traffic_events;
            }
        }

        public void RefreshTraffic()
        {
            PortEvent e;
            if (port_events_queue.TryDequeue(out e))
            {
                TrafficEvents.Add(e.ToString());
            }
        }

        public SerialPorts_ViewModel(SerialPortWrapper.ISerialPorts ports)
        {
            this.ports = ports;
            listener = ports.GetPortListener();

            SerialPortNames = new ReadOnlyObservableCollection<string>(new ObservableCollection<string>(this.ports.GetAvailablePortNames()));

            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.DoWork += worker_DoWork;

            port_events_queue = new ConcurrentQueue<PortEvent>();
            listener.event_handler += OnPortEvent;
        }

        public ReadOnlyObservableCollection<string> SerialPortNames { get; private set; }
        private ObservableCollection<string> speeds = new ObservableCollection<string>();

        private string selected_port_name;
        private int selected_port_speed;
        private int number_of_bits;
        private bool check_parity;
        private int number_of_stopbits;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string SelectedPortName
        {
            get
            {
                return selected_port_name;
            }

            set
            {
                if (value != selected_port_name)
                {
                    selected_port_name = value;
                    NotifyPropertyChanged();

                    IsPortSelected = true;

                    var selected_port_params = ports.GetPort(selected_port_name).PortParams;
                    PortSpeed = selected_port_params.speed.ToString();
                    Speeds = new ObservableCollection<string>(selected_port_params.valid_speeds.Select(z => z.ToString()));
                    NumberOfBits = selected_port_params.number_of_bits;
                    CheckParity = selected_port_params.check_parity;
                    NumberOfStopBits = selected_port_params.number_of_stopbits;

                    listener.SetPort(ports.GetPort(selected_port_name));

                }
            }
        }

        public bool IsPortSelected
        {
            get { return !string.IsNullOrEmpty(selected_port_name); }
            private set { NotifyPropertyChanged(); }
        }

        public string PortSpeed
        {
            get
            {
                return selected_port_speed.ToString();
            }

            set
            {
                if (value != selected_port_speed.ToString())
                {
                    selected_port_speed = int.Parse(value);
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Speeds
        {
            get { return speeds; }
            set
            {
                speeds.Clear();
                foreach (var x in value)
                {
                    speeds.Add(x);
                }

                NotifyPropertyChanged();
            }
        }

        public int NumberOfBits
        {
            get
            {
                return number_of_bits;
            }

            set
            {
                if (value != number_of_bits)
                {
                    number_of_bits = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool? CheckParity
        {
            get
            {
                return check_parity;
            }

            set
            {
                if (value != check_parity)
                {
                    check_parity = value.Value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int NumberOfStopBits
        {
            get
            {
                return number_of_stopbits;
            }

            set
            {
                if (value != number_of_stopbits)
                {
                    number_of_stopbits = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool is_sending_available = true;
        public bool IsSendingAvailable
        {
            get
            {
                return is_sending_available;
            }

            set
            {
                if (value != is_sending_available)
                {
                    is_sending_available = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void ApplyPortSettings()
        {
            ports.GetPort(SelectedPortName).ApplyParams();
        }

        public void SendData(byte[] data_to_send)
        {
            IsSendingAvailable = false;
            worker.RunWorkerAsync(new SendDataParams { port = ports.GetPort(SelectedPortName), data = data_to_send });
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            SendDataParams p = (SendDataParams)e.Argument;
            try
            {
                p.port.Open();
                p.port.SendData(p.data);
            }
            finally
            {
                p.port.Close();
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            IsSendingAvailable = true;
        }

        private void OnPortEvent(string port_name, DateTime event_time, SerialPortWrapper.TrafficDirection direction, byte[] data)
        {
            port_events_queue.Enqueue(new PortEvent { port_name = port_name, event_time = event_time, direction = direction, data = data });
        }
    }
}
