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
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static Action<State> connectionCallbackTemp;
        private static TcpListener server;

        // One StringSocket per connected client
        private static List<StringSocket> allSockets;

        // the name associated with the socket
        private static List<string> user_names;


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

            //TcpClient client = new TcpClient(hostname, port);

            state.workSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            state.workSocket.BeginConnect(hostname, port, new AsyncCallback(Connected_to_Server), state);


            return socket;
        }

        /// <summary>
        /// Network code that begins receiving data and calls call back to send player name
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        public static void Connected_to_Server(IAsyncResult state_in_an_ar_object)
        {
            State state = (State)state_in_an_ar_object.AsyncState;
            state.connectionCallback(state);                //Calls the callback method from View and sends player name

            state.workSocket.BeginReceive(state.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        /// <summary>
        /// Network code to receive data and call callback method to add cubes to world
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        public static void ReceiveCallback(IAsyncResult state_in_an_ar_object)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                State state = (State)state_in_an_ar_object.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(state_in_an_ar_object);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    state.connectionCallback(state);                //Draws player cube
                }
                else
                {
                    i_want_more_data(state);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        
        /// <summary>
        /// Network code to request more data from server
        /// </summary>
        /// <param name="s"></param>
        public static void i_want_more_data(State s)
        {
            s.workSocket.BeginReceive(s.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReceiveCallback), s);
        }

        /// <summary>
        /// Network code to send data to server
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
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
                Socket s = (Socket)state_in_an_ar_object.AsyncState;

                int bytesSent = s.EndSend(state_in_an_ar_object);
                sendDone.Set();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// OS will listen for a connection and save the callback function with the request
        /// when a connection request comes, should invoke the Accept_a_New_Client method
        /// This is an event loop, needs to set up a new connection listener for another connection
        /// 
        /// </summary>
        public static void Server_Awaiting_Client_Loop(Action<State> callback)
        {
            IPHostEntry ipHostInfo = Dns.Resolve("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            State state = new State();
            connectionCallbackTemp = callback;
            //server = new TcpListener(IPAddress.Any, 11000);
            //allSockets = new List<StringSocket>();
            //user_names = new List<string>();
            //server.Start();
            //server.BeginAcceptSocket(Accept_a_New_Client, null);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //state.connectionCallback(state);
            state.connectionCallback = callback;
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while(true)
                {
                    allDone.Reset();
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(Accept_a_New_Client), listener);
                    allDone.WaitOne();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Create a new socket
        /// Call the callback provided by the above method
        /// Await a new connection request
        /// Networking code should not start listening for data, game server should do that
        /// </summary>
        public static void Accept_a_New_Client(IAsyncResult ar)
        {
            //Console.WriteLine("here");
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            //listener.BeginAccept(Accept_a_New_Client, listener);

            State state = new State();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            //Console.WriteLine("here");
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            State state = (State)ar.AsyncState;
            Socket handler = state.workSocket;
            state.connectionCallback = connectionCallbackTemp;
            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                state.connectionCallback(state);
                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                    // Echo the data back to the client.
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }
    }
}
















/*


 try
        {
            State state = (State)state_in_an_ar_object.AsyncState;
            Socket s = state.workSocket;

            int bytes = s.EndReceive(state_in_an_ar_object);

            if (bytes > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytes));

                Console.Write(state.sb.ToString());
            }

            else
            {
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                    Console.Write(response);
                }
                receiveDone.Set();
            }
        }
        catch (Exception)
        {

        }
*/
