using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NetworkController;
using AgCubio;
using Newtonsoft.Json;

namespace Server
{
    class Server
    {
        /// <summary>
        /// Main function, will build new world and start the server
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Server temp = new Server();
            temp.Start();


            
            //build new world
            //start the server
        }

        /// <summary>
        /// Populate initial world (with food)
        /// Set up heartbeat of program (use a timer)
        /// Await network client connections
        /// </summary>
        private void Start()
        {
            //populate initial world (with food)
            //set up heartbeat of program (use a timer)
            //await network client connections
            Network.Server_Awaiting_Client_Loop(HandleConnections);
            Console.WriteLine("here");
        }

        /// <summary>
        /// Needs to be a callback function required by the networking code
        /// Sets up a callback to receive a players name and then request more data
        /// 
        /// </summary>
        private void HandleConnections (State state)
        {
            Console.WriteLine("A new client has connected to the server.");
            string playerName = state.sb.ToString();
            ReceivePlayer(playerName, state);
            state.connectionCallback = DataFromClient;
        }

        /// <summary>
        /// Creates the new player cube (update the world about it)
        /// Store away all the data for the connection to be used for more communication
        /// Sets up the callback for handling move/split requests and request new data from the socket
        /// sends the current state of the world to the player
        /// </summary>
        private void ReceivePlayer(string data, State state)
        {
            Cube playerCube = new Cube(50, 50, 34875, 5000, 0, false, data, 1000);
            string message = JsonConvert.SerializeObject(playerCube);
            Network.Send(state.workSocket, message);
            
            
        }

        /// <summary>
        /// data should be either (move, x, y) or (split, x, y)
        /// </summary>
        private void DataFromClient(State state)
        {

        }

        /// <summary>
        /// Grow new food
        /// handle players eating food or other players
        /// handle player cube attrition (decay)
        /// Handle food growth
        /// Handle sending the current state of the world to EVERY client (new food, player changes)
        /// if a client disconects, should clean up
        /// </summary>
        private void Update ()
        {

        }
    }
}
