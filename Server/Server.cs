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

    interface IRunnable: IStartable, IStoppable{}

    interface IAsyncSocketReciver{
        void ProcessRecive(SocketAsyncEventArgs e);
    }

    interface IAsyncSocketSender{
        void ProcessSend(SocketAsyncEventArgs e);
    }

    interface IAsyncSocketAcceptor{
        void Accept(SocketAsyncEventArgs e);
        void OnAcceptRequestCompleted(object sender, SocketAsyncEventArgs e);
    }

    interface IAsyncServer: 
        IRunnable,
        IAsyncSocketAcceptor,
        IAsyncSocketReciver,
        IAsyncSocketSender
    {
        void OnIOFinished(object sender, SocketAsyncEventArgs e);
        void CloseClientConnection(SocketAsyncEventArgs e);
    }

    class AsyncServer : IAsyncServer{
    }
}
