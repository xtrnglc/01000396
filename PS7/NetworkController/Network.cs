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
using System.Diagnostics;
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

        public Action<State> connectionCallback;                             //OH action states. Very interesting.

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
        private static List<StringSocket> allSockets;                              //We did not include this here. We put this in server and used socket.

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

        public static Socket Connect_to_Server(Action<State> callback_func, string hostname)
        {
            // Establish the remote endpoint for the socket
            try
            {
                // Establish the remote endpoint for the socket     
                IPAddress address;
                try
                {
                    address = Dns.GetHostEntry(hostname).AddressList[0];
                }
                catch (Exception)
                {
                    address = IPAddress.Parse(hostname);
                }
                IPEndPoint remoteEP = new IPEndPoint(address, 11000);

                // Create a TCP/IP socket
                Socket socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Saves callback function in a state object
                State state = new State();
                state.workSocket = socket;
                state.connectionCallback = callback_func;

                // Open socket and use BeginConnect method
                socket.BeginConnect(remoteEP, new AsyncCallback(Connected_to_Server), state);

                // Return the socket
                return socket;
            }
            // Catch any exceptions
            catch (Exception exception)
            {
                State state2 = new State();

                // Change error fields of state object accordingly
                callback_func(state2);
                
                // Print error message
                Console.WriteLine("ERROR: " + exception.ToString());

                // Socket that returns is null
                return state2.workSocket;
            }
        }

        /// <summary>
        /// This function is referenced by the BeginConnect method and is "called" by the OS when the socket connects to the 
        /// server. The "state_in_an_ar_object" object contains a field "AsyncState" which contains the "state" object saved away 
        /// in the above function. Once a connection is established the "saved away" callback function needs to called. Additionally, 
        /// the network connection should "BeginReceive" expecting more data to arrive(and provide the ReceiveCallback function for 
        /// this purpose)
        /// </summary>
        /// <param name="state_in_an_ar_object">Contains a field "AyncState" which contains the "state" object saved away in Connect_To_Server</param>
        public static void Connected_to_Server(IAsyncResult state_in_an_ar_object)
        {
            // Save AsyncState of state_in_an_ar_object into current state
            State currentAsyncState = (State)state_in_an_ar_object.AsyncState;
            try
            {
                // Call "saved away" callback function
                currentAsyncState.workSocket.EndConnect(state_in_an_ar_object);
                currentAsyncState.connectionCallback(currentAsyncState);

                // "BeginReceive" expecting more data to arrive
                currentAsyncState.workSocket.BeginReceive(currentAsyncState.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReceiveCallback), currentAsyncState);
            }
            // Catch any exceptions
            catch (Exception exception)
            {
                currentAsyncState.connectionCallback(currentAsyncState);

                // Print error message
                Console.WriteLine(exception.ToString());
            }
        }

        /// <summary>
        /// The ReceiveCallback method is called by the OS when new data arrives. This method should check to see how much data has 
        /// arrived. If 0, the connection has been closed (presumably by the server). On greater than zero data, this method should 
        /// call the callback function provided above. For our purposes, this function should not request more data. It is up to the 
        /// code in the callback function above to request more data.
        /// </summary>
        public static void ReceiveCallback(IAsyncResult state_in_an_ar_object)
        {
            try
            {
                // Retrieve the state object from the asynchronous state object
                State currentAsyncState = (State)state_in_an_ar_object.AsyncState;

                // Read data from the remote device and save into count
                int count = currentAsyncState.workSocket.EndReceive(state_in_an_ar_object);

                // On greater than zero data, call the callback function
                if (count > 0)
                {
                    // Store the data received so far
                    currentAsyncState.sb.Append(Encoding.UTF8.GetString(currentAsyncState.buffer, 0, count));

                    // Call the provided callback function
                    currentAsyncState.connectionCallback(currentAsyncState);
                }
            }
            // Catch any exceptions
            catch (Exception exception2)
            {
                // Print error message
                Console.WriteLine("ERROR: " + exception2.ToString());
            }
        }

        /// <summary>
        /// This is a small helper function that the client View code will call whenever it wants more data. 
        /// Note: the client will probably want more data every time it gets data.
        /// </summary>
        public static void i_want_more_data(State state)
        {
            state.workSocket.BeginReceive(state.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        /// <summary>
        /// This function (along with it's helper 'SendCallback') will allow a program to send data over a socket. 
        /// This function needs to convert the data into bytes and then send them using socket.BeginSend.
        /// </summary>
        public static void Send(Socket socket, String data)
        {
            try
            {
                // Convert the string data to byte data using UTF8 encoding   
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                // Begin sending the data to the remote device
                socket.BeginSend(bytes, 0, bytes.Length, 0, new AsyncCallback(SendCallback), socket);
            }
            // Catch any exceptions
            catch (Exception)
            {
                try
                {
                    // Attempt to disable the socket if there is no more data to send
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception exception)
                {
                    // Print the error message
                    Console.WriteLine(exception.ToString());
                }
            }
        }

        /// <summary>
        /// This function "assists" the Send function. If all the data has been sent, then life is good and nothing needs to be 
        /// done. If there is more data to send, the SendCallBack needs to arrange to send this data.
        /// </summary>
        public static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Complete sending the data to the remote device
                ((Socket)ar.AsyncState).EndSend(ar);
            }
            // Catch any exceptions
            catch (Exception exception)
            {
                // Print the error message
                Console.WriteLine(exception.ToString());
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
            state.workSocket = listener;
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(new AsyncCallback(Accept_a_New_Client), state);
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
            State state = (State)ar.AsyncState;
            Socket listener = state.workSocket;
            Socket handler = listener.EndAccept(ar);

            State stateHandler = new State();
            stateHandler.workSocket = handler;

            //state.connectionCallback(state);

            listener.BeginAccept(new AsyncCallback(Accept_a_New_Client), listener);                     //Listens for new clients
            handler.BeginReceive(state.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReadCallback), stateHandler);
            //state.connectionCallback(stateHandler);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            //Console.WriteLine("here");
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            State state = (State)ar.AsyncState;
            Socket handler = state.workSocket;
            
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
