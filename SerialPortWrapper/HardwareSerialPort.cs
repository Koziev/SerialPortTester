using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    class HardwareSerialPort : ISerialPort
    {
        private SerialPortParams port_params = new SerialPortParams { speed=9600 };

        public SerialPortParams PortParams
        {
            get
            {
                // todo запрос параметров физического порта
                throw new NotImplementedException();
            }

            private set
            {
                throw new NotImplementedException();
            }
        }

        public void ApplyParams()
        {
            // установка параметров физического порта
        }


        private string port_name;

        public HardwareSerialPort( string port_name )
        {
            Contract.Ensures(!string.IsNullOrEmpty(this.port_name));
            this.port_name = port_name;
        }

        public string GetName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(port_name));
            return port_name;
        }

        public void SendData(byte[] data)
        {
            if( data==null)
            {
                throw new ArgumentNullException("data");
            }

            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public bool IsOpen()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

    }
}
