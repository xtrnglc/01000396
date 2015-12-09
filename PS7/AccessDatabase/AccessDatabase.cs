using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseController
{
    public static class AccessDatabase
    {
        public const string connectionString = "server=atr.eng.utah.edu;database=cs3500_trungl;uid=cs3500_trungl;password=PSWRD";

        /// <summary>
        /// Inserts a new entry into the database
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="playername"></param>
        /// <param name="timealive"></param>
        /// <param name="maximumass"></param>
        /// <param name="cubeseaten"></param>
        /// <param name="timeofdeath"></param>
        /// <param name="playerseaten"></param>
        public static void Insert(int playerid, string playername, int timealive, double maximumass, int cubeseaten, int timeofdeath, int sessionid, List<string> playerseaten)
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();

                    //Insert into general info table
                    command.CommandText = "insert into PlayersTable1 (PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath) values('" + playername + "', " + timealive.ToString()
                                           + ", " + maximumass.ToString() + ", " + cubeseaten.ToString() + ", " + timeofdeath.ToString() + ");";
                    command.ExecuteNonQuery();


                    //Insert into player eaten table
                    if(playerseaten.Count > 0)
                    {
                        foreach(string name in playerseaten)
                        {
                            command.CommandText = "insert into PlayersEatenTable (GameSessionID, PredatorName, PreyName) values(" + sessionid.ToString() +", '" + playername + "', " + "'" + name + "');";
                            command.ExecuteNonQuery();
                        }
                    }    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Get a unique sessionid for every client
        /// </summary>
        /// <returns></returns>
        public static int getGameSession()
        {
            int sessionID = 1;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "select , GameSessionID from PlayersTable1";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sessionID++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return sessionID;
        }


        /// <summary>
        /// Deletes a player from the database
        /// </summary>
        /// <param name="playerid"></param>
        public static void Delete(int playerid)
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "delete from PlayersTable2 where PlayerID = '" + playerid.ToString() + "';";
                    command.ExecuteNonQuery();
                    command.CommandText = "delete from PlayersTable1 where PlayerID = '" + playerid.ToString() + "';";
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
