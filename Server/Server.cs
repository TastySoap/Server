using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Collections.Generic;

// TODO: Replace all ...EventArgs and objects with specific classes
// TODO: Security! (System.Net.Security or openSSL)
// TODO: More xml docs.
namespace TastySoap{
    /// <summary>
    /// Makes object being able to be started.
    /// </summary>
    public interface IStartable{
        void Start();
    }

    /// <summary>
    /// Makes object being able to be stopped.
    /// </summary>
    public interface IStoppable{
        void Stop();
    }

    /// <summary>
    /// Makes object being able to be both started and stopped.
    /// </summary>
    /// <seealso cref="TastySoap.IStartable"/>
    /// <seealso cref="TastySoap.IStoppable"/>
    public interface IRunnable: IStartable, IStoppable{}

    /// <summary>
    /// Makes object being able to do non-blocking reciving.
    /// </summary>
    public interface IAsyncSocketReceiver{
        void ProcessReceive(SocketAsyncEventArgs args);
    }

    ///// <summary>
    ///// Makes object being able to do non-blocking sending.
    ///// </summary>
    //public interface IAsyncSocketSender{
    //    void ProcessSend(SocketAsyncEventArgs args);
    //}

    /// <summary>
    /// Makes object being able to asynchronously accept connections 
    /// and make actions at the end of accepting process.
    /// </summary>
    public interface IAsyncSocketAcceptor{
        // TODO: Connection class
        void Accept(SocketAsyncEventArgs args);
        void ProcessAccept(SocketAsyncEventArgs args);
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args);
    }

    public interface IAsyncErrorHandler{
        void processError(SocketAsyncEventArgs args);
    }

    /// <summary>
    /// Interface of asynchronous server.
    /// </summary>
    /// <seealso cref="TastySoap.IRunnable"/>
    /// <seealso cref="TastySoap.IAsyncSocketAcceptor"/>
    /// <seealso cref="TastySoap.IAsyncSocketReciver"/>
    /// <seealso cref="TastySoap.IAsyncSocketSender"/>
    public interface IAsyncServer: 
        IRunnable,
        IAsyncSocketAcceptor,
        IAsyncSocketReceiver,
        //IAsyncSocketSender,
        IAsyncErrorHandler
    {
        void OnIOFinished(object sender, SocketAsyncEventArgs args);
        void CloseClientConnection(SocketAsyncEventArgs args);
        void takeAction(AsyncToken token);
        int Port{ get; set; }
        IPEndPoint IPEP{ get; set; }
    }
}
