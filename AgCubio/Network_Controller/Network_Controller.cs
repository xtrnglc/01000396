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
using Newtonsoft.Json;
using CustomNetworking;

namespace AgCubio
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

    public static class Network
    {
        // The socket used to communicate with the server.  If no connection has been
        // made yet, this is null.
        private static Socket socket = null;
        private static string response = string.Empty;
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);


        /// <summary>
        /// tries to connect to a server with a given hostname
        /// </summary>
        /// <param name="callback"></param>
        /// <param name=""></param>
        /// <param name="hostname"></param>
        /// <returns></returns>
        public static Socket Connect_to_Server(string hostname)
        {
            int port = 11000;
            if (socket == null)
            {
                TcpClient client = new TcpClient(hostname, port);
                //socket = new StringSocket(client.Client, UTF8Encoding.Default);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                socket.BeginSend(hostname + "\n", (e, p) => { }, null);
                socket.BeginReceive(LineReceived, null);
            }
            return null;
        }

        /// <summary>
        /// Checks to see if connected to server?
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        public static void Connected_to_Server(IAsyncResult state_in_an_ar_object)
        {

        }

        /// <summary>
        /// Called by the OS, does NOT request new data
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        public static void ReceiveCallback(IAsyncResult state_in_an_ar_object)
        {
            try
            {
                State state = (State)state_in_an_ar_object;
                Socket s = state.workSocket;

                int bytes = s.EndReceive(state_in_an_ar_object);

                if (bytes > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytes));
                }

                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    receiveDone.Set();
                }
            }
            catch (Exception)
            {
                
            }
        }
            

        /// <summary>
        /// helper method that will be called by the view to request new data 
        /// </summary>
        /// <param name="state"></param>
        public static void i_want_more_data(State state)
        {

        }

        /// <summary>
        /// allows the program to send data over a socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static void Send (Socket socket, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallBack), socket);
        }

        /// <summary>
        /// Helper method for the Send method
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        public static void SendCallBack(IAsyncResult state_in_an_ar_object)
        {
            try
            {
                Socket s = (Socket)state_in_an_ar_object;

                int bytesSent = s.EndSend(state_in_an_ar_object);
                sendDone.Set();
            }
            catch(Exception)
            {

            }
        }

        /// <summary>
        /// Assists the send function, should write a console write to know when data goes out
        /// </summary>
        private static void SendCallBack()
        {

        }
            




        //
        // PAY ATTENTION: this is one of the most interesting features in the program!
        // Register for this event to be notified when a line of text arrives.
        public static event Action<String> IncomingLineEvent;

        /// <summary>
        /// Connect to the server at the given hostname and port and with the give name.
        /// </summary>
        public static void Connect(string hostname, int port, String name)
        {
            if (socket == null)
            {
                TcpClient client = new TcpClient(hostname, port);
                socket = new StringSocket(client.Client, UTF8Encoding.Default);
                socket.BeginSend(name + "\n", (e, p) => { }, null);
                socket.BeginReceive(LineReceived, null);
                
            }
        }

        /// <summary>
        /// Send a line of text to the server.
        /// </summary>
        /// <param name="line"></param>
        public static void SendMessage(String line)
        {
            if (socket != null)
            {
                socket.BeginSend(line + "\n", (e, p) => { }, null);
            }
        }

        /// <summary>
        /// Deal with an arriving line of text.
        /// </summary>
        private static void LineReceived(String s, Exception e, object p)
        {
            if (IncomingLineEvent != null)
            {
                IncomingLineEvent(s);
            }
            socket.BeginReceive(LineReceived, null);
            
        }        
    }
}
