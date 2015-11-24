//Adam Sorensen and Trung Le
//CS 3500 PS7: AgCubio
//Nov 5th 2015
//Model class to keep track of the cube and world  classes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace AgCubio
{
    /// <summary>
    /// Cube class that will keep track of cube attributes
    /// 
    /// </summary>
    public class Cube
    {
        /// <summary>
        /// variabes for the cube class to keep track of position, color, ID, team ID, name and if it's food or not.
        /// </summary>
        public double loc_x { get; set; }                               //JSON PROPERTY?
        public double loc_y { get; set; }
        public double argb_color { get; set; }
        public double uid { get; set; }
        public string Name { get; set; }
        public bool Food { get; set; }
        public double Mass { get; set; }
        public double team_id { get; set; }                             //JSON PROPERTY?????

        /// <summary>
        /// Constructor for the Cube class. Takes in 7 arguments
        /// </summary>
        public Cube(double x, double y, double color, double ID, double teamID, bool food, string name, double mass)
        {
            //{"loc_x":689.0,"loc_y":498.0,"argb_color":-16777216,"uid":5656,"team_id":0,"food":false,"Name":"cow","Mass":1000.0}
            this.loc_x = x;
            this.loc_y = y;
            this.argb_color = color;
            this.uid = ID;
            this.Food = food;
            this.Name = name;
            this.Mass = mass;
            this.team_id = ID;
        }

        /// <summary>
        /// returns the x coorindate of the cube
        /// </summary>
        /// <returns></returns>
        public double GetX()                             //Could you just use the getter?
        {
            return loc_x;
        }

        /// <summary>
        /// Returns the square root of the mass
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return (int)Math.Sqrt(Mass);
        }

        /// <summary>
        /// Returns the y coorindate of the cube
        /// </summary>
        /// <returns></returns>
        public double GetY()
        {
            return loc_y;
        }

        /// <summary>
        /// Return the name of the cube if it has one
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return Name;
        }

        /// <summary>
        /// Returns the mass of the cube
        /// </summary>
        /// <returns></returns>
        public int GetMass()
        {
            return (int)Mass;
        }
        /// <summary>
        /// Returns the color of the cube
        /// </summary>
        /// <returns></returns>
        public double GetColor()
        {
            return argb_color;
        }

        /// <summary>
        /// Returns status, food or not food
        /// </summary>
        /// <returns></returns>
        public bool GetFood()
        {
            return Food;
        }
        /// <summary>
        /// Returns unique id
        /// </summary>
        public int GetID
        {
            get { return (int)uid; }
        }
    }

    /// <summary>
    /// Public class of the world to be communicated to the GUI
    /// </summary>
    public class World
    {
        /// <summary>
        /// Data to hold information about the world, needs to keep track of width, height and how many cubes are there
        /// </summary>
        private readonly int Width;
        private readonly int Height;
        public Dictionary<int, Cube> ListOfPlayers;                             //PUBLIC VARIABLES???? WHAAAT.
        public Dictionary<int, Cube> ListOfFood;                             //Good idea for the food list though.
        private readonly double topSpeed;
        private readonly double lowSpeed;
        private readonly double attritionRate;
        private readonly double foodValue;
        private readonly int maxFood;
        private readonly double minimumSplitMass;
        private readonly double maximumSplitDistance;
        private readonly int maximumSplits;
        private readonly double absorbDistanceDelta;

//Width, Height - the game size of the world
//Heartbeats per second - how many updates the server should attempt to execute per second. Note: adequate work will simply update the world as fast as possible.
//Top speed - how fast cubes can move when small
//Low speed - how fast the biggest cubes move
//Attrition rate - how fast cubes lose mass
//Food value - the default mass of food
//Player start mass - the default starting mass of the player.
//Max food - how many food to maintain in the world. Server should update one food per heartbeat if below this.
//Minimum split mass - players are not allowed to split if below this mass
//Maximum split distance - how far a cube can be "thrown" when split
//Maximum splits - how many total cubes a single player is allowed. Note: our test server does not implement this. Try setting it to around 10-20.
//Absorb distance delta - how close cubes have to be in order for the larger to eat the smaller

        /// <summary>
        /// Return the Height of the World
        /// </summary>
        public int GetHeight
        {
            get { return Height; }
        }

        /// <summary>
        /// Return the Width of the World
        /// </summary>
        public int GetWidth
        {
            get { return Width; }
        }

        
        /// <summary>
        /// Adds cube to respective world
        /// </summary>
        /// <param name="c"></param>
        public void Add(Cube c)
        {
            if (c.GetFood() == true)
            {
                ListOfFood.Add(c.GetID, c);
            }
            else
            {
                ListOfPlayers.Add(c.GetID, c);
            }
            
        }

        /// <summary>
        /// Constructor for the world
        /// not sure if you set it to a fixed amount from the beginning or compute it from the cubes?
        /// 
        /// </summary>
        public World()
        {
            Width = 1000;
            Height = 1000;
            ListOfPlayers = new Dictionary<int, Cube>();
            ListOfFood = new Dictionary<int, Cube>();
        }
    }
}