using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    class MockPortListener : IPortListener
    {
        private MockSerialPort current_port;
        public void SetPort(ISerialPort port)
        {
            Contract.Ensures(current_port != null);

            if( port==null )
            {
                throw new ArgumentNullException("port");
            }

            if( current_port!=null)
            {
                current_port.event_handler -= DataTraffic;
            }

            current_port = (MockSerialPort)port;
            current_port.event_handler += DataTraffic;
        }

        private void DataTraffic( string port_name, DateTime event_time, TrafficDirection direction, byte[] data )
        {
            event_handler.Invoke(port_name, event_time, direction, data );
        }

        public event TrafficEventHandler event_handler;
    }
}
