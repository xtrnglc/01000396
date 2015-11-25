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
using System.Timers;

namespace Server
{
    class Server
    {
        private Dictionary<int, Cube> WorldPlayerCubes = new Dictionary<int, Cube>();
        private Dictionary<int, Cube> FoodCubes = new Dictionary<int, Cube>();
        private double UID = 5000.0;
        private const int MaxFood = 2000;
        
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
        //comm
        /// <summary>
        /// Populate initial world (with food)
        /// Set up heartbeat of program (use a timer)
        /// Await network client connections
        /// </summary>
        private void Start()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();         //gets a timer for the heartbeat, not sure what to do with it
            Network.Server_Awaiting_Client_Loop(HandleConnections);
            //Console.WriteLine("here");
        }

        /// <summary>
        /// Needs to be a callback function required by the networking code
        /// Sets up a callback to receive a players name and then request more data
        /// 
        /// </summary>
        private void HandleConnections (State state)
        {
            string playerName = state.sb.ToString();
            Console.WriteLine("A new client has connected to the server: ");
            
            if (playerName.EndsWith("\n"))
            {
                playerName = playerName.Remove(playerName.Length - 1);
            }
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
            int location = 50;
            UID += 1;       //Makes sure there is a unique ID for all players
            if (UID > 10000)
            {
                UID = 1;
            }
            Cube playerCube = new Cube(50, 50, 34875, UID, 0, false, data, 1000);
            //if the dictionary is empty or if 
            if (WorldPlayerCubes.Count == 0)
            {
                WorldPlayerCubes.Add((int)UID, playerCube);
            }
            else if(WorldPlayerCubes.ContainsKey((int)UID))
            {
                UID = GenerateUID();
                WorldPlayerCubes.Add((int)UID, playerCube);
            }
            string message = JsonConvert.SerializeObject(playerCube) + "\n";
            state.connectionCallback = DataFromClient;
            Network.Send(state.workSocket, message);
            //Cube randomFood = new Cube(location += 20, location += 20, 34875, UID += 1, 0, true, "", 20);
            //string message2 = JsonConvert.SerializeObject(randomFood) + "\n";
            //randomFood = new Cube(location += 20, location += 20, 34875, UID += 1, 0, true, "", 20);
            //message2 += JsonConvert.SerializeObject(randomFood) + "\n";
            //Network.Send(state.workSocket, message2);
            Update(state);

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
        private void Update (State state)
        {
            //grow new food
            if (FoodCubes.Count < MaxFood)
            {
                Cube randomFood = new Cube(100, 100, 34875, UID += 1, 0, true, "", 20);
                string message2 = JsonConvert.SerializeObject(randomFood) + "\n";
                randomFood = new Cube(150, 150, 34875, UID += 1, 0, true, "", 20);
                message2 += JsonConvert.SerializeObject(randomFood) + "\n";
                Network.Send(state.workSocket, message2);
            }

        }

        private int GenerateUID()
        {
            Random rnd = new Random();
            int uid = rnd.Next(10000);
            while (WorldPlayerCubes.ContainsKey(uid))
            {
                uid = rnd.Next(10000);
            }
            return uid;
        }
    }
}
