//Adam Sorensen and Trung Le
//CS 3500 PS7: AgCubio
//Nov 5th 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.

namespace Model
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
        public int x;
        public int y;
        private int z;
        public int Color;
        public string UID;
        public string Name;
        bool Food;

        public int Mass { get; set; }

        /// <summary>
        /// Returns the X and Y coorindates of the cube
        /// </summary>
        public int[] Coordinates
        {
            get
            {
                int[] position = new int[2];
                position[0] = x;
                position[1] = y;
                return position;
            }
            protected set
            {
                //not sure
            }
        }

        /// <summary>
        /// Returns the width of the cube from the Mass
        /// </summary>
        public int Width
        {
            get { return (int)Math.Sqrt(Mass); }
            private set { Mass = value * value; }
        }

        /// <summary>
        /// Constructor for the Cube class that has 0 arguements
        /// </summary>
        public Cube()
        {
            Mass = 100;
            Console.WriteLine("In constructor");
        }

        /// <summary>
        /// constructor for food?
        /// </summary>
        /// <param name="Mass"></param>
        public Cube(int Mass)
        {
            Food = true;
            //not sure if needed
            this.x = Mass;
            this.Mass = Mass;
            Console.WriteLine("in 2nd constructor");
        }

        /// <summary>
        /// Constructor for player cubes (non food cubes)
        /// </summary>
        /// <param name="name"></param>
        public Cube(string name)
        {
            this.Mass = 100;
            this.Food = false;
            this.Name = name;
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
