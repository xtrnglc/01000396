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
using SpreadsheetUtilities;

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

            this.Cell_name_text.Text = GetCellName(col, row);

            if (value == "")
            {
                //ss.SetValue(col, row, this.Cell_Contents_text.Text);
                ss.GetValue(col, row, out value);
                this.Cell_Value_text.Text = value;
                
                this.Cell_Contents_text.Text = "";
                this.textBox3.Text = "";
            }
            else
            {
                if (!(Sheet.GetCellContents(GetCellName(col, row)) is string) && !(Sheet.GetCellContents(GetCellName(col, row)) is double))
                {
                    string temp = "=";
                    temp += Sheet.GetCellContents(GetCellName(col, row)).ToString();
                    this.Cell_Contents_text.Text = temp;
                    this.Cell_Value_text.Text = Sheet.GetCellValue(GetCellName(col, row)).ToString();
                    DisplayContentType(value);
                    this.textBox3.Text = "Formula";
                }

                else
                {
                    this.Cell_Contents_text.Text = Sheet.GetCellContents(GetCellName(col, row)).ToString();
                    this.Cell_Value_text.Text = Sheet.GetCellValue(GetCellName(col, row)).ToString();
                    DisplayContentType(value);
                }
                
            }

        }
        /// <summary>
        /// Returns name of selected cell
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetCellName(int col, int row)
        {
            row += 1;
            col += 65;
            char c = (char)col;
            string s = "";
            s += c;
            s += row.ToString();
            return s;
        }

        /// <summary>
        /// Returns the coordinates of a given cell
        /// </summary>
        /// <param name="s"></param>
        /// <returns>int col, int row</returns>
        private int[] GetCellCoordinates(string s)
        {
            char c = s[0];
            int col = (int)c;
            col -= 65;
            int row;
            

            string temp = s.Substring(1);
            int.TryParse(temp, out row);
            row -= 1;

            int[] coordinates = new int[2];
            coordinates[0] = col;
            coordinates[1] = row;

            return coordinates;
        }

        /// <summary>
        /// Displays content type depending on its type
        /// </summary>
        /// <param name="value"></param>
        private void DisplayContentType (string value)
        {
            double temp;

            if (value.StartsWith("="))
            {
                this.textBox3.Text = "Formula";
            }
            else if (double.TryParse(value, out temp))
            {
                this.textBox3.Text = "Double";
            }
            else
            {
                this.textBox3.Text = "String";
            }
                
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Help message regarding changing cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Change your selected cell by left clicking on a new box.");
        }

        /// <summary>
        /// Help message regarding changing cell contents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeCellContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To change a cell's contents, select the desired cell and edit the contents box and then press enter.");
        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Cell_name_text_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Event handler for enter key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_Contents_text_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                
                int row, col;
                String value = (sender as TextBox).Text;
                spreadsheetPanel1.GetSelection(out col, out row);
                ISet<String> cellsToRecalculate = new HashSet<String>();
                int[] coordinates = new int[2];

                this.Cell_name_text.Text = GetCellName(col, row);      
                
                try
                {
                    cellsToRecalculate = Sheet.SetContentsOfCell(GetCellName(col, row), (sender as TextBox).Text);
                    if ((sender as TextBox).Text.StartsWith("="))
                    {
                        spreadsheetPanel1.SetValue(col, row, Sheet.GetCellValue(GetCellName(col, row)).ToString());
                    }
                    else
                    {
                        spreadsheetPanel1.SetValue(col, row, this.Cell_Contents_text.Text);
                    }

                    foreach (string entry in cellsToRecalculate)
                    {
                        coordinates = GetCellCoordinates(entry);

                        spreadsheetPanel1.SetValue(coordinates.ElementAt(0), coordinates.ElementAt(1), Sheet.GetCellValue(entry).ToString());
                    }
                    this.Cell_Value_text.Text = Sheet.GetCellValue(GetCellName(col, row)).ToString();

                }
                catch (Exception excep)
                {
                    System.Windows.Forms.MessageBox.Show(excep.Message);
                }
                DisplayContentType(value);
            }
        }
    }
}
