using System;
using System.Net;
namespace TastySoap{
    class Program{
        static void Main(string[] args){
            var addr = Dns.GetHostEntry("127.0.0.1").AddressList[0];
            var port = 1337;
            var ipEndpoint = new IPEndPoint(addr, port);

            var server = new AsyncServer(ipEndpoint, 1, 1024);

            server.Start();

            while(true)
                ;
        }
    }
}
