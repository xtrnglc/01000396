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
        /// variabes for the cube class to keep track of position, color, ID, name and if it's food or not.
        /// </summary>
        private int loc_x;
        private int loc_y;
        private int argb_color;
        private int uid;
        private string Name;
        private bool Food;
        public int Mass { get; set; }
        private int team_id;

        /// <summary>
        /// Constructor for the Cube class. Takes in 7 arguments
        /// </summary>
        public Cube(int x, int y, int color, int ID, bool food, string name, int mass)
        {
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
        public int GetX()
        {
            return loc_x;
        }

        /// <summary>
        /// Returns the y coorindate of the cube
        /// </summary>
        /// <returns></returns>
        public int GetY()
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
            return Mass;
        }

        public int GetColor()
        {
            return argb_color;
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
        public readonly int Width;
        public readonly int Height;
        private HashSet<Cube> WorldPopulation = new HashSet<Cube>();

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
        /// Constructor for the world
        /// not sure if you set it to a fixed amount from the beginning or compute it from the cubes?
        /// 
        /// </summary>
        public World()
        {
            Width = 10000;
            Height = 10000;
        }
    }
}