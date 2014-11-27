using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;

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
        public void ProcessRecive(SocketAsyncEventArgs e);
    }

    /// <summary>
    /// Makes object being able to do non-blocking sending.
    /// </summary>
    public interface IAsyncSocketSender{
        public void ProcessSend(SocketAsyncEventArgs e);
    }

    /// <summary>
    /// Makes object being able to asynchronously accept connections 
    /// and make actions at the end of accepting process.
    /// </summary>
    public interface IAsyncSocketAcceptor{
        // TODO: Connection class
        public void Accept(SocketAsyncEventArgs e);
        protected void OnAcceptRequestCompleted(object sender, SocketAsyncEventArgs e);
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
        protected void OnIOFinished(object sender, SocketAsyncEventArgs e);
        public void CloseClientConnection(SocketAsyncEventArgs e);
    }

    /// <summary>
    /// The represenation of an asynchronous server.
    /// </summary>
    public class AsyncServer : IAsyncServer{
        public AsyncServer(IPEndPoint localEndPoint, int port, int numberOfConnections, int ReciveBufferSize){

        }
    }
}
