using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortWrapper
{
    public static class SPDataConverter
    {
        public static byte[] GetBytes( string str_data, bool is_hex )
        {
            if(is_hex)
            {
                if (str_data.Length % 2 != 0)
                {
                    throw new ArgumentException( $"Input string length {str_data.Length} is odd" );
                }

                byte[] HexAsBytes = new byte[str_data.Length / 2];
                for (int index = 0; index < HexAsBytes.Length; index++)
                {
                    string byteValue = str_data.Substring(index * 2, 2);
                    HexAsBytes[index] = byte.Parse(byteValue, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
                }

                return HexAsBytes;
            }
            else
            {
                return System.Text.Encoding.ASCII.GetBytes(str_data);
            }
        }
    }
}
