/*
Authors: Adam Sorensen, Trung Le
Team Name: Blue Steel
CS 3500
11/01/2015
Purpose: Spreadsheet GUI
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
using SS;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.IO;

namespace SpreadsheetGUI
{

    public partial class Form1 : Form
    {
        //private bool SelectionChange = false;
        private Spreadsheet Sheet = new Spreadsheet(isValid, t => t.ToUpper(), "ps6");
        string fileName = null;
        
      
        public Form1()
        {
            InitializeComponent();

            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);
            this.Cell_name_text.Text = "A1";
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
                string temp = "=";
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
                    DisplayContentType(value);
                    this.Cell_Contents_text.Text = temp += Sheet.GetCellContents(GetCellName(col, row)).ToString();
                }
                catch (Exception excep)
                {
                    System.Windows.Forms.MessageBox.Show(excep.Message);
                }
                
            }
        }

        /// <summary>
        /// Saves the current state of the file using an existing filename
        /// If an existing filename does not exist then one is created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {

            if (fileName == null || fileName == "")
            {
                try
                {
                    fileName = Prompt.ShowDialog("Enter File Name", "");
                }
                catch(Exception excep)
                {
                    System.Windows.Forms.MessageBox.Show(excep.Message);
                }

                if (fileName != "")
                {
                    if (!fileName.Contains(".sprd"))
                    {
                        fileName += ".sprd";
                    }
                    Sheet.Save(fileName);
                }
            }
            else
            {
                if (fileName != "")
                {
                    if (!fileName.Contains(".sprd"))
                    {
                        fileName += ".sprd";
                    }
                    Sheet.Save(fileName);
                }
            }
        }

        /// <summary>
        /// Saves the current state of the file with a new save name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsOption_Click(object sender, EventArgs e)
        {
            try
            {
                fileName = Prompt.ShowDialog("Enter File Name", "");
            }
            catch (Exception excep)
            {
                System.Windows.Forms.MessageBox.Show(excep.Message);
            }

            if (fileName != "")
            {
                if (!fileName.Contains(".sprd"))
                {
                    fileName += ".sprd";
                }
                Sheet.Save(fileName);
            }
            
            
        }

        /// <summary>
        /// Prompt box
        /// </summary>
        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form();
                prompt.Width = 300;
                prompt.Height = 150;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = caption;
                prompt.StartPosition = FormStartPosition.CenterScreen;
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
                Button confirmation = new Button() { Text = "Ok", Left = 100, Width = 50, Top = 75, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }

        public static class PromptForSelection
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form();
                prompt.Width = 300;
                prompt.Height = 150;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = caption;
                prompt.StartPosition = FormStartPosition.CenterScreen;
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
                Button confirmation = new Button() { Text = "Done", Left = 100, Width = 50, Top = 75, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }

        /// <summary>
        /// Open file handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, EventArgs e)
        {
            int[] coordinates = new int[2];
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            Stream myStream = null;
            FileStream fileStream;
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*|sprd files (*.sprd)|*.sprd";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        fileStream = myStream as FileStream;
                        using (fileStream)
                        {
                            Spreadsheet newSpreadsheet = new Spreadsheet(fileStream.Name.ToString(), isValid, s => s.ToUpper(), "ps6");

                            HashSet<String> data = new HashSet<string>(newSpreadsheet.GetNamesOfAllNonemptyCells());

                            Form1 form = new Form1();

                            form.Sheet = newSpreadsheet;

                            form.Text = fileStream.Name.ToString();
                            form.fileName = fileStream.Name.ToString().Substring(fileStream.Name.ToString().LastIndexOf("\\") + 1); ;
                            form.Show();
                            
                            foreach (string entry in data)
                            {
                                coordinates = GetCellCoordinates(entry);

                                form.spreadsheetPanel1.SetValue(coordinates[0], coordinates[1], newSpreadsheet.GetCellValue(entry).ToString());

                                form.Cell_Value_text.Text = Sheet.GetCellValue(entry).ToString();
                                form.Cell_Contents_text.Text = Sheet.GetCellContents(entry).ToString();
                            }

                            if (form.Sheet.GetCellValue("A1") != "")
                            {
                                form.Cell_name_text.Text = "A1";
                                form.Cell_Contents_text.Text = form.Sheet.GetCellContents("A1").ToString();
                                form.Cell_Value_text.Text = form.Sheet.GetCellValue("A1").ToString();
                                double temp;

                                if (form.Sheet.GetCellValue("A1") is double)
                                {
                                    form.textBox3.Text = "Double";
                                }
                                else if (form.Sheet.GetCellValue("A1") is string)
                                {
                                    form.textBox3.Text = "String";
                                }
                                else
                                {
                                    form.textBox3.Text = "Formula";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Close Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// New form handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();            
            form.Show();
        }

        
        /// <summary>
        /// Method to close the spreadsheet. If the user wants to save, it will go to the
        /// save method and ask for a file name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click_1(object sender, EventArgs e)
        {
            if (Sheet.Changed)
            {
                // Display a MsgBox asking the user to save changes or abort.
                if (MessageBox.Show("Do you want to save changes to your spreadsheet?", "Spreadsheet",
                   MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //Call method to save file
                    SaveAsOption_Click(sender, e);
                }
            }
            Close();
        }

        /// <summary>
        /// Variable validator
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool isValid(String name)
        {
            return Regex.IsMatch(name, "^[a-zA-Z][1-9][0-9]?$");
        }

        /// <summary>
        /// Help message regarding save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void savingASpreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To save a Spreadsheet, click on File then Save As. Then enter file name. If this process has already been done then simply click save.");
        }

        /// <summary>
        /// Help message regarding open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openingASpreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To open a Spreadsheet, click on File then Open. Then enter browse to where the file is saved and then click on it");
        }

        private void Cell_Contents_text_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Returns the sum of the cells selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            List<string> listOfCellsToAdd_Name = new List<string>();
            double sum = 0;

            cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");
            listOfCellsToAdd_Name.Add(cellSelection);

            while (MessageBox.Show("Do you want to add another cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");
                if (isValid(cellSelection))
                    listOfCellsToAdd_Name.Add(cellSelection);
                else
                {
                    MessageBox.Show("Please enter a valid cell name");
                }
            }      
            
            try
            {
                foreach (string cell in listOfCellsToAdd_Name)
                {
                    if (double.TryParse((Sheet.GetCellValue(cell).ToString()), out temp))
                    {
                        sum += temp;
                    }
                    else
                    {
                        throw new Exception("The cell " + cell + " cannot be parsed into a double"); 
                    }
                }

                if (MessageBox.Show("The sum of the cells is: " + sum + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + sum + "to", "");

                    try
                    {
                        int row, col;
                        string temp1 = "=";
                        ISet<String> cellsToRecalculate = new HashSet<String>();
                        int[] coordinates = new int[2];
                        coordinates = GetCellCoordinates(cellSelection);

                        cellsToRecalculate = Sheet.SetContentsOfCell(cellSelection, sum.ToString());
                        spreadsheetPanel1.SetValue(coordinates.ElementAt(0), coordinates.ElementAt(1), this.Cell_Contents_text.Text);


                        foreach (string entry in cellsToRecalculate)
                        {
                            coordinates = GetCellCoordinates(entry);

                            spreadsheetPanel1.SetValue(coordinates.ElementAt(0), coordinates.ElementAt(1), Sheet.GetCellValue(entry).ToString());
                        }
                        this.Cell_Value_text.Text = Sheet.GetCellValue(cellSelection).ToString();
                    }
                    catch (Exception excep)
                    {
                        System.Windows.Forms.MessageBox.Show(excep.Message);
                    }
                }
            }
            catch(Exception excep)
            {
                MessageBox.Show("There was a problem trying to sum the cells: " + excep.Message);
            }    
        }
    }
}
