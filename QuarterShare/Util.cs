using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare
{
    class Util
    {

        public static byte[] TwoByteIndictor(int val)
        {
            byte[] intBytes = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            byte[] result = intBytes;
            int len = result.Length;
            byte[] rtn = { result[len - 1], result[len - 2] };

            return rtn;
        }

        public static IPAddress GetLocalNetworkIp()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.MapToIPv4().ToString() == x.ToString()).First();
        }
    }
}
