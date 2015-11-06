//Adam Sorensen and Trung Le
//CS 3500 PS7: AgCubio
//Nov 5th 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int Mass { get; set; }

        public int Width
        {
            get { return Mass / 2; }
            private set { Mass = value * 2; }
        }

        public Cube()
        {
            Mass = 100;
            Console.WriteLine("In constructor");
        }

        public Cube(int Mass, int sass)
        {
            this.x = Mass;
            Console.WriteLine("in 2nd constructor");
        }
    }
}
