using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AgCubio
{
    /// <summary>
    /// This class opens the socket between the client and the server and provides helper functions 
    /// for sending and receiving data
    /// </summary>
    public static class Network
    {
        /// <summary>
        /// Attempts to connect to the server via a provided hostname
        /// </summary>
        /// <param name="callback_func">a function inside the View to be called when a connection is made</param>
        /// <param name="hostname">name of the server to connect to</param>
        /// <returns></returns>
        public static Socket Connect_to_Server(Action<Preserved_State> callback_func, string hostname)
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
                Preserved_State state = new Preserved_State();
                state.workSocket = socket;
                state.callbackFunction = callback_func;

                // Open socket and use BeginConnect method
                socket.BeginConnect(remoteEP, new AsyncCallback(Connected_to_Server), state);

                // Return the socket
                return socket;
            }
            // Catch any exceptions
            catch (Exception exception)
            {
                Preserved_State state2 = new Preserved_State();

                // Change error fields of state object accordingly
                state2.errorMessage = exception.ToString();
                state2.errorHappened = true;
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
            Preserved_State currentAsyncState = (Preserved_State)state_in_an_ar_object.AsyncState;
            try
            {
                // Call "saved away" callback function
                currentAsyncState.workSocket.EndConnect(state_in_an_ar_object);
                currentAsyncState.callbackFunction(currentAsyncState);

                // "BeginReceive" expecting more data to arrive
                currentAsyncState.workSocket.BeginReceive(currentAsyncState.buffer, 0, Preserved_State.BufferSize, 0, new AsyncCallback(ReceiveCallback), currentAsyncState);
            }
            // Catch any exceptions
            catch (Exception exception)
            {
                // Change error fields of state object accordingly
                currentAsyncState.errorHappened = true;
                currentAsyncState.errorMessage = exception.ToString();
                currentAsyncState.callbackFunction(currentAsyncState);

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
                Preserved_State currentAsyncState = (Preserved_State)state_in_an_ar_object.AsyncState;

                // Read data from the remote device and save into count
                int count = currentAsyncState.workSocket.EndReceive(state_in_an_ar_object);

                // On greater than zero data, call the callback function
                if (count > 0)
                {
                    // Store the data received so far
                    currentAsyncState.sb.Append(Encoding.UTF8.GetString(currentAsyncState.buffer, 0, count));

                    // Call the provided callback function
                    currentAsyncState.callbackFunction(currentAsyncState);
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
        public static void i_want_more_data(Preserved_State state)
        {
            state.workSocket.BeginReceive(state.buffer, 0, Preserved_State.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
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
    }
}
