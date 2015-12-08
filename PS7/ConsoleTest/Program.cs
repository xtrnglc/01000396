using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConsoleTest
{
    class Program
    {
        public const string connectionString = "server=atr.eng.utah.edu;database=cs3500_trungl;uid=cs3500_trungl;password=PSWRD";

        static void Main(string[] args)
        {
            AllPeople();
            Insert();
            Console.Read();
        }

        public static void Insert()
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    int playerid = 12;
                    string playername = "joh";
                    int timealive = 1000;
                    int maximumass = 2304;
                    int cubeseaten = 2305;
                    int timeofdeath = 2345;

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "insert into PlayersTable2 (PlayerID, PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath) values(100, 'Magina', 3000, 4859, 3248, 3000);";
                    //command.CommandText = "delete from PlayersTable2 where PlayerName = 'Sam';";
                    command.CommandText = "insert into PlayersTable2 (PlayerID, PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath) values(" + playerid.ToString() + ", '" + playername + "', " + timealive.ToString()
                        + ", " + maximumass.ToString() + ", " + cubeseaten.ToString() + ", " + timeofdeath.ToString() + ");";
                    command.ExecuteNonQuery();
                    command.CommandText = "insert into PlayersTable1 (PlayerID, PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath, PlayersEaten) values(100, 'Magina', 3000, 4859, 3248, 3000, 'Rex');";
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void AllPeople()
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "select PlayerID, PlayerName, TimeAlive from PlayersTable1";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["PlayerID"] + " " + reader["PlayerName"] + " " + reader["TimeAlive"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
