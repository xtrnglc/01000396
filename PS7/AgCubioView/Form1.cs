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

namespace AgCubioView
{
    public partial class Form1 : Form
    {
        private World world = new World();
        private State currentState = new State();
        private bool firstConnection = true;
        private string firstCube;
        private string playerName;
        private int FoodCount = 0;
        private bool Connected = false;
        private int MouseX;
        private int MouseY;
        int counter = 0;
        private bool gameStarted, playerDrawn = false;
        private bool moveSent = false;
        //string[] substrings = new string[1000000];

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ServerTextBox.Text = "localhost";
            this.PlayerNameTextBox.Text = "Adam";
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
        }

        /// <summary>
        /// Second callback to receive data containing player information 
        /// Draws player cube and most likely first food cube
        /// </summary>
        /// <param name="state"></param>
        private void SecondCallBack(State state)
        {
            currentState = state;
            firstCube = state.sb.ToString();
            string [] substrings = Regex.Split(firstCube, "\n");
            playerDrawn = true;
            int count = substrings.Count() - 1;
            string partialCube = substrings.Last();
            substrings[count] = null;
            AddCubes(substrings);
            //DrawCubes();
            currentState.sb.Clear();
            currentState.sb.Append(partialCube);
            Network.i_want_more_data(currentState);          
        }

        /// <summary>
        /// Adds cubes to the world
        /// </summary>
        /// <param name="JSON"></param>
        public void AddCubes(string [] JSON)
        {
            lock (world)
            foreach (string entry in JSON)
            {
                if (entry != null)
                {
                    Cube cube = JsonConvert.DeserializeObject<Cube>(entry);

                    //lock (world)
                    //{
                    //    for (int x = 0; x < world.WorldPopulation.Count; x++)
                    //    {
                    //        if (world.WorldPopulation.ElementAt(x).GetName() == playerName)
                    //        {
                    //            Cube c = world.WorldPopulation.ElementAt(x);
                    //            world.WorldPopulation.Remove(c);
                    //            x = world.WorldPopulation.Count + 1;
                    //        }
                    //    }   
                    //}
                        world.Add(cube);
                    }
            }
        }

        /// <summary>
        /// Draw all the cubes that are part of the world
        /// </summary>
        private void DrawCubes()
        {
            lock (world)
            {
                foreach (Cube cube in world.WorldPopulation)
                {
                    DrawCube(cube);
                    if (cube.GetFood())
                    {
                        FoodCount += 1;
                    }
                }
            }
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="cube"></param>
        private void DrawCube(Cube cube)
        {
            int cubeColor = (int)cube.GetColor();
            cubeColor = Math.Abs(cubeColor);
            Random rnd = new Random();
            //Color color = Color.FromArgb(255, Math.Abs((cubeColor * rnd.Next()) % 255), Math.Abs((cubeColor + rnd.Next()) % 255), (Math.Abs(cubeColor - rnd.Next())) % 255);
            Color color = Color.FromArgb(255, (cubeColor + 100) % 255, (cubeColor / 2) % 255, Math.Abs(cubeColor - 50) % 255);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            
            if (cube.Food == false)
            {
                formGraphics.FillRectangle(myBrush, new Rectangle((int)cube.loc_x, (int)cube.loc_y, cube.GetWidth(), cube.GetWidth()));
                System.Drawing.Graphics formGraphics2 = this.CreateGraphics();
                string drawString = cube.GetName();
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", cube.GetWidth() / 4);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                float x = (float)cube.GetX();
                float y = (float)cube.GetY() + (cube.GetWidth() / 3);
                System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                formGraphics2.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat); 
            }
            else
            {
                formGraphics.FillRectangle(myBrush, new Rectangle((int)cube.loc_x, (int)cube.loc_y, cube.GetWidth() + 2, cube.GetWidth() + 2));
            }
            
        }


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 32))
            {
                Network.Send(currentState.workSocket, "(split, " + ((int)MouseX).ToString() + ", " + ((int)MouseY).ToString() + ")\n");
            }
        }


        protected void OnMouseMove(object sender, MouseEventArgs e)
        {
            MouseX = e.X;
            MouseY = e.Y;
            int i = 0;
            if (playerDrawn && gameStarted)
            {
                //lock (world)
                //{
                //    for (int x = 0; x < world.WorldPopulation.Count; x++)
                //    {
                //        if (world.WorldPopulation.ElementAt(x).GetName() == playerName)
                //        {
                //            Cube c = world.WorldPopulation.ElementAt(x);
                //            world.WorldPopulation.Remove(c);
                //        }
                //    }
                //}
               
                Network.Send(currentState.workSocket, "(move, " + MouseX.ToString() + ", " + MouseY.ToString() + ")\n"); 
                moveSent = true;
                //DrawCubes();
            }
        }

        protected void OnPaint(object sender, PaintEventArgs e)
        {   
            if (Connected)
            {
                lock (world)
                {
                    foreach (Cube cube in world.WorldPopulation)
                    {
                        int cubeColor = (int)cube.GetColor();
                        cubeColor = Math.Abs(cubeColor);
                        Random rnd = new Random();
                        Color color = Color.FromArgb(255, (cubeColor + 100) % 255, (cubeColor / 2) % 255, Math.Abs(cubeColor - 50) % 255);
                        System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
                        System.Drawing.Graphics formGraphics;
                        formGraphics = this.CreateGraphics();

                        if (cube.Food == false)
                        {
                            formGraphics.FillRectangle(myBrush, new Rectangle((int)cube.loc_x, (int)cube.loc_y, cube.GetWidth(), cube.GetWidth()));
                            System.Drawing.Graphics formGraphics2 = this.CreateGraphics();
                            string drawString = cube.GetName();
                            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", cube.GetWidth() / 4);
                            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                            float x = (float)cube.GetX();
                            float y = (float)cube.GetY() + (cube.GetWidth() / 3);
                            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                            formGraphics2.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                        }
                        else
                        {
                            formGraphics.FillRectangle(myBrush, new Rectangle((int)cube.loc_x, (int)cube.loc_y, cube.GetWidth() + 2, cube.GetWidth() + 2));
                        }
                    }
                }
                //Invalidate();
            }
        }
    }
}

