using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace TastySoap {

    /// <summary>
    /// The represenation of an asynchronous server.
    /// </summary>
    public class AsyncServer : IAsyncServer{
        /// <summary>
        /// Prealocated pool of async socket operations.
        /// </summary>
        private Stack<SocketAsyncEventArgs> pool;
        /// <summary>
        /// Port of the server.
        /// </summary>
        public int Port{ get; set; }
        /// <summary>
        /// IP endpoint of the server.
        /// </summary>
        public IPEndPoint IPEP{ get; set; }
        //TODO: create Package class and use it here
        /// <summary>
        /// Package size used both for reciving and sending
        /// </summary>
        public int PackageSize{ get; private set; }
        /// <summary>
        /// Maximal number of connections
        /// </summary>
        public int MaximalConnectionsCount{ get; private set; }
        /// <summary>
        /// Actual number of connections.
        /// </summary>
        private int conectionsCount;
        /// <summary>
        /// Socket for listening.
        /// </summary>
        private Socket listenSocket;
        /// <summary>
        /// shh... its a semaphore...
        /// </summary>
        Semaphore maxNumberAcceptedClients;

        /// <summary>
        /// Constructor of the server. Inits values and prealocates the pool.
        /// </summary>
        /// <param name="ipep">Server endpoint.</param>
        /// <param name="port">Server port.</param>
        /// <param name="maxConnectionCount">Maximal number of connections; Used for prealocated pool.</param>
        /// <param name="packageSize">Buffer size for both reciving and sending</param>
        public AsyncServer(IPEndPoint ipep, int port, int maxConnectionCount, int packageSize){
            Port = port;
            IPEP = ipep;
            MaximalConnectionsCount = maxConnectionCount;
            preparePool(maxConnectionCount, packageSize);
            maxNumberAcceptedClients = new Semaphore(MaximalConnectionsCount, MaximalConnectionsCount);
        }

        /// <summary>
        /// Internal method for pool preparation process.
        /// This method prealocates pool.
        /// </summary>
        /// <param name="maxConnectionCount">Maximal number of connections</param>
        /// <param name="bufferSize">Size of a buffer.</param>
        private void preparePool(int maxConnectionCount, int bufferSize){
            pool = new Stack<SocketAsyncEventArgs>(maxConnectionCount);
            for(int i = 0; i < bufferSize; ++i){
                SocketAsyncEventArgs ioEventArg = new SocketAsyncEventArgs();
                ioEventArg.Completed += OnIOFinished;
                ioEventArg.SetBuffer(new byte[bufferSize], 0, bufferSize);
                pool.Push(ioEventArg);
            }
        }

        /// <summary>
        /// Event called after finishing of both sending and reciving process.
        /// </summary>
        /// <param name="sender">sender of this event</param>
        /// <param name="args">Socket operation</param>
        public void OnIOFinished(object sender, SocketAsyncEventArgs args) {
        }

        /// <summary>
        /// This method starts the server.
        /// It prepares the listening socket and prepares (and starts) the acceptation process.
        /// </summary>
        public void Start(){
            prepareListenSocket();
            prepareAndStartAcceptationProcess();
        }

        /// <summary>
        /// This method prepares the listeninig socket by creating and passing needed informations.
        /// </summary>
        protected void prepareListenSocket(){
            listenSocket = new Socket(IPEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.ReceiveBufferSize = PackageSize;
            listenSocket.SendBufferSize = PackageSize;
            listenSocket.Bind(IPEP);
            listenSocket.Listen(MaximalConnectionsCount);
        }

        /// <summary>
        /// Pepares and starts the acceptation process.
        /// It adds "OnAcceptCompleted" event for the brand new SocketAsyncEventArgs and starts Accepting by passing that object.
        /// </summary>
        protected void prepareAndStartAcceptationProcess(){
            var args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            Accept(args);
        }

        /// <summary>
        /// Starts the acceptation process.
        /// </summary>
        /// <param name="args"></param>
        public void Accept(SocketAsyncEventArgs args){
            args.AcceptSocket = null;
            maxNumberAcceptedClients.WaitOne();
            if(!listenSocket.AcceptAsync(args))
                ProcessAccept(args);
        }

        public void ProcessAccept(SocketAsyncEventArgs args){
            Interlocked.Increment(ref conectionsCount);
            var readEventArgs = pool.Pop();
            var socket = args.AcceptSocket;
            readEventArgs.UserToken = new AsyncToken(socket, PackageSize);
            if(!socket.ReceiveAsync(readEventArgs))
                processReceive(readEventArgs);
        }

        public void processReceive(SocketAsyncEventArgs args){
        }

        public void OnAcceptCompleted(object sender, SocketAsyncEventArgs args){
        }
    }
}
