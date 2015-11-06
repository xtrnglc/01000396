//Adam Sorensen and Trung Le
//CS 3500 PS7: AgCubio
//Nov 5th 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Network_Controller
{

    public class State
    {
        /// <summary>
        /// Client socket
        /// </summary>
        public Socket s = null;
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
    }

    public static class Network
    {


        private const int port = 11000;

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        Socket static Connect_To_Server()
        {

        }

        static void Connected_to_Server()
        {

        }

        static void RecieveCallback (IAsyncResult state_in_an_ar_object)
        {

        }

        static void i_want_more_data()
        {

        }

        static void Send (Socket socket, String data)
        {

        }

        static void SendCallBack()
        {

        }
    }
}
