using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace TastySoap {

    /// <summary>
    /// The represenation of an asynchronous server.
    /// </summary>
    public class AsyncServer : IAsyncServer {
        private Stack<SocketAsyncEventArgs> pool;
        public int Port { get; set; }
        public IPEndPoint IPEP { get; set; }
        public int ReciveBufferSize { get; private set; }
        public int MaximalConnectionsCount { get; private set; }

        private Socket listenSocket;

        public AsyncServer(IPEndPoint ipep, int port, int maxConnectionCount, int reciveBufferSize) {
            Port = port;
            IPEP = ipep;
            MaximalConnectionsCount = maxConnectionCount;
            preparePool(maxConnectionCount, reciveBufferSize);
        }

        private void preparePool(int maxConnectionCount, int reciveBufferSize) {
            pool = new Stack<SocketAsyncEventArgs>(maxConnectionCount);
            for(int i = 0; i < reciveBufferSize; ++i) {
                SocketAsyncEventArgs ioEventArg = new SocketAsyncEventArgs();
                ioEventArg.Completed += OnIOFinished;
                ioEventArg.SetBuffer(new byte[reciveBufferSize], 0, reciveBufferSize);
                pool.Push(ioEventArg);
            }
        }

        override protected void OnIOFinished(object sender, SocketAsyncEventArgs args) {
        }

        override public void Start() {
            prepareListenSocket();
            prepareAndStartAcceptationProcess();
        }

        protected void prepareListenSocket() {
            listenSocket = new Socket(IPEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.ReceiveBufferSize = ReciveBufferSize;
            listenSocket.SendBufferSize = ReciveBufferSize;
            listenSocket.Bind(IPEP);
            listenSocket.Listen(MaximalConnectionsCount);
        }

        protected void prepareAndStartAcceptationProcess() {
            var args = new SocketAsyncEventArgs();
            args.Completed += OnAcceptCompleted;
            Accept(args);
        }

        override public void Accept(SocketAsyncEventArgs args) {
            args.AcceptSocket = null;

        }

        override public void OnAcceptCompleted(object sender, SocketAsyncEventArgs args) {
        }
    }
}
