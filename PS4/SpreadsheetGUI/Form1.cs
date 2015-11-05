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
        /// <summary>
        /// Makes a new spreadsheet to keep track of cell values, contents and dependencies
        /// </summary>
        private Spreadsheet Sheet = new Spreadsheet(isValid, t => t.ToUpper(), "ps6");
        /// <summary>
        /// Global string to keep track of file name
        /// </summary>
        string fileName = null;
        /// <summary>
        /// Used for background worker that we couldn't get working
        /// </summary>
        string CellName;
        /// <summary>
        /// Also used for background worker
        /// </summary>
        string Value;
        /// <summary>
        /// used for background worker. Keeps track of the cells that need to be recalculated
        /// </summary>
        ISet<String> CellsToRecalculate = new HashSet<String>();

        /// <summary>
        /// Makes the new spreadsheet GUI, iniziales the first selection to A1
        /// </summary>
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
            System.Windows.Forms.MessageBox.Show("To change a cell's contents, select the desired cell and edit the contents box and then press enter. Negative numbers are not accepted.");
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
                updateCells(GetCellName(col, row), (sender as TextBox).Text);              
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

        /// <summary>
        /// Generate a prompt box that asks the user for input and returns a string
        /// </summary>
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
                    updateCells(cellSelection, sum);
                }
            }
            catch(Exception excep)
            {
                MessageBox.Show("There was a problem trying to sum the cells: " + excep.Message);
            }    
        }

        /// <summary>
        /// Returns the average of selected cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void averageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            List<string> listOfCellsToAdd_Name = new List<string>();
            double sum = 0;
            double count = 0;
            double average = 0;

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
                        count++;
                    }
                    else
                    {
                        throw new Exception("The cell " + cell + " cannot be parsed into a double");
                    }
                }

                average = sum / count;

                if (MessageBox.Show("The average of the cells is: " + average + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + sum + "to", "");

                    updateCells(cellSelection, average);
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("There was a problem trying to average the cells: " + excep.Message);
            }
        }

        /// <summary>
        /// Returns the maximum selected cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            List<string> listOfCellsToAdd_Name = new List<string>();
            double max = 0;


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
                        if (temp > max)
                        {
                            max = temp;
                        }

                    }
                    else
                    {
                        throw new Exception("The cell " + cell + " cannot be parsed into a double");
                    }
                }

                if (MessageBox.Show("The maximum  of the cells is: " + max + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + max + "to", "");

                    updateCells(cellSelection, max);
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("There was a problem trying to find the maximum of the cells: " + excep.Message);
            }
        }

        /// <summary>
        /// Returns the minimum of selected cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            List<string> listOfCellsToAdd_Name = new List<string>();
            double min = double.PositiveInfinity;

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
                        if (temp < min)
                        {
                            min = temp;
                        }
                    }
                    else
                    {
                        throw new Exception("The cell " + cell + " cannot be parsed into a double");
                    }
                }

                if (MessageBox.Show("The maximum  of the cells is: " + min + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + min + "to", "");

                    updateCells(cellSelection, min);
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("There was a problem trying to find the maximum of the cells: " + excep.Message);
            }
        }

        /// <summary>
        /// Returns the square of a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            double result = 0;

            cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");

            while (!isValid(cellSelection))
            {
                MessageBox.Show("Please enter a valid cell name");
                cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");
            }

            try
            {
                if (double.TryParse(Sheet.GetCellValue(cellSelection).ToString(), out temp))
                {
                    result = temp * temp;
                }
            }
            catch (Exception excep)
            {
                throw new Exception("The cell " + cellSelection + " cannot be parsed into a double");
            }

            if (MessageBox.Show("The square  of the cell is: " + result + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + result + "to", "");

                updateCells(cellSelection, result);
            }
        }

        /// <summary>
        /// Returns the square root of a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void squareRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            double result = 0;

            cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");

            while (!isValid(cellSelection))
            {
                MessageBox.Show("Please enter a valid cell name");
                cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");
            }

            try
            {
                if (double.TryParse(Sheet.GetCellValue(cellSelection).ToString(), out temp))
                {
                    result = Math.Sqrt(temp);
                }
            }
            catch (Exception excep)
            {
                throw new Exception("The cell " + cellSelection + " cannot be parsed into a double");
            }

            if (MessageBox.Show("The square  of the cell is: " + result + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + result + "to", "");

                updateCells(cellSelection, result);
            }
        }

        /// <summary>
        /// Help message regarding finding the square
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findTheSquareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To find the square of a cell, click on Math then click on Square. Then enter the name of the cell. The square of the cell will then be computed and then you can assign that value to a cell.");
        }

        /// <summary>
        /// Help message regarding finding the sum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void summingCellsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To find the sum of a list of cells, click on Math then click on Sum. Then enter the name of the cells. Click on No when prompted to end the list. The sum of the cells will then be computed and then you can assign that value to a cell.");
        }

        /// <summary>
        /// Help message regarding finding the average
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findingTheAverageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To find the average of a list of cells, click on Math then click on Average. Then enter the name of the cells. Click on No when prompted to end the list. The average of the cells will then be computed and then you can assign that value to a cell.");
        }

        /// <summary>
        /// Help message regarding finding the maximum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findingTheMaximumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To find the maximum of a list of cells, click on Math then click on Maximum. Then enter the name of the cells. Click on No when prompted to end the list. The maximum of the cells will then be computed and then you can assign that value to a cell.");
        }

        /// <summary>
        /// Help message regarding finding the minimum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findingTheMinimumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To find the sum of a list of cells, click on Math then click on Minimum. Then enter the name of the cells. Click on No when prompted to end the list. The minimum of the cells will then be computed and then you can assign that value to a cell.");
        }

        /// <summary>
        /// Help message regarding finding the square root
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findingTheSquareRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To find the square root of a cell, click on Math then click on Square Root. Then enter the name of the cells. Click on No when prompted to end the list. The square root of the cell will then be computed and then you can assign that value to a cell.");
        }

        /// <summary>
        /// Find and replace all cells with a specified value to a new value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findAndReplaceToolTip_Click(object sender, EventArgs e)
        {
            string value;
            double doubleValue;
            double temp;
            string valueToReplace;
            double valueToReplaceDouble;
            HashSet<String> cellList = new HashSet<string>(Sheet.GetNamesOfAllNonemptyCells());

            value = PromptForSelection.ShowDialog("Find", "");
            valueToReplace = PromptForSelection.ShowDialog("Replace with", "");
                        
            try
            {
                if (double.TryParse(value, out doubleValue) && double.TryParse(valueToReplace, out valueToReplaceDouble))
                {
                    foreach (string cell in cellList)
                    {
                        if (double.TryParse(Sheet.GetCellValue(cell).ToString(), out temp))
                        {
                            if (temp == doubleValue)
                            {
                                updateCells(cell, valueToReplace);
                            }
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception excep)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid number");
            }
        }

        /// <summary>
        /// Help message for find and replace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void howToFindAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To find and replace, click on Edit then on Find and Replace. First enter the number/string to be replaced then enter the number/string to replace it with.");
        }

        /// <summary>
        /// Clears all entries in spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HashSet<String> cellList = new HashSet<string>(Sheet.GetNamesOfAllNonemptyCells());
            int[] coordinates = new int[2];
            foreach (string cell in cellList)
            {
                Sheet.SetContentsOfCell(cell, "");
                try
                {
                    coordinates = GetCellCoordinates(cell);
                    this.Cell_Value_text.Text = Sheet.GetCellValue(cell).ToString();
                    spreadsheetPanel1.SetValue(coordinates.ElementAt(0), coordinates.ElementAt(1), "");
                }
                catch (Exception excep)
                {
                    System.Windows.Forms.MessageBox.Show(excep.Message);
                }
            }
        }

        /// <summary>
        /// Help message for clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void howToClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("To Clear, click on Edit then click on Clear");
        }

        /// <summary>
        /// Helper method to update cells
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="value"></param>
        private void updateCells(string cellName, object value)
        {
            try
            {
                ISet<String> cellsToRecalculate = new HashSet<String>();
                int[] coordinates = new int[2];
                coordinates = GetCellCoordinates(cellName);

                cellsToRecalculate = Sheet.SetContentsOfCell(cellName, value.ToString());
                spreadsheetPanel1.SetValue(coordinates.ElementAt(0), coordinates.ElementAt(1), this.Cell_Contents_text.Text);


                foreach (string entry in cellsToRecalculate)
                {
                    coordinates = GetCellCoordinates(entry);

                    spreadsheetPanel1.SetValue(coordinates.ElementAt(0), coordinates.ElementAt(1), Sheet.GetCellValue(entry).ToString());
                }
                this.Cell_Value_text.Text = Sheet.GetCellValue(cellName).ToString();


                /*
                CellName = cellName;
                if (value.ToString().StartsWith("="))
                    Value = value.ToString().Substring(1);
                else
                {
                    Value = value.ToString();
                }

                int[] coordinates = new int[2];
                coordinates = GetCellCoordinates(cellName);

                backgroundWorker1.RunWorkerAsync();
                spreadsheetPanel1.SetValue(coordinates.ElementAt(0), coordinates.ElementAt(1), Sheet.GetCellValue(cellName).ToString());
                //spreadsheetPanel1.SetValue(coordinates.ElementAt(0), coordinates.ElementAt(1), this.Cell_Contents_text.Text);

                foreach (string entry in CellsToRecalculate)
                {
                    coordinates = GetCellCoordinates(entry);

                    spreadsheetPanel1.SetValue(coordinates.ElementAt(0), coordinates.ElementAt(1), Sheet.GetCellValue(entry).ToString());
                }
                this.Cell_Value_text.Text = Sheet.GetCellValue(CellName).ToString();
                */
            }
            catch (Exception excep)
            {
                System.Windows.Forms.MessageBox.Show(excep.Message);
            }
        }

        /// <summary>
        /// Returns the cosine of a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            double result = 0;

            cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");

            while (!isValid(cellSelection))
            {
                MessageBox.Show("Please enter a valid cell name");
                cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");
            }

            try
            {
                if (double.TryParse(Sheet.GetCellValue(cellSelection).ToString(), out temp))
                {
                    result = Math.Cos(temp);
                }
            }
            catch (Exception excep)
            {
                throw new Exception("The cell " + cellSelection + " cannot be parsed into a double");
            }

            if (MessageBox.Show("The cosine  of the cell is: " + result + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + result + "to", "");

                updateCells(cellSelection, result);
            }
        }

        /// <summary>
        /// Returns the tangent of the cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            double result = 0;

            cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");

            while (!isValid(cellSelection))
            {
                MessageBox.Show("Please enter a valid cell name");
                cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");
            }

            try
            {
                if (double.TryParse(Sheet.GetCellValue(cellSelection).ToString(), out temp))
                {
                    result = Math.Tan(temp);
                }
            }
            catch (Exception excep)
            {
                throw new Exception("The cell " + cellSelection + " cannot be parsed into a double");
            }

            if (MessageBox.Show("The tangent  of the cell is: " + result + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + result + "to", "");

                updateCells(cellSelection, result);
            }
        }
        
        /// <summary>
        /// Returns the sine of a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            double result = 0;

            cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");

            while (!isValid(cellSelection))
            {
                MessageBox.Show("Please enter a valid cell name");
                cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");
            }

            try
            {
                if (double.TryParse(Sheet.GetCellValue(cellSelection).ToString(), out temp))
                {
                    result = Math.Sin(temp);
                }
            }
            catch (Exception excep)
            {
                throw new Exception("The cell " + cellSelection + " cannot be parsed into a double");
            }

            if (MessageBox.Show("The sin of the cell is: " + result + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + result + "to", "");

                updateCells(cellSelection, result);
            }
        }

        /// <summary>
        /// Returns the cotangent of a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cotangentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cellSelection = "A1";
            double temp;
            double result = 0;

            cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");

            while (!isValid(cellSelection))
            {
                MessageBox.Show("Please enter a valid cell name");
                cellSelection = PromptForSelection.ShowDialog("Enter Cell Name", "");
            }

            try
            {
                if (double.TryParse(Sheet.GetCellValue(cellSelection).ToString(), out temp))
                {
                    result = Math.Tan(temp);
                    result = 1 / result;
                }
            }
            catch (Exception excep)
            {
                throw new Exception("The cell " + cellSelection + " cannot be parsed into a double");
            }

            if (MessageBox.Show("The cotangent of the cell is: " + result + "\n Do you want to assign this to a cell?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cellSelection = PromptForSelection.ShowDialog("Please enter a cell to assign the number " + result + "to", "");

                updateCells(cellSelection, result);
            }
        }

        private void trignometryHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Click on Math then Trigonometry then click on the function you want to compute. Enter a valid non empty cell name when prompted and its trigonometric function will be computed");
        }

        /// <summary>
        /// Find and replace for strings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findAndReplacestringsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value;
            string valueToReplace;
            HashSet<String> cellList = new HashSet<string>(Sheet.GetNamesOfAllNonemptyCells());

            value = PromptForSelection.ShowDialog("Find", "");
            valueToReplace = PromptForSelection.ShowDialog("Replace with", "");

            try
            {
                foreach (string cell in cellList)
                {
                    if (Sheet.GetCellValue(cell).ToString() == value)
                    {
                        updateCells(cell, valueToReplace);
                    }
                        
                    else
                    {
                        //do nothing
                    }
                }
            }
            catch (Exception excep)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid string");
            }
        }

        // backgroundWorker1.RunAsync();
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            CellsToRecalculate = Sheet.SetContentsOfCell(CellName, Value.ToString());
        }
    }     
}
