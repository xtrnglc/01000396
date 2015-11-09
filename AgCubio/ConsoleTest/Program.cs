using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgCubio;
using Newtonsoft.Json;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Cube cube = new Cube(30, 40, -2387546, 57, true, "test", 1000);
            string message = JsonConvert.SerializeObject(cube);
            Cube rebuilt = JsonConvert.DeserializeObject<Cube>(message);
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}
