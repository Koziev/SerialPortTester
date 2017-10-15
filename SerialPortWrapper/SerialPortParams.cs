using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    public class SerialPortParams
    {
        public int speed { get; set; }

        public int number_of_bits { get { return 5; } }

        public bool check_parity {  get { return false; } }

        public int number_of_stopbits { get { return 1; } }

        public IEnumerable<int> valid_speeds { get { return new int[]{ 300, 600, 1200, 2400, 9600, 14400, 19200, 38400, 57600, 115200 }; } }
    }
}
