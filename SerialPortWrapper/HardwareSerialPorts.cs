using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    public class HardwareSerialPorts : ISerialPorts
    {
        private bool initialized;
        private List<HardwareSerialPort> ports;
        public HardwareSerialPorts()
        {
            ports = new List<HardwareSerialPort>();
        }

        private void InitializeFromHardware()
        {
            Contract.Requires(ports != null);
            Contract.Ensures(initialized);

            if (!initialized)
            {
                initialized = true;
                foreach (string port_name in System.IO.Ports.SerialPort.GetPortNames())
                {
                    ports.Add(new HardwareSerialPort(port_name));
                }
            }
        }


        public IEnumerable<string> GetAvailablePortNames()
        {
            Contract.Invariant(ports != null);
            InitializeFromHardware();
            return ports.Select(z => z.GetName());
        }

        public ISerialPort GetPort(string name)
        {
            throw new NotImplementedException();
        }

        public IPortListener GetPortListener()
        {
            throw new NotImplementedException();
        }

    }
}
