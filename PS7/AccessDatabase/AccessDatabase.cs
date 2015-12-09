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

                    updateRanking(playername, maximumass, sessionid);

                    //Insert into general info table
                    command.CommandText = "insert into PlayersTable1 (GameSessionID, PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath) values(" + sessionid.ToString()
                        + ", '" + playername + "', " + timealive.ToString()
                                           + ", " + maximumass.ToString() + ", " + cubeseaten.ToString() + ", " + timeofdeath.ToString() + ");";
                    command.ExecuteNonQuery();


                    //Insert into player eaten table
                    if(playerseaten.Count > 0)
                    {
                        foreach(string name in playerseaten)
                        {
                            command.CommandText = "insert into PlayersEatenTable (GameSessionID, PredatorName, PreyName) values(" + sessionid.ToString() +", '" 
                                + playername + "', " + "'" + name + "');";
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

        public static string getScores()
        {
            string htmlString = "<head><style>table, th, td {border: 1px solid black;border-collapse: collapse;}th, td {padding: 5px;text-align: left;}</style></head><body><table style=\"width: 100 % \"><caption>Scores for all players</caption><tr><th>Player Name</th><th>Time Alive</th><th>Time of Death</th><th>Cubes Eaten</th><th>Maximum Mass</th><th>Rank</th>";
            string end = "</table></body>";
            string temp = "";
            string playernametemp;
            string temp2;
            Dictionary<string, string> nametorank = new Dictionary<string, string>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText = "Select Rank, PlayerName from RankingTable";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(reader["PlayerName"].ToString() != "")
                            {
                                nametorank.Add(reader["PlayerName"].ToString(), reader["Rank"].ToString());
                            }
                        }
                    }

                    command.CommandText = "select PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath from PlayersTable1";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            temp = "<tr>" + "<td>" + reader["PlayerName"] + "</td>" + "<td>" + reader["TimeAlive"] + "</td>" + "<td>" + reader["MaximumMass"] + "</td>" + "<td>" + reader["CubesEaten"] + "</td>" + "<td>" + reader["TimeOfDeath"] + "</td>";
                            
                            playernametemp = reader["PlayerName"].ToString();

                            if(nametorank.TryGetValue(playernametemp, out temp2))
                            {
                                temp += "<td>" + temp2 + "</td></tr>";
                            }
                            else
                            {
                                temp += "<td> - </td></tr>";
                            }
                            htmlString += temp;

                        }
                    }

                    htmlString += end;
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return htmlString;
        }

        /// <summary>
        /// Updates the top 5 all time rankings
        /// </summary>
        /// <param name="playername"></param>
        /// <param name="maximumass"></param>
        /// <param name="sessionid"></param>
        public static void updateRanking(string playername, double maximumass, int sessionid)
        {
            Dictionary<double, string> rankings = new Dictionary<double, string>();
            string entryValue;
            double massKey;
            List<double> toSort = new List<double>();
            string temp;
            string[] substrings;
            int r = 1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "select Rank, GameSessionID, PlayerName, MaximumMass from RankingTable";

                    //Get the current rankings
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(double.TryParse(reader["MaximumMass"].ToString(), out massKey))
                            {
                                entryValue = (reader["Rank"] + "&" + reader["PlayerName"] + "&" + reader["MaximumMass"] + "&" + reader["GameSessionID"]);
                                rankings.Add(massKey, entryValue); //Will run into issues if two players have the exact same mass
                                toSort.Add(massKey);
                            }
                        }
                    }

                    //Add the new player ranking
                    toSort.Add(maximumass);

                    //Sort the rankings
                    toSort.Sort();
                    toSort.Reverse();

                    //Only keep top 5
                    if(toSort.Count > 5)
                    {
                        toSort.RemoveAt(5);
                    }

                    string mock = "1&" + playername + "&" + maximumass + "&" + sessionid;
                    rankings.Add(maximumass, mock);

                    //Update table 
                    foreach(double d in toSort)
                    {
                        rankings.TryGetValue(d, out temp);
                        substrings = temp.Split('&');
                        command.CommandText = "update RankingTable set PlayerName='" + substrings[1]  + "', MaximumMass='"+ substrings[2] + "', GameSessionID='" + substrings[3] 
                            +"' WHERE Rank='" + r + "';";
                        command.ExecuteNonQuery();
                        r++;
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
        public static int getGameSession(int startsession)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "select GameSessionID from PlayersTable1";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            startsession++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return startsession;
        }
    }
}
