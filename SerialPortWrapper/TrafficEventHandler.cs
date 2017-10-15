using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    public enum TrafficDirection { Sent, Received }
    public delegate void TrafficEventHandler(string port_name, DateTime event_time, TrafficDirection direction, byte[] data );
}
