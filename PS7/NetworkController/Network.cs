using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using CustomNetworking;

namespace NetworkController
{
    public static class Network
    {


        public class State
        {
            /// <summary>
            /// Client socket
            /// </summary>
            public Socket workSocket = null;
            /// <summary>
            /// Size of the buffer
            /// </summary>
            public const int BufferSize = 256;
            /// <summary>
            /// for the buffer
            /// </summary>
            public byte[] buffer = new byte[BufferSize];
            /// <summary>
            /// data string
            /// </summary>
            public StringBuilder sb = new StringBuilder();

            public delegate void CallBack();
        }



        // The socket used to communicate with the server.  If no connection has been
        // made yet, this is null.
        private static Socket socket;

        /// <summary>
        /// Size of the buffer
        /// </summary>
        public const int BufferSize = 256;
        /// <summary>
        /// for the buffer
        /// </summary>
        public static byte[] buffer = new byte[BufferSize];
        /// <summary>
        /// data string
        /// </summary>

        public static Socket Connect_to_Server(Delegate callback, String hostname)
        {

            int port = 11000;
            if (socket == null)
            {
                TcpClient client = new TcpClient(hostname, port);
        
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                
            }
            return null;
        }

        public static void Connected_to_Server(IAsyncResult state_in_an_ar_object)
        {

        }

        public static void ReceiveCallback(IAsyncResult state_in_an_ar_object)
        {

        }

        public static void i_want_more_data(State s)
        {

        }

        public static void Send(Socket socket, String data)
        {

        }
    }
}
