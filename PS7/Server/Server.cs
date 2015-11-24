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
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Timers;

namespace Server
{
    class Server
    {
        private World world = new World();
        private int UID = 5000;
        private Stopwatch timers = new Stopwatch();
        private System.Timers.Timer timer;

        /// <summary>
        /// Main function, will build new world and start the server
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Server temp = new Server();
            temp.Start();
            Console.ReadLine();
            
            //build new world
            //start the server
        }
        //comm
        /// <summary>
        /// Populate initial world (with food)
        /// Set up heartbeat of program (use a timer)
        /// Await network client connections
        /// </summary>
        private void Start()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            //populate initial world (with food)
            //set up heartbeat of program (use a timer)
            //await network client connections
            initialPopulate();            
            Network.Server_Awaiting_Client_Loop(HandleConnections);
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
        }

        /// <summary>
        /// Creates the new player cube (update the world about it)
        /// Store away all the data for the connection to be used for more communication
        /// Sets up the callback for handling move/split requests and request new data from the socket
        /// sends the current state of the world to the player
        /// </summary>
        private void ReceivePlayer(string playerName, State state)
        {
            UID = GenerateUID();
            Random rnd = new Random();
            //Create player cube, add it to world
            Cube playerCube = new Cube(rnd.Next(0,100), rnd.Next(0,100), rnd.Next(99999), UID, 0, false, playerName, 1000);

            world.ListOfPlayers.Add(UID, playerCube);

            string message = JsonConvert.SerializeObject(playerCube);
            state.connectionCallback = DataFromClient;
            Network.Send(state.workSocket, message + "\n");

            foreach(Cube c in world.ListOfFood.Values)
            {
                string msg = JsonConvert.SerializeObject(c);
                Network.Send(state.workSocket, msg + "\n");
            }
        }

        /// <summary>
        /// data should be either (move, x, y) or (split, x, y)
        /// </summary>
        private void DataFromClient(State state)
        {
            string commands = state.sb.ToString();
            string[] substrings = Regex.Split(commands, "\n");
            int count = substrings.Count();
            state.sb.Clear();
            state.sb.Append(substrings[count - 1]);
            substrings[count - 1] = null;
            foreach (string command in substrings)
            {
                if(command != null)
                {
                    if(command.StartsWith("(split"))
                    {
                        //split the cube
                    }
                    else
                    {
                        //move the cube
                    }
                }
            }
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

        private void initialPopulate()
        {
            int minutes = (int)timers.Elapsed.Minutes;
            int totalSeconds = (int)timers.Elapsed.Seconds;
            int seconds = totalSeconds % 60;
            Random rnd = new Random();

            //Generate 100 food cubes
            for(int i = 0; i < 100; i++)
            {
                int id = GenerateUID();
                Cube newCube = new Cube(rnd.Next(0, 1000), rnd.Next(0, 1000), rnd.Next(9999999), id, 0, true, "", 5);
                world.ListOfFood.Add(id, newCube);
            }      
        }

        private int GenerateUID()
        {
            Random rnd = new Random();
            int uid = rnd.Next(999999);
            while (world.ListOfPlayers.ContainsKey(uid) || world.ListOfFood.ContainsKey(uid))
            {
                uid = rnd.Next(999999);
            }
            return uid;
        }
    }
}
