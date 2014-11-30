using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TastySoap{
    /// <summary>
    /// Helper class for asynchronous socket connection
    /// </summary>
    public class AsyncToken : IDisposable{
        /// <summary>
        /// The connection socket
        /// </summary>
        public Socket Connection{ get; private set; }
        /// <summary>
        /// Bytes
        /// </summary>
        public Stack<Byte> Stack{ get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection">Connection socket</param>
        /// <param name="bufferSize">buffer size for bytes stack</param>
        public AsyncToken(Socket connection, Int32 bufferSize){
            Connection = connection;
            Stack = new Stack<Byte>(bufferSize);
        }

        /// <summary>
        /// Adds data to Token.Stack(bytes)
        /// </summary>
        /// <param name="args"></param>
        public void addToStack(SocketAsyncEventArgs args){
            //TODO: Find a way to append to stack and use it.
            for(int i = args.Offset; i < args.Buffer.Length; ++i)
                Stack.Push(args.Buffer[i]);
        }

        /// <summary>
        /// Shutdowns and closes connection socket
        /// </summary>
        public void Dispose(){
            try{
                Connection.Shutdown(SocketShutdown.Send);
            } 
            catch(Exception){ /* Client closed socket? Who cares! */ } 
            finally {
                Connection.Close();
            }
        }
    }
}
