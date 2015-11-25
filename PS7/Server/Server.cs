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
using System.Drawing;

namespace Server
{
    class Server
    {
        private Dictionary<int, Cube> PlayerCubes = new Dictionary<int, Cube>();
        private Dictionary<int, Cube> FoodCubes = new Dictionary<int, Cube>();
        private double UID = 5000.0;
        private const int MaxFood = 2000;
        private List<Socket> playerSockets = new List<Socket>();
        private World world;
        private Cube Player;
        private Dictionary<Socket, Cube> sockets = new Dictionary<Socket, Cube>();
        private Random R = new Random();

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
            System.Timers.Timer aTimer = new System.Timers.Timer(1000/25);
            
            aTimer.Elapsed += Update;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            generateIntitialFood();
            Network.Server_Awaiting_Client_Loop(HandleConnections);
            //Console.WriteLine("here");
        }

        /// <summary>
        /// Needs to be a callback function required by the networking code
        /// Sets up a callback to receive a players name and then request more data
        /// 
        /// </summary>
        private void HandleConnections(State state)
        {
            playerSockets.Add(state.workSocket);
            state.connectionCallback = DataFromClient;
            string playerName = state.sb.ToString();
            Console.WriteLine("A new client has connected to the server: ");
            state.sb.Clear();

            if (playerName.EndsWith("\n"))
            {
                playerName = playerName.Remove(playerName.Length - 1);
            }
            ReceivePlayer(playerName, state);

        }

        /// <summary>
        /// Creates the new player cube (update the world about it)
        /// Store away all the data for the connection to be used for more communication
        /// Sets up the callback for handling move/split requests and request new data from the socket
        /// sends the current state of the world to the player
        /// </summary>
        private void ReceivePlayer(string data, State state)
        {
            
            int locationx = R.Next(1, 1000);
            int locationy = R.Next(1, 1000);
            UID += 1;       //Makes sure there is a unique ID for all players
            if (UID > 10000.0)
            {
                UID = 1.0;
            }
            Cube playerCube = new Cube(locationx, locationy, RandomColor(R), (double)UID, 0, false, data, 1000);
            Player = playerCube;
            //if the dictionary is empty or if 
            if (PlayerCubes.Count == 0)
            {
                PlayerCubes.Add((int)UID, playerCube);
            }
            else if (PlayerCubes.ContainsKey((int)UID))
            {
                UID = GenerateUID();
                PlayerCubes.Add((int)UID, playerCube);
            }

            sockets.Add(state.workSocket, playerCube);

            string message = JsonConvert.SerializeObject(playerCube) + "\n";
            state.connectionCallback = DataFromClient;
            Network.Send(state.workSocket, message);
            Console.WriteLine("done");
            //Cube randomFood = new Cube(location += 20, location += 20, 34875, UID += 1, 0, true, "", 20);
            //string message2 = JsonConvert.SerializeObject(randomFood) + "\n";
            //randomFood = new Cube(location += 20, location += 20, 34875, UID += 1, 0, true, "", 20);
            //message2 += JsonConvert.SerializeObject(randomFood) + "\n";
            //Network.Send(state.workSocket, message2);

            foreach (Cube c in FoodCubes.Values)
            {
                string msg = JsonConvert.SerializeObject(c) + "\n";
                Network.Send(state.workSocket, msg);
            }
        }

        /// <summary>
        /// data should be either (move, x, y) or (split, x, y)
        /// </summary>
        private void DataFromClient(State state)
        {
            //Update(state);
            int x;
            int y;
            int xold;
            int yold;
            Cube temp;
            string commands = state.sb.ToString();
            string[] substrings = Regex.Split(commands, "\n");

            int count = substrings.Count();
            state.sb.Clear();
            state.sb.Append(substrings[count - 1]);
            substrings[count - 1] = null;
            foreach (string command in substrings)
            {
                if (command != null)
                {
                    if (command.StartsWith("(split"))
                    {
                        //split the cube
                    }
                    else
                    {
                        string[] location = command.Split(',');
                        location[2] = location[2].Substring(0, location[2].Length - 1);
                        
                        int.TryParse(location[1], out x);
                        int.TryParse(location[2], out y);

                        sockets.TryGetValue(state.workSocket, out temp);
                        PlayerCubes.Remove(temp.GetID());

                        xold = (int)temp.GetX();
                        yold = (int)temp.GetY();

                        if (x < xold)

                        temp.loc_x = x;
                        temp.loc_y = y;

                        PlayerCubes.Add(temp.GetID(), temp);

                        string msg = JsonConvert.SerializeObject(temp);
                        Network.Send(state.workSocket, msg + "\n");
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
        private void Update(Object o, ElapsedEventArgs e)
        {
            
            
            //grow new food

            if (FoodCubes.Count < MaxFood)
            {
                Cube randomFood = new Cube(R.Next(1, 1000), R.Next(1, 1000), RandomColor(R), UID += 1, 0, true, "", 20);
                FoodCubes.Add(randomFood.GetID(), randomFood);
                string message2 = JsonConvert.SerializeObject(randomFood) + "\n";
                //Network.Send(state.workSocket, message2);
                foreach (Socket s in sockets.Keys)
                {
                    Network.Send(s, message2);
                }
            }
        }

        private void generateIntitialFood()
        {
            
            for (int i = 0; i < 100; i++)
            {
                Cube randomFood = new Cube(R.Next(1, 1000), R.Next(1, 1000), RandomColor(R), UID += 1, 0, true, "", 20);
                FoodCubes.Add(randomFood.GetID(), randomFood);
            }
        }

        private int RandomColor(Random r)
        {
            
            KnownColor[] colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randColor = colors[r.Next(0, colors.Length)];
            int colorCode = Color.FromKnownColor(randColor).ToArgb();
            return colorCode;
        }

        
       

        private int GenerateUID()
        {
            Random rnd = new Random();
            int uid = rnd.Next(10000);
            while (PlayerCubes.ContainsKey(uid))
            {
                uid = rnd.Next(10000);
            }
            return uid;
        }
    }
}
