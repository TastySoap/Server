using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;

// TODO: Replace all ...EventArgs and objects with specific classes
// TODO: Security! (System.Net.Security or openSSL)
namespace TastySoap{
    /// <summary>
    /// Makes object being able to be started.
    /// </summary>
    public interface IStartable{
        public void Start();
    }

    /// <summary>
    /// Makes object being able to be stopped.
    /// </summary>
    public interface IStoppable{
        public void Stop();
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
    public interface IAsyncSocketReciver{
        public void ProcessRecive(SocketAsyncEventArgs args);
        public int PackageSize{ get; private set; }
    }

    /// <summary>
    /// Makes object being able to do non-blocking sending.
    /// </summary>
    public interface IAsyncSocketSender{
        public void ProcessSend(SocketAsyncEventArgs args);
    }

    /// <summary>
    /// Makes object being able to asynchronously accept connections 
    /// and make actions at the end of accepting process.
    /// </summary>
    public interface IAsyncSocketAcceptor{
        // TODO: Connection class
        public void Accept(SocketAsyncEventArgs args);
        public void ProcessAccept(SocketAsyncEventArgs args);
        protected void OnAcceptCompleted(object sender, SocketAsyncEventArgs args);
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
        IAsyncSocketReciver,
        IAsyncSocketSender
    {
        protected void OnIOFinished(object sender, SocketAsyncEventArgs args);
        public void CloseClientConnection(SocketAsyncEventArgs args);
        public int Port{ get; set; }
        public IPEndPoint IPEP{ get; set; }
    }
}
