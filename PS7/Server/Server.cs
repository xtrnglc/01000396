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


namespace Server
{
    class Server
    {
        private int UID = 1;
        private List<Socket> playerSockets = new List<Socket>();
        private string pathToFile = "gamestate.txt";
        //private Cube Player;
        private Dictionary<Socket, Cube> sockets = new Dictionary<Socket, Cube>();
        private Dictionary<Cube, Socket> cubetosockets = new Dictionary<Cube, Socket>();
        private Random R = new Random();
        private Dictionary<int, Rectangle> rectangles = new Dictionary<int, Rectangle>();
        private World w = new World();
        //private Dictionary<int, Cube> splitCubes = new Dictionary<int, Cube>();
        private Dictionary<Socket, Tuple<int, int>> Destination = new Dictionary<Socket, Tuple<int, int>>();
        private int teamid = 0;
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

                                    case "":
                                        break;
                                }
                            }
                        }
                    }
                    Console.WriteLine("Game state XML parsed successfully");
                    w = new World(Width, Height, maxFood, topSpeed, attritionRate, foodValue, startMass, minimumSplitMass, maximumSplits, numberofVirus);
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
            System.Timers.Timer aTimer = new System.Timers.Timer(1000/25);
            System.Timers.Timer attritionTimer = new System.Timers.Timer(3000);
            System.Timers.Timer splitTimer = new System.Timers.Timer(5000);

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
            Cube playerCube = new Cube(200, 200, RandomColor(R), UID, teamid, false, data, w.startMass);
            //RectangleF((float)cube.loc_x - cube.GetWidth() * 1.5f, (float)cube.loc_y - cube.GetWidth() * 1.5f, cube.GetWidth() * 3, cube.GetWidth() * 3);
            Rectangle playerRectangle = new Rectangle((int)(playerCube.loc_x - playerCube.GetWidth() * 1.5), (int)(playerCube.loc_y - playerCube.GetWidth() * 1.5), playerCube.GetWidth() * 3, playerCube.GetWidth() * 3);
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
            rectangles.Add(playerCube.uid, playerRectangle);
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
        }

        /// <summary>
        /// data should be either (move, x, y) or (split, x, y)
        /// </summary>
        private void DataFromClient(State state)
        {
            int x;
            int y;
            int tempID;
            int xold;
            int yold;
            Tuple<int, int> pair;
            Cube temp, split1, split2, split3, split4;
            Rectangle tempRectangle;
            
            string message = "";
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

                                    tempRectangle = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);
                                    rectangles[c.uid] = tempRectangle;

                                    Cube split = new Cube(c.loc_x + 200, c.loc_y + 200, c.argb_color, GenerateUID(), c.team_id, false, c.Name, c.Mass);
                                    tempRectangle = new Rectangle((int)(split.loc_x - split.GetWidth() * 1.5), (int)(split.loc_y - split.GetWidth() * 1.5), split.GetWidth() * 3, split.GetWidth() * 3);
                                    rectangles.Add(split.uid, tempRectangle);

                                    w.ListOfPlayers.Add(split.uid, split);

                                    message += (JsonConvert.SerializeObject(split) + "\n");
                                    message += (JsonConvert.SerializeObject(c) + "\n");
                                    Network.Send(state.workSocket, message);
                                }
                                Network.Send(state.workSocket, message);
                            }
                         }
                    }
                    else
                    {
                        lock (w)
                        {
                            string[] location = command.Split(',');
                            location[2] = location[2].Substring(0, location[2].Length - 1);

                            int.TryParse(location[1], out x);
                            int.TryParse(location[2], out y);

                            if(x > w.GetWidth)
                            {
                                x = w.GetWidth;
                            }
                            if(x < 0)
                            {
                                x = 0;
                            }
                            if(y > w.GetHeight)
                            {
                                y = w.GetHeight;
                            }
                            if(y < 0)
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
            Rectangle tempRectangle;
            
            Cube temp, temp2;

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
                            c.Mass += temp2.Mass;
                            temp2.Mass = 0;
                            //Send the dead cube
                            message += JsonConvert.SerializeObject(temp2) + "\n";
                            message2 += JsonConvert.SerializeObject(c) + "\n";

                            tempRectangle = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);
                            rectangles[c.uid] = tempRectangle;
                        }
                        //Send updated mass of cubes after eating food
                        foreach (Socket s in sockets.Keys)
                        {
                            Network.Send(s, message);
                            Network.Send(s, message2);
                        }
                        while (playerEaten(c) != null)
                        {  
                            //temp2 = eaten
                            //c = eater
                            temp2 = playerEaten(c);                          
                            c.Mass += temp2.Mass; 
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

                            tempRectangle = new Rectangle((int)(c.loc_x - c.GetWidth() * 1.5), (int)(c.loc_y - c.GetWidth() * 1.5), c.GetWidth() * 3, c.GetWidth() * 3);
                            rectangles[c.uid] = tempRectangle;
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

                //for (int i = 0; i < w.numberOfVirus; i++)
                //{
                //    Cube virusCube = new Cube(R.Next(1, w.GetWidth), R.Next(1, w.GetHeight), -10039894, UID += 1, 0, true, "virus", 500);
                //    w.ListOfFood.Add(virusCube.GetID(), virusCube);
                //}
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
        /// Returns true if cubes are colliding
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private int CollisionDetection(double ID)
        {
            Cube main;
            bool xCollision = false, yCollision = false;
            double offset = 1;
            List<Cube> templist = FindTeamCubes(ID);
            foreach (Cube c in FindTeamCubes(ID))
            {
                main = c;
                foreach (Cube u in FindTeamCubes(ID))
                {
                    if (u.GetID() == main.GetID())
                    {
                        //do nothing
                    }
                    else
                    {
                        if (main.GetX() > (int)u.GetX() - (main.GetWidth() * offset) && main.GetX() < (int)u.GetX() + (main.GetWidth() * offset))
                        {
                            yCollision = true;
                        }
                        if (main.GetY() > (int)u.GetY() - (main.GetWidth() * offset) && main.GetY() < (int)u.GetY() + (main.GetWidth() * offset))
                        {
                            xCollision = true;
                        }
                        if (xCollision && yCollision)
                        {
                            return 3;
                        }
                        if (xCollision)
                        {
                            return 1;
                        }
                        if (yCollision)
                        {
                            return 2;
                        }
                        if (!xCollision && !yCollision)
                        {
                            return 0;
                        }
                        
                    }
                }              
            }
            return 0;
        }


        /// <summary>
        /// Generates coordinates that do not overlap player cubes
        /// </summary>
        /// <returns></returns>
        private Tuple<int,int> generateCoordinates()
        {
            Boolean valid = false;
            Tuple<int, int> coordinates;
            int x = 0;
            int y = 0;
            
            double offset = 1.5;
            while (!valid)
            {
                x = R.Next(1, w.GetWidth);
                y = R.Next(1, w.GetHeight);
                foreach (Cube c in w.ListOfPlayers.Values)
                {
                    if (c.GetX() > x - (c.GetWidth() * offset) && c.GetX() < x + (c.GetWidth() * offset))
                    {
                        if (c.GetY() > y - (c.GetWidth() * offset) && c.GetY() < y + (c.GetWidth() * offset))
                        {
                            valid = true;
                            break;
                        }
                    }
                }
            }
            coordinates = new Tuple<int, int>(x, y);
            return coordinates;
        }

        private bool CubeIdExists(double ID)
        {
            foreach (Cube c in w.ListOfPlayers.Values)
            {
                if (ID == c.uid)
                {
                    return true;
                }
            }
            foreach (Cube f in w.ListOfFood.Values)
            {
                if (ID == f.uid)
                {
                    return true;
                }
            }
            return false;
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
            Rectangle tempRectangle;
            Rectangle tempRactangle2;

            foreach (KeyValuePair<Socket, Tuple<int, int>> s in Destination.ToList())
            {
                sockets.TryGetValue(s.Key, out temp);
                rectangles.TryGetValue(temp.uid, out tempRectangle);
                if (FindTeamCubes(temp.team_id).Count > 1)
                    foreach (Cube c in FindTeamCubes(temp.team_id))
                    {
                        if (c.GetID() == temp.GetID()) { }
                        else if (temp.team_id == c.team_id)
                        {
                            List<Cube> list = FindTeamCubes(temp.team_id);
                            rectangles.TryGetValue(c.uid, out tempRectangle);
                            //Speed is inversely related to the mass
                            speed = (10000 / temp.GetMass());
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
                                foreach(Cube t in list)
                                {
                                    rectangles.TryGetValue(t.uid, out tempRactangle2);
                                    if(t.uid != c.uid)
                                    {
                                        if (!tempRectangle.IntersectsWith(tempRactangle2))
                                        {
                                            c.loc_x = xold + speed;
                                        }
                                        else
                                        {
                                            c.loc_x = xold - 15;
                                        }
                                    }       
                                }
                            }
                            //Move to the left
                            else
                            {
                                foreach(Cube t in list)
                                {
                                    rectangles.TryGetValue(t.uid, out tempRactangle2);
                                    if (t.uid != c.uid)
                                    {
                                        if (!tempRectangle.IntersectsWith(tempRactangle2))
                                        {
                                            c.loc_x = xold - speed;
                                        }
                                        else
                                        {
                                            c.loc_x = xold + 15;
                                        }
                                    }  
                                } 
                            }       
                            if (yold < pair.Item2 + offset && yold > pair.Item2 - offset)
                            { }
                            //Move down
                            else if (pair.Item2 > yold)
                            {
                                foreach(Cube t in list)
                                {
                                    rectangles.TryGetValue(t.uid, out tempRactangle2);
                                    if (t.uid != c.uid)
                                    {
                                        if (!tempRectangle.IntersectsWith(tempRactangle2))
                                        {
                                            c.loc_y = yold + speed;
                                        }
                                        else
                                        {
                                            c.loc_y = yold - 15;
                                        }
                                    }
                                }           
                            }
                            //move up
                            else
                            {
                                foreach(Cube t in list)
                                {
                                    rectangles.TryGetValue(t.uid, out tempRactangle2);
                                    if (t.uid != c.uid)
                                    {
                                        if (!tempRectangle.IntersectsWith(tempRactangle2))
                                        {
                                            c.loc_y = yold - speed;
                                        }
                                        else
                                        {
                                            c.loc_y = yold + 15;
                                        }
                                    }  
                                }   
                            }
                            rectangles[c.uid] = tempRectangle;
                            string message = JsonConvert.SerializeObject(c);
                            
                            foreach(Socket tempsocket in sockets.Keys)
                            {
                                Network.Send(tempsocket, message +"\n");
                            }

                            w.ListOfPlayers.Remove(c.GetID());
                            sockets[s.Key] = c;
                            w.ListOfPlayers.Add(c.GetID(), c);
                        }
                    }
                else
                {
                    //Speed is inversely related to the mass
                    speed = (10000 / temp.GetMass());
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

                    tempRectangle = new Rectangle((int)(temp.loc_x - temp.GetWidth() * 1.5), (int)(temp.loc_y - temp.GetWidth() * 1.5), temp.GetWidth() * 3, temp.GetWidth() * 3);
                    rectangles[temp.uid] = tempRectangle;
                    w.ListOfPlayers.Add(temp.GetID(), temp);
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
    }
}
