﻿//CS 3500 PS7
//Adam Sorensen and Trung Le
//This is the view class for the AgCubio game, holds the GUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using AgCubio;

namespace View
{
    public partial class Form1 : Form
    {
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
                //check to see if the server text also works
                this.PlayerName.Visible = false;
                this.PlayerNameText.Visible = false;
                this.Server.Visible = false;
                this.ServerText.Visible = false;

                Draw();
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
            formGraphics.FillRectangle(myBrush, new Rectangle(cube.GetX(), cube.GetY(), (int) Math.Sqrt(cube.GetMass()), (int)Math.Sqrt(cube.GetMass())));
            myBrush.Dispose();
            formGraphics.Dispose();
            int colormain, color1, color2, color3, color4;

            for (int i = 0; i < 100; i++)
            {
                cube = new Cube(rnd.Next(1, 1000), rnd.Next(1, 1000), rnd.Next(1, 1000000), 57, true, "test", 1000);
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

            /*
            {
                Color color = Color.FromArgb(count, count, count);
                myBrush = new System.Drawing.SolidBrush(color);

                count++;

                if (count > 255) count = 0;

                e.Graphics.FillRectangle(myBrush, new Rectangle(count, count, 10 + count, 10 + 2 * count));
                Console.WriteLine("repainting " + count);
            }
            */
        }

        public class Form2 : Form
        {
            public Form2()
            {
                Text = "Agcubio";
            }
        }
    }
}
