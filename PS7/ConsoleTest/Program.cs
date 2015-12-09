using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DatabaseController;

namespace ConsoleTest
{
    class Program
    {
        public const string connectionString = "server=atr.eng.utah.edu;database=cs3500_trungl;uid=cs3500_trungl;password=PSWRD";

        static void Main(string[] args)
        {
            AccessDatabase.updateRanking("test", 1500, 12);
            Console.Read();
        }

        public static void Insert()
        {
            
        }

       
        
    }
}
