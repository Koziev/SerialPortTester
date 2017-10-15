using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    public interface ISerialPort
    {
        string GetName();

        SerialPortParams PortParams { get; }

        void ApplyParams();

        void Open();
        bool IsOpen();
        void Close();

        void SendData(byte[] data);
    }
}
