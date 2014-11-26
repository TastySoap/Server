using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;

namespace TastySoap{
    interface IStartable{
        void Start();
    }

    interface IStoppable{
        void Stop();
    }

    interface IAsyncServer: 
        IStartable,
        IStoppable
    {
        void AcceptRequest(SocketAsyncEventArgs e);
        void OnAcceptRequestFinished(object sender, SocketAsyncEventArgs e);
        void ProcessAccept(SocketAsyncEventArgs e);
        void OnIOFinished(object sender, SocketAsyncEventArgs e);
        void ProcessRecive(SocketAsyncEventArgs e);
        void ProcessSend(SocketAsyncEventArgs e);
        void CloseClientConnection(SocketAsyncEventArgs e);
    }

    class AsyncServer : IAsyncServer{
    }
}
