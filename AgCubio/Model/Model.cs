//Adam Sorensen and Trung Le
//CS 3500 PS7: AgCubio
//Nov 5th 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace AgCubio
{
    public class Model
    {
        
    }

    /// <summary>
    /// Cube class that will keep track of cube attributes
    /// 
    /// </summary>
    public class Cube
    {
        public int loc_x;
        public int loc_y;
        public int argb_color;
        public int uid;
        public string Name;
        public bool Food;

        public int Mass { get; set; }

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
        }

        public int GetColor()
        {
            return this.argb_color;
        }

        public int GetX()
        {
            return loc_x;
        }

        public int GetY()
        {
            return loc_y;
        }

        public string GetName()
        {
            return Name;
        }
        
        public int GetMass()
        {
            return Mass;
        }
        
    }

    /// <summary>
    /// Public class of the world to be communicated to the GUI
    /// </summary>
    public class World
    {
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
        /// Constructor for the world
        /// not sure if you set it to a fixed amount from the beginning or compute it from the cubes?
        /// 
        /// </summary>
        public World()
        {
            Width = 1000;
            Height = 1000;
        }

        
    }
}
