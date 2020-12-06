using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Connection
{
    class ToClientFlag
    {
        public static byte[] PONG_FLAG => new byte[] { 3 };
        public static byte[] COPY_FLAG => new byte[] { 4 };
    }
}
