using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //spreadsheetPanel1.SelectionChanged += displaySelection;
            //spreadsheetPanel1.SetSelection(2, 3);
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void changeSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Change your selected cell by left clicking on a new box.");
        }

        private void changeCellContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To change a cell's contents, select the desired cell and edit the contents box.");
        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
