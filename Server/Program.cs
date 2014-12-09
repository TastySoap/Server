using System;
using System.Net;
using System.Net.Sockets;

namespace TastySoap{
    class ReceiveViewingServer : AsyncServer {
        public ReceiveViewingServer(IPEndPoint ipep, int maxConnectionNumber, int packageSize):
            base(ipep, maxConnectionNumber, packageSize){}

        public override void OnReceiveCompleted(AsyncToken token) {
            
            System.Console.WriteLine(
                "Server.OnReceiveCompleted: {0} said: {1}", 
                token.Connection.RemoteEndPoint.ToString(),
                System.Text.Encoding.UTF8.GetString(token.Stack.ToArray())
            );
            base.OnReceiveCompleted(token);
        }
    }
    class LoggingServer : ReceiveViewingServer{
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
        public override void OnAcceptCompleted(object sender, SocketAsyncEventArgs args){
            base.OnAcceptCompleted(sender, args);
            Console.WriteLine("Server.OnAcceptCompleted; Remote EndPoint: {0}", 
                              args.AcceptSocket.RemoteEndPoint.ToString());
        }
        public override void ProcessReceive(SocketAsyncEventArgs args) {
            Console.WriteLine("Server.ProcessReceive; Remote EndPoint: {0}", args.AcceptSocket.RemoteEndPoint.ToString());
            base.ProcessReceive(args);
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
