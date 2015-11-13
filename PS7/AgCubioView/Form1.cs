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
        string[] substrings = new string[1000000];

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.ServerTextBox.Text = "localhost";
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
        /// CallBack function for the initial connect to server
        /// Initiially it will send the player name to the server and then prints a message box if connection is succesful
        /// </summary>
        /// <param name="state"></param>
        private void FirstCallBack(State state)
        {
            currentState = state;
            state.connectionCallback = SecondCallBack;
            Network.Send(state.workSocket, this.PlayerNameTextBox.Text + "\n");
            
            MessageBox.Show("Connected");
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
            substrings = Regex.Split(firstCube, "\n");
            Cube playerCube = JsonConvert.DeserializeObject<Cube>(substrings[0]);
            //DrawCube(playerCube);
            string temp = substrings[2];
            substrings[2] = null;
            AddCubes(substrings);
            DrawCubes();
            currentState.sb.Clear();
            currentState.sb.Append(temp);
            currentState.connectionCallback = ThirdCallBack;
            Network.i_want_more_data(currentState);
        }

        /// <summary>
        /// Constantly receives state from server
        /// Adds cubes to world population and draws cubes
        /// </summary>
        /// <param name="state"></param>
        private void ThirdCallBack(State state)
        {
            firstCube = state.sb.ToString();
            substrings = Regex.Split(firstCube, "\n");
            string temp = substrings[2];
            substrings[2] = null;
            AddCubes(substrings);
            DrawCubes();
            currentState.sb.Clear();
            currentState.sb.Append(temp);
            currentState.connectionCallback = ThirdCallBack;
            Network.i_want_more_data(currentState);
        }

        public void AddCubes(string [] JSON)
        {
            foreach (string entry in JSON)
            {
                if (entry != null)
                {
                    Cube cube = JsonConvert.DeserializeObject<Cube>(entry);
                    world.Add(cube);
                }
            }
        }

        private void DrawCubes()
        {
            {
                foreach (Cube cube in world.WorldPopulation)
                {
                    DrawCube(cube);
                }
            }
        }

        private void DrawCube(Cube cube)
        {
            if (cube.Food == false)
            {
                int cubeColor = (int)cube.GetColor();
                cubeColor = Math.Abs(cubeColor);
                Random rnd = new Random();
                Color color = Color.FromArgb(255, Math.Abs((cubeColor * rnd.Next()) % 255), Math.Abs((cubeColor + rnd.Next()) % 255), (Math.Abs(cubeColor - rnd.Next())) % 255);
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
                System.Drawing.Graphics formGraphics;
                formGraphics = this.CreateGraphics();
                formGraphics.FillRectangle(myBrush, new Rectangle((int)cube.loc_x / 2, (int)cube.loc_y / 2, (int)Math.Sqrt(cube.Mass), (int)Math.Sqrt(cube.Mass)));
                myBrush.Dispose();
                formGraphics.Dispose();
            }
            else
            {
                int cubeColor = (int)cube.GetColor();
                cubeColor = Math.Abs(cubeColor);
                Random rnd = new Random();
                Color color = Color.FromArgb(255, Math.Abs((cubeColor * rnd.Next()) % 255), Math.Abs((cubeColor + rnd.Next()) % 255), (Math.Abs(cubeColor - rnd.Next())) % 255);
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
                System.Drawing.Graphics formGraphics;
                formGraphics = this.CreateGraphics();
                formGraphics.FillRectangle(myBrush, new Rectangle((int)cube.loc_x / 2, (int)cube.loc_y / 2, (int)Math.Sqrt(cube.Mass)+5, (int)Math.Sqrt(cube.Mass)+5));
                myBrush.Dispose();
                formGraphics.Dispose();
            }
        }
    }
}

