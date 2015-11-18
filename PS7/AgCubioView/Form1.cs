/*
Author: Trung Le and Adam Sorensen
11/11/2015
CS 3500
PS7 - AgCubio
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgCubio;
using NetworkController;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AgCubioView
{
    public partial class Form1 : Form
    {
        private World world = new World();
        private State currentState = new State();
        private string firstCube;
        private string playerName;
        private Stopwatch timer = new Stopwatch();
        private bool Connected = false;
        private bool playerAlive;
        private int MouseX;
        private int MouseY;
        private int foodEaten = 0;
        private bool gameStarted, playerDrawn = false;
        private int playersEaten = 0;
        private int playerMass;
        

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ServerTextBox.Text = "localhost";
            this.PlayerNameTextBox.Text = "Adam";
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "AgCubio";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void PlayerNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Connected)
            {
                Network.Send(currentState.workSocket, string.Empty);
            }
        }

        private void Form1_Closed(object sender, System.EventArgs e)
        {
            MessageBox.Show("hey");
        }

        /// <summary>
        /// Initial connect to server
        /// </summary>
        private void ConnectMethod()
        {
            this.PlayerNameLabel.Visible = false;
            this.PlayerNameTextBox.Visible = false;
            this.ServerLabel.Visible = false;
            this.ServerTextBox.Visible = false;
            this.ConnectButton.Visible = false;

            try
            {
                string name = this.PlayerNameTextBox.Text;
                string server = this.ServerTextBox.Text;

                Socket socket;

                socket = Network.Connect_to_Server(FirstCallBack, server);
            }

            catch (Exception excep)
            {
                MessageBox.Show("Failed to connect");
                this.PlayerNameLabel.Visible = true;
                this.PlayerNameTextBox.Visible = true;
                this.ServerLabel.Visible = true;
                this.ServerTextBox.Visible = true;
                this.ConnectButton.Visible = true;
            }
        }

        /// <summary>
        /// Connect handler for enter key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerNameTextBox_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && PlayerNameTextBox.Text != "")
            {
                ConnectMethod();
            }
        }

        /// <summary>
        /// Connect handler for connect button press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectMethod();
        }

        /// <summary>
        /// CallBack function for the initial connect to server
        /// Initiially it will send the player name to the server and then prints a message box if connection is succesful
        /// </summary>
        /// <param name="state"></param>
        private void FirstCallBack(State state)
        {
            currentState = state;
            state.connectionCallback = SecondCallBack;
            Network.Send(state.workSocket, this.PlayerNameTextBox.Text + "\n");
            playerName = PlayerNameTextBox.Text;
            Connected = true;
            gameStarted = true;
            playerAlive = true;
            timer.Start();
        }

        /// <summary>
        /// Second callback to receive data containing player information 
        /// Draws player cube and most likely first food cube
        /// </summary>
        /// <param name="state"></param>
        private void SecondCallBack(State state)
        {
            currentState = state;
            firstCube = currentState.sb.ToString();
            string [] substrings = Regex.Split(firstCube, "\n");
            playerDrawn = true;
            int count = substrings.Count() - 1;
            string partialCube = substrings.Last();
            substrings[count] = null;
            currentState.connectionCallback = ThirdCallBack;
            currentState.sb.Clear();
            currentState.sb.Append(partialCube);
            Network.i_want_more_data(currentState);          
        }

        private void ThirdCallBack(State state)
        {
            currentState = state;
            string dataString = state.sb.ToString();
            //Console.WriteLine(dataString);
            string[] substrings = Regex.Split(dataString, "\n");
            int count = substrings.Count() - 1;
            string partialCube = substrings.Last();
            substrings[count] = null;
            currentState.connectionCallback = ThirdCallBack;
            currentState.sb.Clear();
            currentState.sb.Append(partialCube);

            if (playerDrawn && gameStarted)
            {
                Network.Send(currentState.workSocket, "(move, " + MouseX.ToString() + ", " + MouseY.ToString() + ")\n");
            }

            lock (world)
            {
                foreach (string entry in substrings)
                {
                    if (entry != null)
                    {
                        Cube cube = JsonConvert.DeserializeObject<Cube>(entry);
                        {

                            if (cube.GetFood() == false && world.ListOfPlayers.ContainsKey(cube.GetID))
                            {
                                if (cube.Mass == 0)
                                {
                                    world.ListOfPlayers.Remove(cube.GetID);
                                    playersEaten++;
                                }
                                else
                                {
                                    world.ListOfPlayers.Remove(cube.GetID);
                                    world.ListOfPlayers[cube.GetID] = cube;
                                }

                            }
                            else if (cube.GetFood() == true && world.ListOfFood.ContainsKey(cube.GetID))
                            {
                                if (cube.Mass == 0)
                                {
                                    world.ListOfFood.Remove(cube.GetID);
                                    foodEaten++;
                                }
                                else
                                {
                                    world.ListOfFood.Remove(cube.GetID);
                                    world.ListOfFood[cube.GetID] = cube;
                                }                                
                            }
                            else
                            {
                                world.Add(cube);
                            }
                        }
                    }
                }
            
            }
            Network.i_want_more_data(currentState);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //
              //      MessageBox.Show("space pressed");


            //{
            //    Network.Send(currentState.workSocket, "(split, " + (MouseX).ToString() + ", " + (MouseY).ToString() + ")\n");
            //}
        }


        protected void OnMouseMove(object sender, MouseEventArgs e)
        {
            MouseX = e.X;
            MouseY = e.Y;
        }

        private void foodtextbox_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                
            }
        }

        private void food_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        protected void OnPaint(object sender, PaintEventArgs e)
        {
            if (Connected)
            {
                lock (world)
                {
                    foreach (KeyValuePair<int, Cube> c in world.ListOfFood)
                    {
                        Cube cube = c.Value;
                        SolidBrush brush = new SolidBrush(Color.FromArgb((int)cube.argb_color));
                        e.Graphics.FillRectangle(brush, (float)cube.loc_x, (float)cube.loc_y, 2, 2);
                        base.Invalidate();
                    }

                    foreach (KeyValuePair<int, Cube> c in world.ListOfPlayers)
                    {
                        Cube cube = c.Value;
                        if (cube.GetName() == playerName)
                        {
                            playerMass = cube.GetMass();
                            if (playerMass == 0)
                            {
                                playerAlive = false;
                                timer.Stop();
                            }
                        }
                        SolidBrush brush = new SolidBrush(Color.FromArgb((int)cube.argb_color));
                        RectangleF rectangle = new RectangleF((float)cube.loc_x - cube.GetWidth() * 1.5f, (float)cube.loc_y - cube.GetWidth() * 1.5f, cube.GetWidth() * 3, cube.GetWidth() * 3);
                        e.Graphics.FillRectangle(brush, rectangle);

                        Font font = new Font("Arial", cube.GetWidth() / 4);
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        e.Graphics.DrawString(cube.Name, font, Brushes.Yellow, rectangle, stringFormat);
                    }

                    try
                    {
                        this.masstextright.Text = playerMass.ToString();
                        this.masstextright.Invalidate();
                        this.masstextright.Update();

                        this.foodtextbox.Text = foodEaten.ToString();
                        this.foodtextbox.Invalidate();
                        this.foodtextbox.Update();

                        this.playerseatentextbox.Text = playersEaten.ToString();
                        this.playerseatentextbox.Invalidate();
                        this.playerseatentextbox.Update();

                        int minutes = (int)timer.Elapsed.Minutes;
                        int totalSeconds = (int)timer.Elapsed.Seconds;
                        int seconds = totalSeconds % 60;
                        string time = minutes + ":" + seconds;

                        this.timealivetext.Text = time;
                        this.timealivetext.Invalidate();
                        this.timealivetext.Update();
                    }
                    catch(Exception excep)
                    {

                    }
                }
            }
        }
    }
}


