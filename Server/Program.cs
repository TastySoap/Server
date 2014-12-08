using System;
using System.Net;
using System.Net.Sockets;

namespace TastySoap{
    class LoggingServer : AsyncServer{
        public LoggingServer(IPEndPoint ipep, int maxConnectionNumber, int packageSize): 
            base(ipep, maxConnectionNumber, packageSize){}

        public override void Start(){
            Console.WriteLine("Server.Start;");
            base.Start();
        }
        public override void Accept(SocketAsyncEventArgs args){
            Console.WriteLine("Server.Accept;");
            base.Accept(args);
        }
        public override void ProcessAccept(SocketAsyncEventArgs args){

            Console.WriteLine("Server.ProcessAccept;");
            base.ProcessAccept(args);
            Console.WriteLine("Remote EndPoint: {0}", args.AcceptSocket.RemoteEndPoint.ToString());
        }
        public override void Stop(){
            Console.WriteLine("Server.Stop;");
            base.Stop();
        }
    }
    class Program{
        static void Main(string[] args){
            var addr = Dns.GetHostEntry("127.0.0.1").AddressList[0];
            var port = 1337;
            var ipEndpoint = new IPEndPoint(addr, port);

            var server = new LoggingServer(ipEndpoint, 1, 1024);

            server.Start();
 
            while(true)
                ;
        }
    }
}
