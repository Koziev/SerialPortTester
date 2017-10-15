using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    public class MockSerialPorts : ISerialPorts
    {
        private List<ISerialPort> ports;
        public MockSerialPorts()
        {
            ports = new List<ISerialPort>();
            ports.Add(new MockSerialPort("COM1"));
            ports.Add(new MockSerialPort("COM2"));
        }


        public IEnumerable<string> GetAvailablePortNames()
        {
            Contract.Invariant(ports != null);
            return ports.Select(z => z.GetName());
        }

        public ISerialPort GetPort(string name)
        {
            Contract.Invariant(ports != null);
            var selected_port = ports.Where(z => z.GetName() == name).FirstOrDefault();
            if (selected_port == null)
            {
                throw new KeyNotFoundException("name");
            }

            return selected_port;
        }

        public IPortListener GetPortListener()
        {
            return new MockPortListener();
        }
    }
}
