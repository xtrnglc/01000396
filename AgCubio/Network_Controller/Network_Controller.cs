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
        Socket static Connect_To_Server(string hostname)
        {
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo = Dns.Resolve(hostname);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                // Send test data to the remote device.
                Send(client, "This is a test<EOF>");
                sendDone.WaitOne();

                // Receive the response from the remote device.
                Receive(client);
                receiveDone.WaitOne();

                // Write the response to the console.
                Console.WriteLine("Response received : {0}", response);

                // Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
