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
        private bool SelectionChange = false;
        private Spreadsheet Sheet = new Spreadsheet();
      
        public Form1()
        {
            

            InitializeComponent();

            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);

            
        }

        /// <summary>
        /// Clicking on a cell
        /// </summary>
        /// <param name="ss"></param>
        private void displaySelection(SpreadsheetPanel ss)
        {
            int row, col;
            String value;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);

            //DisplayCellName(col, row).ToString()

            this.Cell_name_text.Text = DisplayCellName(col, row);

            
            if (value == "")
            {
                ss.SetValue(col, row, this.Cell_Contents_text.Text);
                ss.GetValue(col, row, out value);
                this.Cell_Value_text.Text = value;
                //MessageBox.Show("Selection: column " + col + " row " + row + " value " + value);
                this.Cell_Contents_text.Text = "";
            }
            else
            {
                this.Cell_Contents_text.Text = value;
                this.Cell_Value_text.Text = value;
            }

            Sheet.SetContentsOfCell(DisplayCellName(col, row), value);
        }

        private string DisplayCellName(int col, int row)
        {
            
            row += 1;
            col += 65;
            char c = (char)col;
            string s = "";
            s += c;
            s += row.ToString();
            return s;
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

        private void Cell_name_text_TextChanged(object sender, EventArgs e)
        {
            this.Cell_name_text.Text = "Hello";
        }

        private void Cell_Contents_text_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Cell_Contents_text_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int row, col;
                String value;
                spreadsheetPanel1.GetSelection(out col, out row);
                spreadsheetPanel1.GetValue(col, row, out value);

                //DisplayCellName(col, row).ToString()

                this.Cell_name_text.Text = DisplayCellName(col, row);

                
                
                if (value == "")
                {
                    spreadsheetPanel1.SetValue(col, row, this.Cell_Contents_text.Text);
                    spreadsheetPanel1.GetValue(col, row, out value);
                    this.Cell_Value_text.Text = value;
                    //MessageBox.Show("Selection: column " + col + " row " + row + " value " + value);
                    this.Cell_Contents_text.Text = "";
                }
                else
                {
                    spreadsheetPanel1.SetValue(col, row, this.Cell_Contents_text.Text);
                    this.Cell_Contents_text.Text = value;
                    this.Cell_Value_text.Text = value;
                }
                
            }

            
        }
    }
}
