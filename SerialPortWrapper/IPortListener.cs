using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    public interface IPortListener
    {
        void SetPort(ISerialPort port);

        event TrafficEventHandler event_handler;
    }
}
