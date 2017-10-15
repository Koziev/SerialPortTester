using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    public interface ISerialPorts
    {
        /// <summary>
        /// Получение списка имен доступных портов
        /// </summary>
        /// <returns>перечислитель для доступных имен</returns>
        IEnumerable<string> GetAvailablePortNames();

        /// <summary>
        /// Получение контроллера порта по его имени
        /// </summary>
        /// <param name="name">имя порта</param>
        /// <returns>экземпляр контроллера</returns>
        ISerialPort GetPort(string name);

        IPortListener GetPortListener();
    }
}
