using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Database
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
        public static void Insert(int playerid, string playername, int timealive, double maximumass, int cubeseaten, int timeofdeath, string playerseaten)
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "insert into PlayersTable2 (PlayerID, PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath) values(" + playerid.ToString() + ", '" + playername + "', " + timealive.ToString()
                        + ", " + maximumass.ToString() + ", " + cubeseaten.ToString() + ", " + timeofdeath.ToString() + ");";
                    command.ExecuteNonQuery();
                    command.CommandText = "insert into PlayersTable1 (PlayerID, PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath, PlayersEaten) values(" + playerid.ToString() + ", '" + playername + "', " + timealive.ToString()
                                           + ", " + maximumass.ToString() + ", " + cubeseaten.ToString() + ", " + timeofdeath.ToString() + ", '" + playerseaten +"');"; command.ExecuteNonQuery();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
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
