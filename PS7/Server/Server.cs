/*
Author: Trung Le and Adam Sorensen
12/2/2015
CS 3500
PS8 - AgCubio Server
*/

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
using System.Xml;
using System.IO;
using System.Diagnostics;
using DatabaseController;


namespace Server
{
    class Server
    {
        /// <summary>
        /// UID starts at 1. Each cube will have a unique ID, will add 1 to everycube
        /// </summary>
        private int UID = 1;
        /// <summary>
        /// List of all the sockets that will connect to the server and keeps track of them
        /// </summary>
        private List<Socket> playerSockets = new List<Socket>();
        /// <summary>
        /// Is the path to the editable xml file to change game parameters
        /// </summary>
        private string pathToFile = "gamestate.txt";
        /// <summary>
        /// Keeps the socket as the key, and the cube associated with the socket as the value
        /// </summary>
        private Dictionary<Socket, Cube> sockets = new Dictionary<Socket, Cube>();
        /// <summary>
        /// Oppisite as above, in case we need the Cube associated with a certain socket
        /// </summary>
        private Dictionary<Cube, Socket> cubetosockets = new Dictionary<Cube, Socket>();
        /// <summary>
        /// Global random variable so we don't have to create one every color method call
        /// </summary>
        private Random R = new Random();
        /// <summary>
        /// For collision dectection. Every player cube will have a rectangle associated with it
        /// </summary>
        private Dictionary<int, Rectangle> rectangles = new Dictionary<int, Rectangle>();
        /// <summary>
        /// Creates a world object so the server can keep track of things, use constants
        /// </summary>
        private World w = new World();
        /// <summary>
        /// Keeps track of the special cubes - viruses
        /// </summary>
        private Dictionary<int, Cube> VirusList = new Dictionary<int, Cube>();
        /// <summary>
        /// used for coorindates of the mouse
        /// </summary>
        private Dictionary<Socket, Tuple<int, int>> Destination = new Dictionary<Socket, Tuple<int, int>>();
        /// <summary>
        /// Every player cube will get a unique team ID, this is used for keeping track of splitting and "friendly" cubes
        /// </summary>
        private int teamid = 0;
        /// <summary>
        /// Timer for the heartbeat
        /// </summary>
        private System.Timers.Timer aTimer;
        /// <summary>
        /// Timer for the attrition
        /// </summary>
        private System.Timers.Timer attritionTimer;
        /// <summary>
        /// Stop watch for the splitting and merging
        /// </summary>
        private Stopwatch stopWatch = new Stopwatch();
        /// <summary>
        /// Main function, will build new world and start the server
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the AgCubio server");
            
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
            string element = "";
            int Width = 0;
            int Height = 0;
            int maxFood = 0;
            int topSpeed = 0;
            int attritionRate = 0;
            int foodValue = 0;
            int startMass = 0;
            int minimumSplitMass = 0;
            int maximumSplits = 0;
            int numberofVirus = 0;
            int maxsize = 0;
            int mergeTimer = 0;
            int virusSize = 0;
            int attritiontimer = 0;
            Console.WriteLine("Do you have an XML gamestate file? Y to try and parse the file");
            string choice = Console.ReadLine();
            //Try and parse the game state xml file
            if (choice == "Y" || choice == "y")
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(pathToFile))
                    {
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                switch (reader.Name)
                                {
                                    case "width":
                                        element = reader.Name;                                       
                                        break;

                                    case "height":
                                        element = reader.Name;
                                        break;

                                    case "maxfood":
                                        element = reader.Name;
                                        break;

                                    case "topspeed":
                                        element = reader.Name;
                                        break;

                                    case "attrition":
                                        element = reader.Name;
                                        break;

                                    case "foodvalue":
                                        element = reader.Name;
                                        break;

                                    case "startmass":
                                        element = reader.Name;
                                        break;

                                    case "minsplitmass":
                                        element = reader.Name;
                                        break;

                                    case "maxsplits":
                                        element = reader.Name;
                                        break;

                                    case "numberofvirus":
                                        element = reader.Name;
                                        break;
                                    case "maxsize":
                                        element = reader.Name;
                                        break;
                                    case "mergetimer":
                                        element = reader.Name;
                                        break;
                                    case "virussize":
                                        element = reader.Name;
                                        break;
                                    case "attritiontime":
                                        element = reader.Name;
                                        break;
                                }
                            }
                            else
                            {
                                switch (element)
                                {
                                    case "width":
                                        int.TryParse(reader.Value, out Width);
                                        element = "";
                                        break;

                                    case "height":
                                        int.TryParse(reader.Value, out Height);
                                        element = "";
                                        break;

                                    case "maxfood":
                                        int.TryParse(reader.Value, out maxFood);
                                        element = "";
                                        break;

                                    case "topspeed":
                                        int.TryParse(reader.Value, out topSpeed);
                                        element = "";
                                        break;

                                    case "attrition":
                                        int.TryParse(reader.Value, out attritionRate);
                                        element = "";
                                        break;

                                    case "foodvalue":
                                        int.TryParse(reader.Value, out foodValue);
                                        element = "";
                                        break;

                                    case "startmass":
                                        int.TryParse(reader.Value, out startMass);
                                        element = "";
                                        break;

                                    case "minsplitmass":
                                        int.TryParse(reader.Value, out minimumSplitMass);
                                        element = "";
                                        break;

                                    case "maxsplits":
                                        int.TryParse(reader.Value, out maximumSplits);
                                        element = "";
                                        break;

                                    case "numberofvirus":
                                        int.TryParse(reader.Value, out numberofVirus);
                                        element = "";
                                        break;
                                    case "maxsize":
                                        int.TryParse(reader.Value, out maxsize);
                                        element = "";
                                        break;

                                    case "mergetimer":
                                        int.TryParse(reader.Value, out mergeTimer);
                                        element = "";
                                        break;

                                    case "virussize":
                                        int.TryParse(reader.Value, out virusSize);
                                        element = "";
                                        break;

                                    case "attritiontime":
                                        int.TryParse(reader.Value, out attritiontimer);
                                        element = "";
                                        break;

                                    case "":
                                        break;
                                }
                            }
                        }
                    }
                    Console.WriteLine("Game state XML parsed successfully");
                    w = new World(Width, Height, maxFood, topSpeed, attritionRate, foodValue, startMass, minimumSplitMass, maximumSplits, numberofVirus, maxsize, mergeTimer, virusSize, attritiontimer);
                }
                
                catch (Exception)
                {
                    Console.WriteLine("Game state XML not parsed correctly. Server will start with default parameters");
                    w = new World();
                }
                
            }

            //If no game state file present, use default constructor
            else
            {
                w = new World();
            }
            aTimer = new System.Timers.Timer(1000/25);
            attritionTimer = new System.Timers.Timer(w.attritionTimer * 1000);
            stopWatch.Start();
            attritionTimer.Elapsed += attritionUpdate;
            attritionTimer.AutoReset = true;
            attritionTimer.Enabled = true;
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
            state.connectionCallback = DataFromClient;
            string playerName = state.sb.ToString();
            Console.WriteLine("A new client has connected to the server");
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
            teamid += 5;
            int locationx = R.Next(1, w.GetWidth);
            int locationy = R.Next(1, w.GetHeight);
            UID += 1;       //Makes sure there is a unique ID for all players
            if (UID > 10000.0)
            {
                UID = 1;
            }
            Cube playerCube = new Cube(200, 200, PlayerColor(R), UID, teamid, false, data, w.startMass);
            //playerCube.maxMass = w.startMass;
            //playerCube.spawnTime = stopWatch.ElapsedMilliseconds;

            Rectangle playerRectangleF = new Rectangle((int)(playerCube.loc_x - playerCube.GetWidth() * 1.5), (int)(playerCube.loc_y - playerCube.GetWidth() * 1.5), playerCube.GetWidth() * 3, playerCube.GetWidth() * 3);
            //Player = playerCube;
            //if the dictionary is empty or if 
            lock (w)
            {
                if (w.ListOfPlayers.Count == 0)
                {
                    w.ListOfPlayers.Add(UID, playerCube);
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
            rectangles.Add(playerCube.uid, playerRectangleF);
            sockets.Add(state.workSocket, playerCube);
            cubetosockets.Add(playerCube, state.workSocket);

            string message = JsonConvert.SerializeObject(playerCube) + "\n";
            state.connectionCallback = DataFromClient;
            Network.Send(state.workSocket, message);
            Console.WriteLine("Player " + data + " has joined the game");
            message = "";

            lock(w)
            if (sockets.Count > 0 && w.ListOfFood.Count > 0)
            {
                if (w.ListOfPlayers.Count > 0)
                {
                    foreach (Cube t in w.ListOfPlayers.Values)
                    {
                        message += JsonConvert.SerializeObject(t) + "\n";
                    }
                    Network.Send(state.workSocket, message);
                }
                    message = "";
                foreach (Cube c in w.ListOfFood.Values)
                {
                    message += JsonConvert.SerializeObject(c) + "\n";
                }
                Network.Send(state.workSocket, message);
                
            }
            Console.WriteLine("Player id: " + playerCube.uid);
        }

        /// <summary>
        /// data should be either (move, x, y) or (split, x, y)
        /// </summary>
        private void DataFromClient(State state)
        {
            int x;
            int y;
            int xold;
            int yold;
            Tuple<int, int> pair;
            Cube temp;
            string message = "";
            string commands = state.sb.ToString();
            string[] substrings = Regex.Split(commands, "\n");
            long splittime = 0;
            Rectangle tempRectangleF;
            Boolean first = true;
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
                            string[] location = command.Split(',');
                            location[2] = location[2].Substring(0, location[2].Length - 1);

                            int.TryParse(location[1], out x);
                            int.TryParse(location[2], out y);

                            //Deal with split
                            lock (w)
                            {
                                sockets.TryGetValue(state.workSocket, out temp);

                                if (temp.Mass > w.minimumSplitMass && FindTeamCubes(temp.team_id) != null)
                                {
                                    foreach (Cube c in FindTeamCubes(temp.team_id))
                                    {
                                        c.Mass = c.Mass / 2;
                                        c.loc_x -= 100;
                                        c.loc_y -= 100;
                                        c.numberOfSplits++;

                                        tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3,
                                        c.GetWidth() * 3);
                                        rectangles[c.uid] = tempRectangleF;

                                        Cube split = new Cube(c.loc_x + 200, c.loc_y + 200, c.argb_color, GenerateUID(), c.team_id, false, c.Name, c.Mass);
                                        split.numberOfSplits = c.numberOfSplits;
                                        tempRectangleF = new Rectangle((int)(split.loc_x - split.GetWidth() * 1.5), (int)(split.loc_y - split.GetWidth() * 1.5), split.GetWidth()
                                         * 3, split.GetWidth() * 3);
                                        rectangles.Add(split.uid, tempRectangleF);

                                        if (c.splitTime == 0)
                                        {
                                            c.splitTime = stopWatch.ElapsedMilliseconds;
                                            split.splitTime = c.splitTime;
                                        }
                                        else
                                        {
                                            if (first)
                                            {
                                                splittime = stopWatch.ElapsedMilliseconds;
                                                split.splitTime = splittime;
                                                first = false;
                                            }
                                            else
                                            {
                                                split.splitTime = splittime;
                                            }

                                        }

                                        w.ListOfPlayers.Add(split.uid, split);

                                        message += (JsonConvert.SerializeObject(split) + "\n");
                                        message += (JsonConvert.SerializeObject(c) + "\n");
                                        Network.Send(state.workSocket, message);
                                    }
                                }
                            }
                        }
                        else if (command.StartsWith("(move"))
                        {
                            lock (w)
                            {
                                string[] location = command.Split(',');
                                location[2] = location[2].Substring(0, location[2].Length - 1);

                                int.TryParse(location[1], out x);
                                int.TryParse(location[2], out y);

                                if (x > w.GetWidth)
                                {
                                    x = w.GetWidth;
                                }
                                if (x < 0)
                                {
                                    x = 0;
                                }
                                if (y > w.GetHeight)
                                {
                                    y = w.GetHeight;
                                }
                                if (y < 0)
                                {
                                    y = 0;
                                }
                                sockets.TryGetValue(state.workSocket, out temp);
                                xold = (int)temp.GetX();
                                yold = (int)temp.GetY();

                                pair = new Tuple<int, int>(x, y);

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
                        else
                        {
                            //Bad command, terminate socket
                            sockets.Remove(state.workSocket);
                            playerSockets.Remove(state.workSocket);
                            Destination.Remove(state.workSocket);
                            state.workSocket.Disconnect(false);
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
            string message = "";
            string message2 = "";
            Rectangle tempRectangleF;
            
            Cube temp2;

            Socket tempsocket, tempsocket2;
            
            //grow new food
            lock (w)
            {
                if (w.ListOfFood.Count < w.maxFood)
                {    
                    Cube randomFood = new Cube(R.Next(1, w.GetWidth), R.Next(1, w.GetHeight), RandomColor(R), UID += 1, 0, true, "", w.foodValue);
                    w.ListOfFood.Add(randomFood.GetID(), randomFood);
                    message += JsonConvert.SerializeObject(randomFood) + "\n";
                    randomFood = new Cube(R.Next(1, w.GetWidth), R.Next(1, w.GetHeight), RandomColor(R), UID += 1, 0, true, "", w.foodValue);
                    w.ListOfFood.Add(randomFood.GetID(), randomFood);
                    message += JsonConvert.SerializeObject(randomFood) + "\n";
                    randomFood = new Cube(R.Next(1, w.GetWidth), R.Next(1, w.GetHeight), RandomColor(R), UID += 1, 0, true, "", w.foodValue);
                    w.ListOfFood.Add(randomFood.GetID(), randomFood);
                    message += JsonConvert.SerializeObject(randomFood) + "\n";
                    randomFood = new Cube(R.Next(1, w.GetWidth), R.Next(1, w.GetHeight), RandomColor(R), UID += 1, 0, true, "", w.foodValue);
                    w.ListOfFood.Add(randomFood.GetID(), randomFood);
                    message += JsonConvert.SerializeObject(randomFood) + "\n";

                    if (VirusList.Count < w.numberOfVirus)
                    {
                        MakeVirus();
                        VirusList = FindVirus();
                        foreach (Cube v in VirusList.Values)
                        {
                            message += JsonConvert.SerializeObject(v) + "\n";
                        }
                    }
                    //Send new foods
                    foreach (Socket s in sockets.Keys)
                    {
                        Network.Send(s, message);
                    }        
                }

                
                Move();
                //Handle move requests
                message = "";
                message2 = "";
                if (w.ListOfPlayers.Count > 0 && sockets.Count > 0)
                {
                    foreach (Cube c in w.ListOfPlayers.Values.ToList())
                    {
                        message += JsonConvert.SerializeObject(c) + "\n";
                        //Send current position of each cube
                        foreach (Socket s in sockets.Keys)
                        {
                            Network.Send(s, message);
                        }
                        while (foodEaten(c) != null)
                        {
                            temp2 = foodEaten(c);
                            w.ListOfFood.Remove(temp2.GetID());

                            /*
                            c.cubesEaten++;
                            c.Mass += temp2.Mass;

                            if(c.Mass > c.maxMass)
                            {
                                c.maxMass = c.Mass;
                            }*/

                            temp2.Mass = 0;
                            //Send the dead cube
                            message += JsonConvert.SerializeObject(temp2) + "\n";
                            message2 += JsonConvert.SerializeObject(c) + "\n";

                            tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);
                            rectangles[c.uid] = tempRectangleF;
                        }

                        while (VirusEaten(c) != null)
                        {
                            temp2 = VirusEaten(c);
                            w.ListOfFood.Remove(temp2.GetID());
                            VirusList.Remove(temp2.GetID());
                            temp2.Mass = 0;
                            if (c.numberOfSplits >= w.maximumSplits)
                                c.Mass += temp2.Mass;
                            else      //split the cube
                            {
                                message += Split(c);
                            }
                            
                            //Send the dead cube
                            message += JsonConvert.SerializeObject(temp2) + "\n";
                            message2 += JsonConvert.SerializeObject(c) + "\n";

                            tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);
                            rectangles[c.uid] = tempRectangleF;
                        }
                        //Send updated mass of cubes after eating food
                        foreach (Socket s in sockets.Keys)
                        {
                            Network.Send(s, message);
                            Network.Send(s, message2);
                        }
                        message = "";
                        while (playerEaten(c) != null)
                        {
                            temp2 = playerEaten(c);
                            //temp2 = eaten
                            //c = eater
                            //Deal with regular cubes being eaten
                            if (FindTeamCubes(temp2.team_id).Count == 1)
                            {
                                c.Mass += temp2.Mass;
                                /*
                                if (c.Mass > c.maxMass)
                                {
                                    c.maxMass = c.Mass;
                                }
                                c.updatePlayersEaten(temp2.Name);
                                c.cubesEaten++;
                                
                                */
                                temp2.Mass = 0;

                                

                                /*
                                Update database entry here
                                */

                                //updateDB(temp2);


                                //Tell the player that his cube is dead and remove references to the client
                                message2 = JsonConvert.SerializeObject(temp2) + "\n";
                                cubetosockets.TryGetValue(temp2, out tempsocket);
                                cubetosockets.TryGetValue(c, out tempsocket2);
                                message += JsonConvert.SerializeObject(c) + "\n";

                                //Network.Send(tempsocket2, message);
                                Network.Send(tempsocket, message);
                                Network.Send(tempsocket, message2);

                                w.ListOfPlayers.Remove(temp2.GetID());
                                cubetosockets.Remove(temp2);
                                sockets.Remove(tempsocket);
                                Destination.Remove(tempsocket);
                                playerSockets.Remove(tempsocket);

                                tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);
                                rectangles[c.uid] = tempRectangleF;
                                //Send updated mass of cube after eating players
                                foreach (Socket s in sockets.Keys)
                                {
                                    Network.Send(s, message);
                                    Network.Send(s, message2);
                                }
                            }
                            //Deal with eating split cubes
                            else
                            {
                                Socket transferSocket;
                                //If main cube from split is being eaten
                                if(cubetosockets.TryGetValue(temp2, out transferSocket))
                                {
                                    c.Mass += temp2.Mass;

                                    /*
                                    if (c.Mass > c.maxMass)
                                    {
                                        c.maxMass = c.Mass;
                                    }
                                    c.cubesEaten++;
                                    c.updatePlayersEaten(temp2.Name);
                                    */
                                    temp2.Mass = 0;

                                    

                                    //Tell the player that his cube is dead and remove references to the client
                                    message2 = JsonConvert.SerializeObject(temp2) + "\n";
                                    cubetosockets.TryGetValue(c, out tempsocket2);
                                    message += JsonConvert.SerializeObject(c) + "\n";

                                    //Network.Send(tempsocket2, message);

                                    tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);
                                    rectangles[c.uid] = tempRectangleF;

                                    //Remove references to the old cube and have one of the team cubes be the "main" cube
                                    w.ListOfPlayers.Remove(temp2.GetID());
                                    cubetosockets.Remove(temp2);
                                    List < Cube > transferCube = FindTeamCubes(temp2.team_id);
                                    transferCube.Remove(temp2);
                                    Cube newMain = transferCube.ElementAt(0);
                                    sockets[transferSocket] = newMain;
                                    cubetosockets.Add(newMain, transferSocket);

                                    foreach (Socket s in sockets.Keys)
                                    {
                                        Network.Send(s, message);
                                        Network.Send(s, message2);
                                    }
                                }
                                else
                                {
                                    c.Mass += temp2.Mass;
                                    /*
                                    if (c.Mass > c.maxMass)
                                    {
                                        c.maxMass = c.Mass;
                                    }
                                    c.cubesEaten++;
                                    c.updatePlayersEaten(temp2.Name);*/
                                    temp2.Mass = 0;

                                    

                                    //Tell the player that his cube is dead and remove references to the client
                                    message2 = JsonConvert.SerializeObject(temp2) + "\n";
                                    cubetosockets.TryGetValue(temp2, out tempsocket);
                                    cubetosockets.TryGetValue(c, out tempsocket2);
                                    message += JsonConvert.SerializeObject(c) + "\n";

                                    //Network.Send(tempsocket2, message);
                                    Network.Send(tempsocket, message);
                                    Network.Send(tempsocket, message2);

                                    w.ListOfPlayers.Remove(temp2.GetID());
                                    cubetosockets.Remove(temp2);
                                    sockets.Remove(tempsocket);
                                    Destination.Remove(tempsocket);
                                    playerSockets.Remove(tempsocket);

                                    tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);
                                    rectangles[c.uid] = tempRectangleF;
                                    //Send updated mass of cube after eating players
                                    foreach (Socket s in sockets.Keys)
                                    {
                                        Network.Send(s, message);
                                        Network.Send(s, message2);
                                    }
                                }
                            }
                        }  
                    }                 
                }
            }  
        }

        /// <summary>
        /// Updates the database with the dead player cube
        /// </summary>
        /// <param name="c"></param>
        private void updateDB(Cube c)
        {
            //long timeAlive = stopWatch.ElapsedMilliseconds - c.spawnTime;
            long timeOfDeath = stopWatch.ElapsedMilliseconds;

            //if(c.playersEaten.Count == 0)
            {
                //Empty string to show that the player has died without eating any other player
                //AccessDatabase.Insert(c.uid, c.Name, (int)timeAlive, c.maxMass, c.cubesEaten, (int)timeOfDeath, "");
            }
            //else
            {
                //AccessDatabase.Insert(c.uid, c.Name, (int)timeAlive, c.maxMass, c.cubesEaten, (int)timeOfDeath, c.playersEaten.ToString());
            }
        }

        /// <summary>
        /// Determines if a player cube is overlapping another player cube
        /// </summary>
        /// <param name="playerCube"></param>
        /// <returns></returns>
        private Cube playerEaten(Cube playerCube)
        {
            double offset = 1.5;
            while (w.ListOfPlayers.Count > 1)
            {
                foreach (Cube c in w.ListOfPlayers.Values)
                {
                    if (c.uid != playerCube.uid && c.team_id != playerCube.team_id)
                    {
                        if (playerCube.GetX() > (int)c.GetX() - (playerCube.GetWidth() * offset) && playerCube.GetX() < (int)c.GetX() + (playerCube.GetWidth() * offset))
                        {
                            if (playerCube.GetY() > (int)c.GetY() - (playerCube.GetWidth() * offset) && playerCube.GetY() < (int)c.GetY() + (playerCube.GetWidth() * offset))
                            {
                                if (c.Mass > playerCube.Mass)
                                {
                                    return playerCube;
                                }
                                else if (playerCube.Mass > c.Mass)
                                {
                                    return c;
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                    }
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Updates the attrition timer
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void attritionUpdate(Object o, ElapsedEventArgs e)
        {
            lock (w)
            {
                foreach (Cube c in w.ListOfPlayers.Values)
                {
                    if (c.Mass >= w.startMass)
                    {
                        c.Mass -= w.attritionRate * c.Mass / 100;
                    }
                }
            }   
        }

        /// <summary>
        /// Draw initial start up cubes
        /// </summary>
        private void generateIntitialFood()
        {
            lock (w)
            {
                for (int i = 0; i < w.maxFood / 3; i++)
                {
                    Cube randomFood = new Cube(R.Next(1, w.GetWidth), R.Next(1, w.GetHeight), RandomColor(R), UID += 1, 0, true, "", w.foodValue);
                    w.ListOfFood.Add(randomFood.GetID(), randomFood);
                }

                for (int i = 0; i < w.numberOfVirus; i++)
                {
                    Cube virusCube = new Cube(R.Next(1, w.GetWidth), R.Next(1, w.GetHeight), -10039894, UID += 1, 0, true, "virus", 1000);
                    w.ListOfFood.Add(virusCube.GetID(), virusCube);
                }

                VirusList = FindVirus();
            }
        }

        /// <summary>
        /// Generate random color
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private int RandomColor(Random r)
        {
            KnownColor[] colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            
            
            KnownColor randColor = colors[r.Next(0, colors.Length)];
            int colorCode = Color.FromKnownColor(randColor).ToArgb();
            
            if (colorCode == -331546)    //don't want pink
                RandomColor(r);
            if (colorCode == -4139)
                RandomColor(r);
            if (colorCode == 16777215)
                RandomColor(r);
            if (colorCode == -986896)
                RandomColor(r);
            
            return colorCode;
        }

        /// <summary>
        /// Will return colors that are easy to see
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private int PlayerColor(Random r)
        {
            KnownColor[] colors = new KnownColor[13] { KnownColor.Red, KnownColor.Indigo, KnownColor.Blue, KnownColor.Purple, KnownColor.Cyan, KnownColor.DarkGray, KnownColor.Green, KnownColor.DeepSkyBlue,
            KnownColor.Orange, KnownColor.Olive, KnownColor.Teal, KnownColor.Chocolate, KnownColor.BlueViolet};
            KnownColor randColor = colors[r.Next(0, colors.Length)];
            int colorCode = Color.FromKnownColor(randColor).ToArgb();
            return colorCode;
        }

        /// <summary>
        /// Generate random ID
        /// </summary>
        /// <returns></returns>
        private int GenerateUID()
        {
            Random rnd = new Random();
            int uid = rnd.Next(10000);
            while (w.ListOfPlayers.ContainsKey(uid) || w.ListOfFood.ContainsKey(uid))
            {
                uid = rnd.Next(10000);
            }
            return uid;
        }

        /// <summary>
        /// Determines if a food is overlapping a player cube
        /// </summary>
        /// <param name="playerCube"></param>
        /// <returns></returns>
        private Cube foodEaten(Cube playerCube)
        {
            double offset = 1.5;
            foreach (Cube c in w.ListOfFood.Values)
            {
                if (playerCube.GetX() > (int)c.GetX() - (playerCube.GetWidth() * offset) && playerCube.GetX() < (int)c.GetX() + (playerCube.GetWidth() * offset))
                {
                    if (playerCube.GetY() > (int)c.GetY() - (playerCube.GetWidth() * offset) && playerCube.GetY() < (int)c.GetY() + (playerCube.GetWidth() * offset))
                    {
                        return c;
                    }  
                }
            }
            return null;
        }
      
        /// <summary>
        /// Helper method to move the cube.
        /// </summary>
        private void Move()
        {
            int xold;
            int yold;
            Cube temp;
            Tuple<int, int> pair;
            int speed;
            int offset;
            List<Cube> partnerCubeList = new List<Cube>();
            List<Cube> cubesThatNeedToDie = new List<Cube>();
            Rectangle tempRectangleF;
            Rectangle tempRactangle2;
            lock (w)
            {
                foreach (KeyValuePair<Socket, Tuple<int, int>> s in Destination.ToList())
                {
                    sockets.TryGetValue(s.Key, out temp);
                    rectangles.TryGetValue(temp.uid, out tempRectangleF);
                    if (FindTeamCubes(temp.team_id).Count > 1)
                        foreach (Cube c in FindTeamCubes(temp.team_id).ToList())
                        {
                            if (c.GetID() == temp.GetID()) { }
                            else if (cubesThatNeedToDie != null && cubesThatNeedToDie.Contains(c)) { }
                            else if (temp.team_id == c.team_id)
                            {
                                List<Cube> list = FindTeamCubes(temp.team_id);
                                rectangles.TryGetValue(c.uid, out tempRectangleF);
                                //Speed is inversely related to the mass
                                speed = (w.maxSize / c.GetMass());
                                //If speed exceeds topspeed, then reassign the topspeed as the speed
                                if (speed > w.topSpeed)
                                {
                                    speed = w.topSpeed;
                                }
                                offset = c.GetWidth();
                                xold = (int)c.GetX();
                                yold = (int)c.GetY();

                                //pair is target destination
                                pair = s.Value;
                                //move toward target destination


                                if (xold < pair.Item1 + offset && xold > pair.Item1 - offset)  //it's in the right spot, doesn't need to move
                                { }
                                else if (pair.Item1 > xold)  //needs to move to the right
                                {
                                    foreach (Cube t in list)
                                    {
                                        rectangles.TryGetValue(t.uid, out tempRactangle2);
                                        if (t.uid != c.uid)
                                        {
                                            if (!tempRectangleF.IntersectsWith(tempRactangle2))
                                            {
                                                c.loc_x = xold + speed;
                                                tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);

                                                rectangles[c.uid] = tempRectangleF;
                                            }
                                            else
                                            {
                                                c.loc_x = xold - 15;
                                                tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);

                                                rectangles[c.uid] = tempRectangleF;
                                            }
                                        }
                                    }
                                }
                                //Move to the left
                                else
                                {
                                    foreach (Cube t in list)
                                    {
                                        rectangles.TryGetValue(t.uid, out tempRactangle2);
                                        if (t.uid != c.uid)
                                        {
                                            if (!tempRectangleF.IntersectsWith(tempRactangle2))
                                            {
                                                c.loc_x = xold - speed;
                                                tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);

                                                rectangles[c.uid] = tempRectangleF;
                                            }
                                            else
                                            {
                                                c.loc_x = xold + 15;
                                                tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);

                                                rectangles[c.uid] = tempRectangleF;
                                            }
                                        }
                                    }
                                }
                                if (yold < pair.Item2 + offset && yold > pair.Item2 - offset)
                                { }
                                //Move down
                                else if (pair.Item2 > yold)
                                {
                                    foreach (Cube t in list)
                                    {
                                        rectangles.TryGetValue(t.uid, out tempRactangle2);
                                        if (t.uid != c.uid)
                                        {
                                            if (!tempRectangleF.IntersectsWith(tempRactangle2))
                                            {
                                                c.loc_y = yold + speed;
                                                tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);

                                                rectangles[c.uid] = tempRectangleF;
                                            }
                                            else
                                            {
                                                c.loc_y = yold - 15;
                                                tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);

                                                rectangles[c.uid] = tempRectangleF;
                                            }
                                        }
                                    }
                                }
                                //move up
                                else
                                {
                                    foreach (Cube t in list)
                                    {
                                        rectangles.TryGetValue(t.uid, out tempRactangle2);
                                        if (t.uid != c.uid)
                                        {
                                            if (!tempRectangleF.IntersectsWith(tempRactangle2))
                                            {
                                                c.loc_y = yold - speed;
                                                tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);

                                                rectangles[c.uid] = tempRectangleF;
                                            }
                                            else
                                            {
                                                c.loc_y = yold + 15;
                                                tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);

                                                rectangles[c.uid] = tempRectangleF;
                                            }
                                        }
                                    }
                                }

                                if (c.loc_x > w.GetWidth)
                                    c.loc_x = xold;
                                if (c.loc_x < 0)
                                    c.loc_x = c.GetWidth();
                                if (c.loc_y > w.GetHeight)
                                    c.loc_y = yold;
                                if (c.loc_y < 0)
                                    c.loc_y = c.GetWidth();

                                //tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);

                                rectangles[c.uid] = tempRectangleF;
                                string message = JsonConvert.SerializeObject(c);

                                w.ListOfPlayers.Remove(c.GetID());
                                sockets[s.Key] = c;
                                w.ListOfPlayers.Add(c.GetID(), c);

                                foreach (Socket tempsocket in sockets.Keys)
                                {
                                    Network.Send(tempsocket, message + "\n");
                                }

                                //Remerge cubes after 10 seconds. Should really be its own function
                                if (stopWatch.ElapsedMilliseconds - c.splitTime > (w.mergeTimer * 1000) && c.splitTime != 0)
                                {
                                    partnerCubeList = FindSplitCubes(c.splitTime);

                                    //Merge cubes into one and kill rest of cubes
                                    cubesThatNeedToDie = Merge(partnerCubeList);
                                    message = "";
                                    foreach(Cube cube in cubesThatNeedToDie)
                                    {
                                        message += JsonConvert.SerializeObject(cube) +"\n";
                                    }
                                    foreach (Socket tempsocket in sockets.Keys)
                                    {
                                        Network.Send(tempsocket, message);
                                    }
                                }

                                //Final remerge into one cube
                                message = "";
                                partnerCubeList = FindSplitCubes(c.splitTime);
                                if(partnerCubeList.Count != 0)
                                {
                                    
                                    Boolean finalTwo = false;
                                    List<Cube> finalcubemerge = new List<Cube>();
                                    if (partnerCubeList.Count == 2)
                                    {
                                        
                                        foreach (Cube finalmerge in partnerCubeList)
                                        {
                                            if(finalmerge.splitTime == 0)
                                            {
                                                finalTwo = true;
                                            }
                                            else
                                            {
                                                finalTwo = false;
                                            }
                                            if (finalTwo)
                                            {
                                                finalcubemerge.Add(finalmerge);
                                            }
                                            
                                        }
                                        if (finalTwo)
                                        {
                                            cubesThatNeedToDie = Merge(finalcubemerge);
                                            foreach (Cube deadCube in cubesThatNeedToDie)
                                            {
                                                message += JsonConvert.SerializeObject(deadCube) +"\n";
                                            }

                                            foreach (Socket tempsocket in sockets.Keys)
                                            {
                                                Network.Send(tempsocket, message);
                                            }
                                        }          
                                    }  
                                }
                            }
                        }
                    else
                    {
                        //Speed is inversely related to the mass
                        speed = (w.maxSize / temp.GetMass());
                        //If speed exceeds topspeed, then reassign the topspeed as the speed
                        if (speed > w.topSpeed)
                        {
                            speed = w.topSpeed;
                        }
                        offset = temp.GetWidth();
                        xold = (int)temp.GetX();
                        yold = (int)temp.GetY();
                        w.ListOfPlayers.Remove(temp.GetID());
                        //pair is target destination
                        pair = s.Value;
                        //move toward target destination
                        if (xold < pair.Item1 + offset && xold > pair.Item1 - offset)
                        { }
                        else if (pair.Item1 > xold)
                            temp.loc_x = xold + speed;
                        else
                            temp.loc_x = xold - speed;

                        if (yold < pair.Item2 + offset && yold > pair.Item2 - offset)
                        { }
                        else if (pair.Item2 > yold)
                            temp.loc_y = yold + speed;
                        else
                            temp.loc_y = yold - speed;

                        sockets[s.Key] = temp;

                        tempRectangleF = new Rectangle((int)(temp.loc_x - temp.GetWidth() * 1.5), (int)(temp.loc_y - temp.GetWidth() * 1.5), temp.GetWidth() * 3, temp.GetWidth() * 3);
                        rectangles[temp.uid] = tempRectangleF;
                        w.ListOfPlayers.Add(temp.GetID(), temp);
                    }
                }
            }
        }

        /// <summary>
        /// Method will merge all cubes that have the same split time
        /// </summary>
        private List<Cube> Merge(List<Cube> partnerCubeList)
        {
            lock(w)
            {
                List<Cube> cubesToKill = new List<Cube>();
                Cube combinedCube = null;
                Cube c;
                Cube partnerCube;
                Socket tempsocket = null;
                double combinedMass = 0;
                bool mainCubeDetected = false;
                Rectangle tempRectangleF;
                Cube[] cubeArray = new Cube[partnerCubeList.Count];
                int i = 0;
                List<Cube> remainingPartnerCubes = partnerCubeList;

                foreach (Cube x in partnerCubeList)
                {
                    cubeArray[i] = x;
                    i++;
                    combinedMass += x.Mass;
                    if (cubetosockets.TryGetValue(x, out tempsocket))
                    {
                        mainCubeDetected = true;
                    }
                }

                if (mainCubeDetected)
                {
                    if (cubetosockets.TryGetValue(cubeArray[0], out tempsocket))
                    {
                        c = cubeArray[0];
                        partnerCube = cubeArray[1];
                    }
                    else
                    {
                        cubetosockets.TryGetValue(cubeArray[1], out tempsocket);
                        c = cubeArray[1];
                        partnerCube = cubeArray[0];
                    }
                    combinedCube = new Cube(c.loc_x, c.loc_y, c.argb_color, c.uid, c.team_id, false, c.Name, combinedMass);
                    combinedCube.splitTime = 0;
                    cubetosockets.Remove(partnerCube);
                    sockets[tempsocket] = combinedCube;
                    cubetosockets.Remove(c);
                    cubetosockets.Add(combinedCube, tempsocket);
                    partnerCube.Mass = 0;
                    rectangles.Remove(partnerCube.uid);
                    tempRectangleF = new Rectangle((int)(combinedCube.loc_x - combinedCube.GetWidth() * 1.5), (int)(combinedCube.loc_y - combinedCube.GetWidth() * 1.5),
                        combinedCube.GetWidth() * 3, combinedCube.GetWidth() * 3);
                    rectangles[c.uid] = tempRectangleF;
                    w.ListOfPlayers.Remove(partnerCube.uid);
                    w.ListOfPlayers[c.uid] = combinedCube;
                    
                    string message1 = JsonConvert.SerializeObject(partnerCube) + "\n";
                    string message2 = JsonConvert.SerializeObject(combinedCube) + "\n";

                    List<Cube> teamcubes = FindTeamCubes(c.team_id);
                    foreach (Cube cube in teamcubes)
                    {
                        cube.numberOfSplits--;
                        if(cube.numberOfSplits < 0)
                        {
                            cube.numberOfSplits = 0;
                        }
                    }

                    cubesToKill.Add(partnerCube);
                    return cubesToKill;
                }
                else
                {
                    c = cubeArray[0];
                    combinedCube = new Cube(c.loc_x, c.loc_y, c.argb_color, c.uid, c.team_id, false, c.Name, combinedMass);
                    combinedCube.splitTime = 0;
                    tempRectangleF = new Rectangle((int)(combinedCube.loc_x - combinedCube.GetWidth() * 1.5), (int)(combinedCube.loc_y - combinedCube.GetWidth() * 1.5),
                        combinedCube.GetWidth() * 3, combinedCube.GetWidth() * 3);
                    rectangles[c.uid] = tempRectangleF;
                    w.ListOfPlayers[c.uid] = combinedCube;

                    remainingPartnerCubes.Remove(c);

                    foreach (Cube x in remainingPartnerCubes)
                    {
                        partnerCube = x;
                        cubetosockets.Remove(partnerCube);
                        partnerCube.Mass = 0;
                        rectangles.Remove(partnerCube.uid);
                        w.ListOfPlayers.Remove(partnerCube.uid);
                        cubesToKill.Add(partnerCube);
                    }

                    List<Cube> teamcubes = FindTeamCubes(c.team_id);
                    foreach (Cube cube in teamcubes)
                    {
                        cube.numberOfSplits--;
                        if (cube.numberOfSplits < 0)
                        {
                            cube.numberOfSplits = 0;
                        }
                    }
                    return cubesToKill;
                }
            }
        }

        /// <summary>
        /// Method will find all the cubes that have the same teamID as the cube that needs to move
        /// </summary>
        /// <param name="teamID"></param>
        /// <returns></returns>
        private List<Cube> FindTeamCubes(double teamID)
        {
            List<Cube> cubes = new List<Cube>();
            if (w.ListOfPlayers.Values.Count != 0)
            {
                foreach (Cube c in w.ListOfPlayers.Values)
                {
                    if (teamID == c.team_id)
                    {
                        cubes.Add(c);
                    }
                }
                return cubes;
            }
            else
                return null;
        }

        /// <summary>
        /// Method will find all the split cubes
        /// </summary>
        /// <param name="splitTime"></param>
        /// <returns></returns>
        private List<Cube> FindSplitCubes(long splitTime)
        {
            List<Cube> cubes = new List<Cube>();
            if (w.ListOfPlayers.Values.Count != 0)
            {
                foreach (Cube c in w.ListOfPlayers.Values)
                {
                    if (splitTime == c.splitTime)
                    {
                        cubes.Add(c);
                    }
                }
                return cubes;
            }
            else
                return null;
        }

        /// <summary>
        /// Method will find the viruses
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, Cube> FindVirus ()
        {
            Dictionary<int, Cube> virusList = new Dictionary<int, Cube>();
            foreach (Cube v in w.ListOfFood.Values)
            {
                if (v.Name != "")
                    virusList.Add(v.uid, v);
            }

            return virusList;
        }

        /// <summary>
        /// Will make a virus
        /// </summary>
        private void MakeVirus()
        {
            Cube virusCube = new Cube(R.Next(1, w.GetWidth), R.Next(1, w.GetHeight), -10039894, UID += 1, 0, true, "virus", w.virusSize);
            w.ListOfFood.Add(virusCube.GetID(), virusCube);
        }

        /// <summary>
        /// Method checks to see if the a virus has been eaten
        /// </summary>
        /// <param name="playerCube"></param>
        /// <returns></returns>
        private Cube VirusEaten(Cube playerCube)
        {
            double offset = 1.5;
            foreach (Cube v in VirusList.Values)
            {
                if (playerCube.GetX() > (int)v.GetX() - (playerCube.GetWidth() * offset) && playerCube.GetX() < (int)v.GetX() + (playerCube.GetWidth() * offset))
                {
                    if (playerCube.GetY() > (int)v.GetY() - (playerCube.GetWidth() * offset) && playerCube.GetY() < (int)v.GetY() + (playerCube.GetWidth() * offset))
                    {
                        return v;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Method accepts a cube to be split. Will deserialize the cube and send back a message as it's return type
        /// </summary>
        /// <param name="tempCube"></param>
        /// <returns></returns>
        private string Split(Cube tempCube)
        {
            long splittime = 0;
            Boolean first = true;
            Rectangle tempRectangleF;           
            string message = "";

            if (tempCube.Mass > w.minimumSplitMass && FindTeamCubes(tempCube.team_id) != null)
            {
                foreach (Cube c in FindTeamCubes(tempCube.team_id))
                {
                    c.Mass = c.Mass / 2;
                    c.loc_x -= 100;
                    c.loc_y -= 100;
                    c.numberOfSplits++;

                    tempRectangleF = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3,
                        c.GetWidth() * 3);
                    rectangles[c.uid] = tempRectangleF;

                    Cube split = new Cube(c.loc_x + 200, c.loc_y + 200, c.argb_color, GenerateUID(), c.team_id, false, c.Name, c.Mass);
                    split.numberOfSplits = c.numberOfSplits;
                    tempRectangleF = new Rectangle((int)(split.loc_x - split.GetWidth() * 1.5), (int)(split.loc_y - split.GetWidth() * 1.5), split.GetWidth()
                        * 3, split.GetWidth() * 3);
                    rectangles.Add(split.uid, tempRectangleF);

                    if (c.splitTime == 0)
                    {
                        c.splitTime = stopWatch.ElapsedMilliseconds;
                        split.splitTime = c.splitTime;
                    }
                    else
                    {
                        if (first)
                        {
                            splittime = stopWatch.ElapsedMilliseconds;
                            split.splitTime = splittime;
                            first = false;
                        }
                        else
                        {
                            split.splitTime = splittime;
                        }
                    }
                    w.ListOfPlayers.Add(split.uid, split);

                    message += (JsonConvert.SerializeObject(split) + "\n");
                    message += (JsonConvert.SerializeObject(c) + "\n");
                }
            }
            return message;
        }
    }
}