/*
 public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.ServerText.Text = "localhost";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "AgCubio";
        }

        

        private void PlayerNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && PlayerNameText.Text != "")
            {
                //check to see if the server connects with the given hostname
                this.PlayerNameLabel.Visible = false;
                this.PlayerNameTextBox.Visible = false;
                this.ServerLabel.Visible = false;
                this.ServerTextBox.Visible = false;

                callback();
            }
        }

        /// <summary>
        /// Method needs to attempt to connect to the server with the given hostname
        /// </summary>
        private void callback()
        {
            

            string hostname = this.ServerText.Text;


        }

        private void GetMoreData()
        {
            //calls the i_want_more_data method in the network
        }

        private void Draw()
        {
            Cube cube = new Cube(30, 40, -79840260, 57, true, "test", 1000);
            int cubeColor = cube.GetColor();
            cubeColor = Math.Abs(cubeColor);
            Random rnd = new Random();
            Color color = Color.FromArgb(cubeColor % 255, cubeColor % 255, cubeColor % 255);
            
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(cube.GetX(), cube.GetY(), (int) Math.Sqrt(cube.GetMass()), (int)Math.Sqrt(cube.GetMass())));
            myBrush.Dispose();
            formGraphics.Dispose();
            int colormain, color1, color2, color3, color4;

            for (int i = 0; i < 100; i++)
            {
                cube = new Cube(rnd.Next(1, 1000), rnd.Next(1, 1000), rnd.Next(1, 1000000), 57, true, "test", 100);
                cubeColor = cube.GetColor();
                cubeColor = Math.Abs(cubeColor);
                formGraphics = this.CreateGraphics();
                colormain = cubeColor % 255;
                color1 = colormain + 50;
                color2 = (colormain - 50) * 2;
                color3 = colormain / 2 + 50;
                color4 = colormain * 2;
                if (color1 > 255)
                    color1 = 255;
                if (color2 < 0 || color2 > 255)
                    color2 = 125;
                if (color4 > 255)
                    color4 = 100;
                color = Color.FromArgb(255, color2, color3, color4);
                myBrush = new System.Drawing.SolidBrush(color);
                formGraphics.FillRectangle(myBrush, new Rectangle(cube.GetX(), cube.GetY(), (int)Math.Sqrt(cube.GetMass()), (int)Math.Sqrt(cube.GetMass())));
            }

        }

        private void You_MouseDown(object sender, MouseEventArgs e)
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X - 50, Cursor.Position.Y - 50);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
        }

        private void Form1_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void MoveCursor()
        {
            // Set the Current cursor, move the cursor's Position,
            // and set its clipping rectangle to the form. 

            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X - 50, Cursor.Position.Y - 50);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
        }


    }
}



     private void Draw()
        {
            
            Cube cube = new Cube(30, 40, -79840260, 57, true, "test", 1000);
            int cubeColor = cube.GetColor();
            cubeColor = Math.Abs(cubeColor);
            Random rnd = new Random();
            Color color = Color.FromArgb(cubeColor % 255, cubeColor % 255, cubeColor % 255);


            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            myBrush.Dispose();
            formGraphics.Dispose();
            int colormain, color1, color2, color3, color4;

            for (int i = 0; i < 100; i++)
            {
                cube = new Cube(rnd.Next(1, 1000), rnd.Next(1, 1000), rnd.Next(1, 1000000), 57, true, "test", 100);
                cubeColor = cube.GetColor();
                cubeColor = Math.Abs(cubeColor);
                formGraphics = this.CreateGraphics();
                colormain = cubeColor % 255;
                color1 = colormain + 50;
                color2 = (colormain - 50) * 2;
                color3 = colormain / 2 + 50;
                color4 = colormain * 2;
                if (color1 > 255)
                    color1 = 255;
                if (color2 < 0 || color2 > 255)
                    color2 = 125;
                if (color4 > 255)
                    color4 = 100;
                color = Color.FromArgb(255, color2, color3, color4);
                myBrush = new System.Drawing.SolidBrush(color);
                formGraphics.FillRectangle(myBrush, new Rectangle(cube.GetX(), cube.GetY(), (int)Math.Sqrt(cube.GetMass()), (int)Math.Sqrt(cube.GetMass())));
            }
        }

*/
