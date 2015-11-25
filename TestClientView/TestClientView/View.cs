using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestClientView;


namespace View
{
    /// <summary>
    /// Main GUI control
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Keeps track of socket
        /// </summary>
        public Socket _socket;
        /// <summary>
        /// Keeps track of game's "world"
        /// </summary>
        public TestClientView.World world;
        /// <summary>
        /// Keeps track of elapsed time
        /// </summary>
        private Stopwatch timer = new Stopwatch();
        /// <summary>
        /// To keep track of unique ID of player's cube
        /// </summary>
        private long uid;
        /// <summary>
        /// Keeps track of amount of food eaten 
        /// </summary>
        private int foodEaten;
        /// <summary>
        /// Keeps track of amount of players eaten
        /// </summary>
        private int playersEaten;
        /// <summary>
        /// Keeps track of frames
        /// </summary>
        int frames = 0;

        /// <summary>
        /// Main set-up of AgCubio view
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            world = new AgCubio.World();
        }

        /// <summary>
        /// Sends          
        /// </summary>
        private void send_the_name(AgCubio.Preserved_State state)
        {
            // Display error message if error occurs
            if (state.errorHappened)
            {
                ErrorLabel.Invoke(new MethodInvoker(delegate { ErrorLabel.Text = state.errorMessage; }));
            }
            else
            {
                // Hide the initial screen
                InitialPanel.Invoke(new MethodInvoker(delegate { this.InitialPanel.Hide(); }));
                ErrorLabel.Invoke(new MethodInvoker(delegate { this.ErrorLabel.Hide(); }));

                // Set callback function to receive_player         
                state.callbackFunction = new Action<AgCubio.Preserved_State>(this.receive_player);

                // Send the player name to the server
                AgCubio.Network.Send(this._socket, this.PlayerTextbox.Text + "\n");
            }
        }

        /// <summary>
        /// This method handles the initial player data, then changes GUI callback to more_data_available, which will ask for more data.
        /// </summary>
        private void receive_player(AgCubio.Preserved_State state)
        {
            StringBuilder sb = state.sb;
            char[] separator = new char[] { '\n' };
            string str = sb.ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];
            sb.Remove(0, str.Length);

            // Create a cube object from the initial player data
            AgCubio.Cube cube = JsonConvert.DeserializeObject<AgCubio.Cube>(str);

            // Add player cube to the world
            lock (this.world)
            {
                this.world.players[cube.uid] = cube;
            }
            this.uid = cube.uid;

            // Change GUI callback to more_data_availabe
            state.callbackFunction = new Action<AgCubio.Preserved_State>(this.more_data_available);
            this.more_data_available(state);
        }

        /// <summary>
        /// Handles extra data
        /// </summary>
        private void more_data_available(AgCubio.Preserved_State state)
        {
            StringBuilder sb = state.sb;
            lock (this.world)
            {
                char[] separator = new char[] { '\n' };
                string[] dataArray = sb.ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                foreach (string data in dataArray)
                {
                    if (data.StartsWith("{") && data.EndsWith("}"))
                    {
                        // Create cube objects from data
                        AgCubio.Cube cube = JsonConvert.DeserializeObject<AgCubio.Cube>(data);
                        // Deal with food cubes
                        if (cube.food)
                        {
                            // Remove food from game if mass is zero
                            if (cube.Mass == 0.0)
                            {
                                this.world.food.Remove(cube.uid);
                                foodEaten++;
                            }
                            // Or add the cube to the world
                            else
                            {
                                this.world.food[cube.uid] = cube;
                            }
                        }
                        // Deal with player cubes
                        else if (cube.Mass == 0.0)
                        {
                            // Remove cube from game if mass is zero
                            this.world.players.Remove(cube.uid);

                            // Stop game if player is dead 
                            if (cube.uid == this.uid)
                            {
                                timer.Stop();
                            }
                            else
                            {
                                playersEaten++;
                            }
                        }
                        // Else add cube to player dictionary in world
                        else
                        {
                            this.world.players[cube.uid] = cube;
                        }
                    }
                }
                // Clear the string builder
                sb.Clear();

                // Redraw the base
                base.Invalidate();
            }
            if (!timer.IsRunning)
            {
                // Disconnect gracefully
                this._socket.Disconnect(false);

                // Allow user to restart game
                this.Invoke(new MethodInvoker(delegate { this.RestartPanel.Show(); }));

                // Display statistics about game
                this.Invoke(new MethodInvoker(delegate { this.EndStatsPanel.Show(); }));
            }
            else
            {
                // Call for more data
                AgCubio.Network.i_want_more_data(state);
            }
        }

        /// <summary>
        /// Draws the scene
        /// </summary>
        private void PaintMe(object sender, PaintEventArgs e)
        {
            if (timer.IsRunning)
            {
                Focus();
            }
            else
            {
                this.world = new AgCubio.World();
            }


            // Send a request to server asking to move according to mouse location
            send_move_request();

            // Redraw invalidated regions
            base.Update();

            // Set frames per second 
            if (timer.IsRunning)
            {
                int ms = (int)timer.ElapsedMilliseconds;
                DynamicFPSLabel.Text = "" + (int)frames / (ms / 1000f);
                DynamicFPSLabel.Invalidate();
                DynamicFPSLabel.Update();
                DynamicFPSLabel.Refresh();
            }

            // Set the viewport
            try
            {
                float scale = (float)(world.players[uid].Mass * .001);
                e.Graphics.ScaleTransform(scale, scale);
                e.Graphics.TranslateTransform(-world.players[uid].loc_x, -world.players[uid].loc_y);
                e.Graphics.TranslateTransform(+this.Size.Width / 2 / scale, +this.Size.Height / 2 / scale);
            }
            catch
            {; }

            // Draw the cubes
            lock (this.world)
            {
                // Draw player cubes
                foreach (AgCubio.Cube cube in world.players.Values)
                {
                    // Draws the actual cube
                    SolidBrush brush = new SolidBrush(Color.FromArgb(cube.argb_color));
                    RectangleF rectangle = new RectangleF(cube.loc_x - cube.Center, cube.loc_y - cube.Center, cube.Width, cube.Width);
                    e.Graphics.FillRectangle(brush, rectangle);

                    // Deals with name on cube
                    Font font = new Font("Arial", 16F);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(cube.Name, font, Brushes.Yellow, rectangle, stringFormat);
                }
                // Draw food cubes
                foreach (AgCubio.Cube food in world.food.Values)
                {
                    SolidBrush brush = new SolidBrush(Color.FromArgb(food.argb_color));
                    e.Graphics.FillRectangle(brush, food.loc_x, food.loc_y, 5, 5);
                }
            }

            try
            {
                // Change mass and width labels according to object
                int mass = (int)(world.players[this.uid].Mass);
                this.DynamicMassLabel.Text = mass.ToString();
                this.DynamicWidthLabel.Text = world.players[this.uid].Width.ToString();
                this.DynamicMassLabel.Invalidate();
                this.DynamicMassLabel.Update();

                // Change the stats labels
                this.DynamicFoodEaten.Text = foodEaten.ToString(); // Food eaten

                this.DynamicPlayersEaten.Text = playersEaten.ToString(); // Players eaten

                int minutes = (int)timer.Elapsed.Minutes;
                int totalSeconds = (int)timer.Elapsed.Seconds;
                int seconds = totalSeconds % 60;
                string time = minutes + ":" + seconds;
                this.DynamicTimeAlive.Text = time; // Time alive
            }
            catch (KeyNotFoundException)
            {
                ;
            }

            // Invalidate and redraw control
            frames++;
            Invalidate();
        }

        /// <summary>
        /// Handles when user presses enter in the player textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            // Performs action if player presses enter
            if (e.KeyCode == Keys.Enter)
            {
                // Removes the noises when keys are pressed
                e.SuppressKeyPress = true;

                // Call CheckInputServer method
                CheckInputServer();
            }
        }

        /// <summary>
        /// Sends a message to the server which allows user to "split" their player cube
        /// </summary>
        private void DealWithSpacebar(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                int dest_x = base.PointToClient(Control.MousePosition).X;
                int dest_y = base.PointToClient(Control.MousePosition).Y;
                AgCubio.Network.Send(this._socket, "(split, " + dest_x.ToString() + ", " + dest_y.ToString() + ")\n");
            }
        }

        /// <summary>
        /// Sends a request to the server asking to move in a given direction
        /// </summary>
        private void send_move_request()
        {
            AgCubio.Cube cube;
            if (this.world.players.TryGetValue(this.uid, out cube))
            {
                int dest_x = base.PointToClient(Control.MousePosition).X;
                int dest_y = base.PointToClient(Control.MousePosition).Y;
                AgCubio.Network.Send(this._socket, "(move, " + dest_x.ToString() + ", " + dest_y.ToString() + ")\n");
            }
        }

        /// <summary>
        /// Helper method that is called whenever the enter button is clicked in the player or server textboxes, or when
        /// user presses the start button
        /// </summary>
        private void CheckInputServer()
        {
            // Deal with error label
            ErrorLabel.Text = "Connecting...";
            ErrorLabel.Show();

            // Call Connect_to_Server method
            this._socket = AgCubio.Network.Connect_to_Server(new Action<AgCubio.Preserved_State>(this.send_the_name), this.ServerTextbox.Text);

            // Start the timer
            timer.Restart();

            // Change the KeyEventHandler to deal with the spacebar
            this.KeyDown += new KeyEventHandler(this.DealWithSpacebar);
        }

        /// <summary>
        /// Handles when user presses button to play game again
        /// </summary>
        private void playAgainButton_Click(object sender, EventArgs e)
        {
            InitialPanel.Show();
            base.Invalidate();
            EndStatsPanel.Hide();
            RestartPanel.Hide();
        }
    }
}
