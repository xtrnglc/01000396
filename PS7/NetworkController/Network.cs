/*
Author: Trung Le and Adam Sorensen
11/11/2015
CS 3500
PS7 - AgCubio
*/
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
        
        public Action<State> connectionCallback;
    }
    public static class Network
    {
        // The socket used to communicate with the server.  If no connection has been
        // made yet, this is null.
        private static Socket socket;
        //text that has been recieved from the client but not yet dealt with
        private static string incoming;
        //text that needs to be sent to the client but has not yet gone
        private static string outgoing;
        private static string response = string.Empty;
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);

        /// <summary>
        /// Size of the buffer
        /// </summary>
        public const int BufferSize = 256;
        /// <summary>
        /// or the buffer
        /// </summary>
        public static byte[] buffer = new byte[BufferSize];
        /// <summary>
        /// data string
        /// </summary>

        public static Socket Connect_to_Server(Action<State> callback, String hostname)
        {
            State state = new State();
            state.connectionCallback = callback;
            int port = 11000;
            
            TcpClient client = new TcpClient(hostname, port);
        
            state.workSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            state.workSocket.BeginConnect(hostname, port, new AsyncCallback(Connected_to_Server), state);
                
            return socket;
        }

        public static void Connected_to_Server(IAsyncResult state_in_an_ar_object)
        {
            State state = (State)state_in_an_ar_object.AsyncState;
            state.connectionCallback(state);
            
            state.workSocket.BeginReceive(state.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            
            //call receivecallback
        }

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

        public static void i_want_more_data(State s)
        {

        }

        public static void Send(Socket socket, String data)
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
            catch (Exception)
            {

            }
        }
    }
}
