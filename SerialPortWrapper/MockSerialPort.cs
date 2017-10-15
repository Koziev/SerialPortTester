using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    class MockSerialPort : ISerialPort
    {
        private bool is_open;
        private SerialPortParams port_params = new SerialPortParams { speed = 9600 };

        public event TrafficEventHandler event_handler;


        public SerialPortParams PortParams
        {
            get
            {
                // todo: запрос параметров физического порта
                return port_params;
            }

            private set { }
        }

        public void ApplyParams()
        {
            // nothing to do
        }

        private string port_name;

        public MockSerialPort(string port_name)
        {
            Contract.Ensures(!string.IsNullOrEmpty(this.port_name));
            this.port_name = port_name;
        }

        public string GetName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(port_name));
            return port_name;
        }

        public void Open()
        {
            Contract.Ensures(is_open);
            if (is_open)
            {
                throw new ApplicationException("Port is already open!");
            }

            is_open = true;
        }

        public bool IsOpen()
        {
            return is_open;
        }

        public void Close()
        {
            Contract.Ensures(!is_open);
            if (!is_open)
            {
                throw new ApplicationException("Port is not open!");
            }

            is_open = false;
        }


        public void SendData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (!is_open)
            {
                throw new ApplicationException("Port is not open!");
            }

            System.Threading.Thread.Sleep(10000);

            event_handler.Invoke(GetName(),DateTime.Now,TrafficDirection.Sent,data);

            System.Threading.Thread.Sleep(5000);

            event_handler.Invoke(GetName(), DateTime.Now, TrafficDirection.Received, System.Text.Encoding.ASCII.GetBytes("fake response") );

            return;
        }
    }
}
