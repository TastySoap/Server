using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

//TODO: Xml docs.
namespace TastySoap{
    public class AsyncToken : IDisposable{
        public Socket Connection{ get; private set; }
        public Stack<Byte> Stack{ get; private set; }

        public AsyncToken(Socket connection, Int32 bufferSize){
            Connection = connection;
            Stack = new Stack<Byte>(bufferSize);
        }

        public void addToStack(SocketAsyncEventArgs args){
            //TODO: Find a way to append to stack and use it.
            for(int i = args.Offset; i < args.Buffer.Length; ++i)
                Stack.Push(args.Buffer[i]);
        }

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
