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
        private double UID = 5000.0;
        private const int MaxFood = 2000;
        private List<Socket> playerSockets = new List<Socket>();
        
        private Cube Player;
        private Dictionary<Socket, Cube> sockets = new Dictionary<Socket, Cube>();
        private Random R = new Random();
        private World w = new World();
        private Dictionary<Socket, Tuple<int, int>> Destination = new Dictionary<Socket, Tuple<int, int>>();
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
        }

        /// <summary>
        /// Needs to be a callback function required by the networking code
        /// Sets up a callback to receive a players name and then request more data
        /// 
        /// </summary>
        private void HandleConnections(State state)
        {
            playerSockets.Add(state.workSocket);
            string message = "";
            state.connectionCallback = DataFromClient;
            string playerName = state.sb.ToString();
            Console.WriteLine("A new client has connected to the server: ");
            state.sb.Clear();
            lock (w)
            {
                if (playerName.EndsWith("\n"))
                {
                    playerName = playerName.Remove(playerName.Length - 1);
                }
                

                ReceivePlayer(playerName, state);
            }
            

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
            lock (w)
            {
                if (w.ListOfPlayers.Count == 0)
                {
                    w.ListOfPlayers.Add((int)UID, playerCube);
                }
                else if (w.ListOfPlayers.ContainsKey((int)UID))
                {
                    UID = GenerateUID();
                    w.ListOfPlayers.Add((int)UID, playerCube);
                }
                else
                {
                    w.ListOfPlayers.Add((int)UID, playerCube);
                }
            }
            

            sockets.Add(state.workSocket, playerCube);

            string message = JsonConvert.SerializeObject(playerCube) + "\n";
            state.connectionCallback = DataFromClient;
            Network.Send(state.workSocket, message);
            Console.WriteLine("Player add: " + data);
            //Cube randomFood = new Cube(location += 20, location += 20, 34875, UID += 1, 0, true, "", 20);
            //string message2 = JsonConvert.SerializeObject(randomFood) + "\n";
            //randomFood = new Cube(location += 20, location += 20, 34875, UID += 1, 0, true, "", 20);
            //message2 += JsonConvert.SerializeObject(randomFood) + "\n";
            //Network.Send(state.workSocket, message2);

            lock(w)
            if (sockets.Count > 0 && w.ListOfFood.Count > 0)
            {
                foreach (Socket s in sockets.Keys)
                {
                    if (w.ListOfPlayers.Count > 0)
                    {
                        foreach (Cube t in w.ListOfPlayers.Values)
                        {
                            message += JsonConvert.SerializeObject(t) + "\n";
                        }
                        Network.Send(s, message);
                    }
                    message = "";

                    foreach (Cube c in w.ListOfFood.Values)
                    {
                        message += JsonConvert.SerializeObject(c) + "\n";
                    }

                    Network.Send(s, message);
                }
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
            Tuple<int, int> pair;
            Cube temp;
            string commands = state.sb.ToString();
            string[] substrings = Regex.Split(commands, "\n");

            int count = substrings.Count();
            state.sb.Clear();
            state.sb.Append(substrings[count - 1]);
            substrings[count - 1] = null;
            if (substrings.Count() > 0)
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
                        lock (w)
                        {
                            string[] location = command.Split(',');
                            location[2] = location[2].Substring(0, location[2].Length - 1);

                            int.TryParse(location[1], out x);
                            int.TryParse(location[2], out y);

                            sockets.TryGetValue(state.workSocket, out temp);
                            xold = (int)temp.GetX();
                            yold = (int)temp.GetY();
                            
                            pair = new Tuple<int, int>(x, y);

                                //if (x > xold)
                                //    temp.loc_x = xold + 1;
                                //else
                                //    temp.loc_x = xold - 1;
                                //if (y > yold)
                                //    temp.loc_y = yold + 1;
                                //else
                                //    temp.loc_y = yold - 1;
                            if (Destination.ContainsKey(state.workSocket))
                            {
                                Destination[state.workSocket] = pair;
                            }
                            else
                            {
                                Destination.Add(state.workSocket, pair);
                            }

                                
                            

                            string msg = JsonConvert.SerializeObject(temp);
                            Network.Send(state.workSocket, msg + "\n");
                            
                        }
                        
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
            string message;
            int xold;
            int yold;
            Cube temp;
            Tuple<int, int> pair;

            //grow new food
            lock (w)
            {
                if (w.ListOfFood.Count < MaxFood)
                {
                    Cube randomFood = new Cube(R.Next(1, 1000), R.Next(1, 1000), RandomColor(R), UID += 1, 0, true, "", 20);
                    w.ListOfFood.Add(randomFood.GetID(), randomFood);
                    message = JsonConvert.SerializeObject(randomFood) + "\n";
                    
                    //Network.Send(state.workSocket, message2);
                    if (sockets.Count > 0)
                    {
                        foreach (Socket s in sockets.Keys)
                        {
                            Network.Send(s, message);
                        }
                    }
                }

                

                message = "";
                if (w.ListOfPlayers.Count > 0 && sockets.Count > 0)
                {
                    foreach (KeyValuePair<Socket, Tuple<int, int>> s in Destination.ToList())
                    {

                        sockets.TryGetValue(s.Key, out temp);

                        xold = (int)temp.GetX();
                        yold = (int)temp.GetY();
                        w.ListOfPlayers.Remove(temp.GetID());
                        pair = s.Value;

                        if (pair.Item1 > xold)
                            temp.loc_x = xold + 1;
                        else
                            temp.loc_x = xold - 1;
                        if (pair.Item2 > yold)
                            temp.loc_y = yold + 1;
                        else
                            temp.loc_y = yold - 1;

                        sockets[s.Key] = temp;
                        w.ListOfPlayers.Add(temp.GetID(), temp);
                    }
                    foreach (Cube c in w.ListOfPlayers.Values)
                    {
                        message += JsonConvert.SerializeObject(c) + "\n";
                    }

                    foreach (Socket s in sockets.Keys)
                    {
                        Network.Send(s, message);
                    }
                }





            }  
        }

        private void generateIntitialFood()
        {
            lock (w)
            {
                for (int i = 0; i < 100; i++)
                {
                    Cube randomFood = new Cube(R.Next(1, 1000), R.Next(1, 1000), RandomColor(R), UID += 1, 0, true, "", 20);
                    w.ListOfFood.Add(randomFood.GetID(), randomFood);
                }
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
            while (w.ListOfPlayers.ContainsKey(uid))
            {
                uid = rnd.Next(10000);
            }
            return uid;
        }
    }
}
