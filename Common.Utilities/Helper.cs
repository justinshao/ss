using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Common.Utilities
{
    public class Helper {
        // only support IPv4 protocol
        public static long IpToLong(string strIp) {
            IPAddress ip = IPAddress.Parse(strIp);
            int x = 3;
            long o = 0;
            foreach (byte f in ip.GetAddressBytes()) {
                o += (long)f << 8 * x--;
            }
            return o;
        }
        // only support IPv4 protocol
        public static string LongToIp(long l) {
            var b = new byte[4];
            for (int i = 0; i < 4; i++) {
                b[3 - i] = (byte)(l >> 8 * i & 255);
            }
            return new IPAddress(b).ToString();
        }

    }
}
