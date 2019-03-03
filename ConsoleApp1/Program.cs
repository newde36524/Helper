using Helper;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static long ToInt(string addr)
        {
            // careful of sign extension: convert to uint first;
            // unsigned NetworkToHostOrder ought to be provided.
            return (long)(uint)IPAddress.NetworkToHostOrder(
            (int)IPAddress.Parse(addr).Address);
        }

        static string ToAddr(long address)
        {
            return IPAddress.Parse(address.ToString()).ToString();
            // This also works:
            // return new IPAddress((uint) IPAddress.HostToNetworkOrder(
            // (int) address)).ToString();
        }
        static void Main(string[] args)
        {
            //TestWhileTaskProxy();
            {
                //var ip = IPAddress.Parse("192.168.3.112");
                //var intIp = ip.IpToInt();
                //Console.WriteLine(intIp);
                //Console.WriteLine(IPAddressExtension.IntToIp(intIp));
                //Console.WriteLine("===================================");
            }
            {
                //TaskHelper.RunParallel(new System.Collections.Generic.List<Action>() {

                //},10);
            }
            {
                MyClass myClass = new MyClass
                {
                    Age = 10,
                    Name = "名字"
                };
                MyClassClone myClassClone = ObjectCloneHelper.TransExp<MyClass, MyClassClone>(myClass);
                Console.WriteLine($"{myClassClone.Age} {myClassClone.Name}");
            }
            Console.ReadLine();
        }

        public static void TestWhileTaskProxy()
        {
            WhileTaskProxy whileTaskProxy = new WhileTaskProxy();

            whileTaskProxy.Next(() =>
            {
                Console.WriteLine(1111);
            }, 100);

            Console.ReadLine();

            whileTaskProxy.Next(() =>
            {
                Console.WriteLine(2222);
            }, 100);

            Console.ReadLine();

            whileTaskProxy.Next(() =>
            {
                Console.WriteLine(3333);
            }, 100);

            Console.ReadLine();

            whileTaskProxy.Pause();

            Console.ReadLine();

            whileTaskProxy.Reume();

            Console.ReadLine();
        }

    }


    public class MyClass
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }

    public class MyClassClone
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
    }
}
