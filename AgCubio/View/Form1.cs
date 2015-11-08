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

namespace View
{
    public partial class Form1 : Form
    {
        private System.Drawing.SolidBrush myBrush;
        int count = 0;
        
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
            Random rnd = new Random();
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(rnd.Next(1, 1000), rnd.Next(1, 1000), 10 + count, 10 + 2 * count));
            myBrush.Dispose();
            formGraphics.Dispose();

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
