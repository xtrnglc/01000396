﻿//Adam Sorensen and Trung Le
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
        /// 
        [JsonProperty]
        public double loc_x { get; set; }
        [JsonProperty]//JSON PROPERTY?
        public double loc_y { get; set; }
        [JsonProperty]
        public int argb_color { get; set; }
        [JsonProperty]
        public int uid { get; set; }
        [JsonProperty]
        public int team_id { get; set; }
        [JsonProperty]
        public bool food { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public double Mass { get; set; }


        //NOT JSON PROPERTY DO NOT SEND 
        //So this might be why our server is not working with Jim's client. Because we send over cube info his cube class does not have
        //However it works on a friend's client and debugging their client it seems like it ignores these two fields
        public int numberOfSplits { get; set; }
        public long splitTime { get; set; }
        
        /// <summary>
        /// Constructor for the Cube class. Takes in 7 arguments
        /// </summary>
        public Cube(double x, double y, int color, int ID, int teamID, bool foodtemp, string name, double mass)
        {
            //{"loc_x":689.0,"loc_y":498.0,"argb_color":-16777216,"uid":5656,"team_id":0,"food":false,"Name":"cow","Mass":1000.0}
            this.loc_x = x;
            this.loc_y = y;
            this.argb_color = color;
            this.uid = ID;
            this.food = foodtemp;
            this.team_id = teamID;
            this.Name = name;
            this.Mass = mass;
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
            return food;
        }
        /// <summary>
        /// Returns unique id
        /// </summary>
        public int GetID()
        {
            return (int)uid;
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
        public Dictionary<int, Cube> ListOfPlayers;                             //PUBLIC VARIABLES???? WHAAAT.
        public Dictionary<int, Cube> ListOfFood;                             //Good idea for the food list though.
        public int maxFood;
        public int topSpeed;
        public int attritionRate;
        public int foodValue;
        public int startMass;
        public int minimumSplitMass;
        public int maximumSplitDistance;
        public int maximumSplits;
        public int numberOfVirus;
        public int maxSize;
        public int virusSize;
        public int mergeTimer;
        public int attritionTimer;
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
                ListOfFood.Add(c.GetID(), c);
            }
            else
            {
                ListOfPlayers.Add(c.GetID(), c);
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
            maxFood = 2000;
            topSpeed = 500;
            attritionRate = 10;
            foodValue = 20;
            startMass = 1000;
            minimumSplitMass = 100;
            numberOfVirus = 0;
            //maximumSplitDistance;
            maximumSplits = 10;
            maxSize = 15000;
            mergeTimer = 10;
            virusSize = 1000;
            attritionTimer = 3;
        }

        /// <summary>
        /// explicit value constructor
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="maxfood"></param>
        /// <param name="topspeed"></param>
        /// <param name="attrition"></param>
        /// <param name="foodvalue"></param>
        /// <param name="startmass"></param>
        /// <param name="minsplitmass"></param>
        /// <param name="maxsplits"></param>
        public World(int width, int height, int maxfood, int topspeed, int attrition, int foodvalue, int startmass, int minsplitmass, int maxsplits, int numVirus, int maxsize, int mergetime, int virussize, int atimer)
        {
            Width = width;
            Height = height;
            ListOfPlayers = new Dictionary<int, Cube>();
            ListOfFood = new Dictionary<int, Cube>();
            maxFood = maxfood;
            topSpeed = topspeed;
            attritionRate = attrition;
            foodValue = foodvalue;
            startMass = startmass;
            minimumSplitMass = minsplitmass;
            //maximumSplitDistance;
            maximumSplits = maxsplits;
            numberOfVirus = numVirus;
            maxSize = maxsize;
            mergeTimer = mergetime;
            virusSize = virussize;
            attritionTimer = atimer;
        }
    }
}