using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Helper
{
    public static class IPAddressExtension
    {
        public static long IpToInt(this IPAddress iPAddress)
        {
            var items = iPAddress.GetAddressBytes();
            return (long)items[0] << 24 | (long)items[1] << 16 | (long)items[2] << 8 | (long)items[3];
        }
        public static IPAddress IntToIp(long ipInt)
        {
            string result = $"{(ipInt >> 24) & 0xFF}.{(ipInt >> 16) & 0xFF}.{(ipInt >> 8) & 0xFF}.{ipInt & 0xFF}";
            return IPAddress.Parse(result);
        }
        public static long IpToInt2(this IPAddress iPAddress)
        {
            return (long)(uint)IPAddress.NetworkToHostOrder((int)iPAddress.Address);
        }
        public static IPAddress IntToIp2(long address)
        {
            return IPAddress.Parse(address.ToString());
        }
    }
}
