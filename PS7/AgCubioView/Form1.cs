using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgCubioView
{
    public partial class Form1 : Form
    {
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

        private void PlayerNameTextBox_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && PlayerNameTextBox.Text != "")
            {
            }
        }

        public void callBack()
        {
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            this.PlayerNameLabel.Visible = false;
            this.PlayerNameTextBox.Visible = false;
            this.ServerLabel.Visible = false;
            this.ServerTextBox.Visible = false;
            this.ConnectButton.Visible = false;

            try
            {

            }
            catch(Exception excep)
            {

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

*/
