using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgCubio
{
    /// <summary>
    /// Representation of a cube in the game logic.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Cube
    {
        /// <summary>
        /// The team id of the cube. This will be the same as the uid if the cubes belong to the player
        /// </summary>
        public long team_id;

        /// <summary>
        /// The horizontal location of the cube
        /// </summary>
        [JsonProperty]
        public float loc_x;

        /// <summary>
        /// The vertical location of the cube
        /// </summary>
        [JsonProperty]
        public float loc_y;

        /// <summary>
        /// The color of the cube
        /// </summary>
        [JsonProperty]
        public int argb_color;

        /// <summary>
        /// The unique ID of the cube
        /// </summary>
        [JsonProperty]
        public long uid;

        /// <summary>
        /// Field that tells whether or not the cube is food. True if so, false if otherwise.
        /// </summary>
        [JsonProperty]
        public bool food;

        /// <summary>
        /// Getter and setter for the name of the cube
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// The mass of the food
        /// </summary>
        public double _mass;

        /// <summary>
        /// Getter and setter for the width of a cube
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Getter and setter for the center of a cube
        /// </summary>
        public int Center { get; set; }

        /// <summary>
        /// Constructor for a Cube object
        /// </summary>
        public Cube(float _loc_x, float _loc_y, int _argb_color, bool _food, String _name, double _Mass)
        {
            loc_x = _loc_x;
            loc_y = _loc_y;
            argb_color = _argb_color;
            uid = 0;
            team_id = uid;
            food = _food;
            Name = _name;
            Mass = _Mass;
        }

        /// <summary>
        /// Returns true if the cubes belong to the same parent cube, false if otherwise.
        /// </summary>
        /// <returns></returns>
        public bool Equals(Cube inputCube)
        {
            if ((Cube)inputCube == null)
            {
                return false;
            }
            return this.uid == ((Cube)inputCube).uid;
        }

        /// <summary>
        /// Getter and setter for mass of the cube
        /// </summary>
        [JsonProperty]
        public double Mass
        {
            get { return this._mass; }
            set
            {
                this._mass = value;
                this.Width = (int)Math.Pow(this._mass, 0.65);
                this.Center = Width / 2;
            }
        }
    }
}


//Written by Michelle Nguyen and Kenny Ho for CS3500, November 2015.


namespace AgCubio
{
    /// <summary>
    /// Representation of the "state" simulation. This class is responsible
    /// for tracking the world width and height and cubes in the game.
    /// </summary>
    public class World
    {
        /// <summary>
        /// Read-only width field of the World
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// Read-only height field of the World
        /// </summary>
        public readonly int Height;

        /// <summary>
        /// Keeps track of the player cubes
        /// </summary>
        public Dictionary<long, Cube> players;

        /// <summary>
        /// Keeps track of the food cubes
        /// </summary>
        public Dictionary<long, Cube> food;

        /// <summary>
        /// Constructor of the World object
        /// </summary>
        public World()
        {
            players = new Dictionary<long, Cube>();
            food = new Dictionary<long, Cube>();
        }
    }
}
