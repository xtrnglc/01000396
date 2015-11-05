namespace SpreadsheetGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.New = new System.Windows.Forms.ToolStripMenuItem();
            this.Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Save = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsOption = new System.Windows.Forms.ToolStripMenuItem();
            this.Close = new System.Windows.Forms.ToolStripMenuItem();
            this.mathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.averageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squareRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trigonometryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cotangentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findAndReplaceToolTip = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findAndReplacestringsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeCellContentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savingASpreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openingASpreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mathHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.summingCellsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findingTheAverageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findingTheMaximumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findingTheMinimumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findTheSquareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findingTheSquareRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trignometryHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToFindAndReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToClearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Cell = new System.Windows.Forms.Label();
            this.Cell_Contents = new System.Windows.Forms.Label();
            this.Cell_Content_Type = new System.Windows.Forms.Label();
            this.Cell_Value = new System.Windows.Forms.Label();
            this.Cell_name_text = new System.Windows.Forms.TextBox();
            this.Cell_Value_text = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.Cell_Contents_text = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToSortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(1, 54);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1189, 483);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mathToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1190, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.New,
            this.Open,
            this.Save,
            this.SaveAsOption,
            this.Close});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // New
            // 
            this.New.Name = "New";
            this.New.Size = new System.Drawing.Size(114, 22);
            this.New.Text = "New";
            this.New.Click += new System.EventHandler(this.New_Click);
            // 
            // Open
            // 
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(114, 22);
            this.Open.Text = "Open";
            this.Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // Save
            // 
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(114, 22);
            this.Save.Text = "Save";
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // SaveAsOption
            // 
            this.SaveAsOption.Name = "SaveAsOption";
            this.SaveAsOption.Size = new System.Drawing.Size(114, 22);
            this.SaveAsOption.Text = "Save As";
            this.SaveAsOption.Click += new System.EventHandler(this.SaveAsOption_Click);
            // 
            // Close
            // 
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(114, 22);
            this.Close.Text = "Close";
            this.Close.Click += new System.EventHandler(this.Close_Click_1);
            // 
            // mathToolStripMenuItem
            // 
            this.mathToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sumToolStripMenuItem,
            this.averageToolStripMenuItem,
            this.maxToolStripMenuItem,
            this.minimumToolStripMenuItem,
            this.squareToolStripMenuItem,
            this.squareRootToolStripMenuItem,
            this.trigonometryToolStripMenuItem});
            this.mathToolStripMenuItem.Name = "mathToolStripMenuItem";
            this.mathToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.mathToolStripMenuItem.Text = "Math";
            // 
            // sumToolStripMenuItem
            // 
            this.sumToolStripMenuItem.Name = "sumToolStripMenuItem";
            this.sumToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.sumToolStripMenuItem.Text = "Sum";
            this.sumToolStripMenuItem.Click += new System.EventHandler(this.sumToolStripMenuItem_Click);
            // 
            // averageToolStripMenuItem
            // 
            this.averageToolStripMenuItem.Name = "averageToolStripMenuItem";
            this.averageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.averageToolStripMenuItem.Text = "Average";
            this.averageToolStripMenuItem.Click += new System.EventHandler(this.averageToolStripMenuItem_Click);
            // 
            // maxToolStripMenuItem
            // 
            this.maxToolStripMenuItem.Name = "maxToolStripMenuItem";
            this.maxToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.maxToolStripMenuItem.Text = "Maximum";
            this.maxToolStripMenuItem.Click += new System.EventHandler(this.maxToolStripMenuItem_Click);
            // 
            // minimumToolStripMenuItem
            // 
            this.minimumToolStripMenuItem.Name = "minimumToolStripMenuItem";
            this.minimumToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.minimumToolStripMenuItem.Text = "Minimum";
            this.minimumToolStripMenuItem.Click += new System.EventHandler(this.minimumToolStripMenuItem_Click);
            // 
            // squareToolStripMenuItem
            // 
            this.squareToolStripMenuItem.Name = "squareToolStripMenuItem";
            this.squareToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.squareToolStripMenuItem.Text = "Square";
            this.squareToolStripMenuItem.Click += new System.EventHandler(this.squareToolStripMenuItem_Click);
            // 
            // squareRootToolStripMenuItem
            // 
            this.squareRootToolStripMenuItem.Name = "squareRootToolStripMenuItem";
            this.squareRootToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.squareRootToolStripMenuItem.Text = "Square Root";
            this.squareRootToolStripMenuItem.Click += new System.EventHandler(this.squareRootToolStripMenuItem_Click);
            // 
            // trigonometryToolStripMenuItem
            // 
            this.trigonometryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sinToolStripMenuItem,
            this.cosToolStripMenuItem,
            this.tanToolStripMenuItem,
            this.cotangentToolStripMenuItem});
            this.trigonometryToolStripMenuItem.Name = "trigonometryToolStripMenuItem";
            this.trigonometryToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.trigonometryToolStripMenuItem.Text = "Trigonometry";
            // 
            // sinToolStripMenuItem
            // 
            this.sinToolStripMenuItem.Name = "sinToolStripMenuItem";
            this.sinToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.sinToolStripMenuItem.Text = "Sine";
            this.sinToolStripMenuItem.Click += new System.EventHandler(this.sinToolStripMenuItem_Click);
            // 
            // cosToolStripMenuItem
            // 
            this.cosToolStripMenuItem.Name = "cosToolStripMenuItem";
            this.cosToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.cosToolStripMenuItem.Text = "Cosine";
            this.cosToolStripMenuItem.Click += new System.EventHandler(this.cosToolStripMenuItem_Click);
            // 
            // tanToolStripMenuItem
            // 
            this.tanToolStripMenuItem.Name = "tanToolStripMenuItem";
            this.tanToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.tanToolStripMenuItem.Text = "Tangent";
            this.tanToolStripMenuItem.Click += new System.EventHandler(this.tanToolStripMenuItem_Click);
            // 
            // cotangentToolStripMenuItem
            // 
            this.cotangentToolStripMenuItem.Name = "cotangentToolStripMenuItem";
            this.cotangentToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.cotangentToolStripMenuItem.Text = "Cotangent";
            this.cotangentToolStripMenuItem.Click += new System.EventHandler(this.cotangentToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findAndReplaceToolTip,
            this.clearToolStripMenuItem,
            this.findAndReplacestringsToolStripMenuItem,
            this.sortToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // findAndReplaceToolTip
            // 
            this.findAndReplaceToolTip.Name = "findAndReplaceToolTip";
            this.findAndReplaceToolTip.Size = new System.Drawing.Size(222, 22);
            this.findAndReplaceToolTip.Text = "Find and Replace (numbers)";
            this.findAndReplaceToolTip.Click += new System.EventHandler(this.findAndReplaceToolTip_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // findAndReplacestringsToolStripMenuItem
            // 
            this.findAndReplacestringsToolStripMenuItem.Name = "findAndReplacestringsToolStripMenuItem";
            this.findAndReplacestringsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.findAndReplacestringsToolStripMenuItem.Text = "Find and Replace (strings)";
            this.findAndReplacestringsToolStripMenuItem.Click += new System.EventHandler(this.findAndReplacestringsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeSelectionToolStripMenuItem,
            this.changeCellContentsToolStripMenuItem,
            this.savingASpreadsheetToolStripMenuItem,
            this.openingASpreadsheetToolStripMenuItem,
            this.mathHelpToolStripMenuItem,
            this.editHelpToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // changeSelectionToolStripMenuItem
            // 
            this.changeSelectionToolStripMenuItem.Name = "changeSelectionToolStripMenuItem";
            this.changeSelectionToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.changeSelectionToolStripMenuItem.Text = "Change Selection";
            this.changeSelectionToolStripMenuItem.Click += new System.EventHandler(this.changeSelectionToolStripMenuItem_Click);
            // 
            // changeCellContentsToolStripMenuItem
            // 
            this.changeCellContentsToolStripMenuItem.Name = "changeCellContentsToolStripMenuItem";
            this.changeCellContentsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.changeCellContentsToolStripMenuItem.Text = "Change Cell Contents";
            this.changeCellContentsToolStripMenuItem.Click += new System.EventHandler(this.changeCellContentsToolStripMenuItem_Click);
            // 
            // savingASpreadsheetToolStripMenuItem
            // 
            this.savingASpreadsheetToolStripMenuItem.Name = "savingASpreadsheetToolStripMenuItem";
            this.savingASpreadsheetToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.savingASpreadsheetToolStripMenuItem.Text = "Saving a Spreadsheet";
            this.savingASpreadsheetToolStripMenuItem.Click += new System.EventHandler(this.savingASpreadsheetToolStripMenuItem_Click);
            // 
            // openingASpreadsheetToolStripMenuItem
            // 
            this.openingASpreadsheetToolStripMenuItem.Name = "openingASpreadsheetToolStripMenuItem";
            this.openingASpreadsheetToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.openingASpreadsheetToolStripMenuItem.Text = "Opening a Spreadsheet";
            this.openingASpreadsheetToolStripMenuItem.Click += new System.EventHandler(this.openingASpreadsheetToolStripMenuItem_Click);
            // 
            // mathHelpToolStripMenuItem
            // 
            this.mathHelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.summingCellsToolStripMenuItem,
            this.findingTheAverageToolStripMenuItem,
            this.findingTheMaximumToolStripMenuItem,
            this.findingTheMinimumToolStripMenuItem,
            this.findTheSquareToolStripMenuItem,
            this.findingTheSquareRootToolStripMenuItem,
            this.trignometryHelpToolStripMenuItem});
            this.mathHelpToolStripMenuItem.Name = "mathHelpToolStripMenuItem";
            this.mathHelpToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.mathHelpToolStripMenuItem.Text = "Math Help";
            // 
            // summingCellsToolStripMenuItem
            // 
            this.summingCellsToolStripMenuItem.Name = "summingCellsToolStripMenuItem";
            this.summingCellsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.summingCellsToolStripMenuItem.Text = "Finding the Sum";
            this.summingCellsToolStripMenuItem.Click += new System.EventHandler(this.summingCellsToolStripMenuItem_Click);
            // 
            // findingTheAverageToolStripMenuItem
            // 
            this.findingTheAverageToolStripMenuItem.Name = "findingTheAverageToolStripMenuItem";
            this.findingTheAverageToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.findingTheAverageToolStripMenuItem.Text = "Finding the Average";
            this.findingTheAverageToolStripMenuItem.Click += new System.EventHandler(this.findingTheAverageToolStripMenuItem_Click);
            // 
            // findingTheMaximumToolStripMenuItem
            // 
            this.findingTheMaximumToolStripMenuItem.Name = "findingTheMaximumToolStripMenuItem";
            this.findingTheMaximumToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.findingTheMaximumToolStripMenuItem.Text = "Finding the Maximum";
            this.findingTheMaximumToolStripMenuItem.Click += new System.EventHandler(this.findingTheMaximumToolStripMenuItem_Click);
            // 
            // findingTheMinimumToolStripMenuItem
            // 
            this.findingTheMinimumToolStripMenuItem.Name = "findingTheMinimumToolStripMenuItem";
            this.findingTheMinimumToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.findingTheMinimumToolStripMenuItem.Text = "Finding the Minimum";
            this.findingTheMinimumToolStripMenuItem.Click += new System.EventHandler(this.findingTheMinimumToolStripMenuItem_Click);
            // 
            // findTheSquareToolStripMenuItem
            // 
            this.findTheSquareToolStripMenuItem.Name = "findTheSquareToolStripMenuItem";
            this.findTheSquareToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.findTheSquareToolStripMenuItem.Text = "Finding the Square";
            this.findTheSquareToolStripMenuItem.Click += new System.EventHandler(this.findTheSquareToolStripMenuItem_Click);
            // 
            // findingTheSquareRootToolStripMenuItem
            // 
            this.findingTheSquareRootToolStripMenuItem.Name = "findingTheSquareRootToolStripMenuItem";
            this.findingTheSquareRootToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.findingTheSquareRootToolStripMenuItem.Text = "Finding the Square Root";
            this.findingTheSquareRootToolStripMenuItem.Click += new System.EventHandler(this.findingTheSquareRootToolStripMenuItem_Click);
            // 
            // trignometryHelpToolStripMenuItem
            // 
            this.trignometryHelpToolStripMenuItem.Name = "trignometryHelpToolStripMenuItem";
            this.trignometryHelpToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.trignometryHelpToolStripMenuItem.Text = "Trignometry Help";
            this.trignometryHelpToolStripMenuItem.Click += new System.EventHandler(this.trignometryHelpToolStripMenuItem_Click);
            // 
            // editHelpToolStripMenuItem
            // 
            this.editHelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howToFindAndReplaceToolStripMenuItem,
            this.howToClearToolStripMenuItem,
            this.howToSortToolStripMenuItem});
            this.editHelpToolStripMenuItem.Name = "editHelpToolStripMenuItem";
            this.editHelpToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.editHelpToolStripMenuItem.Text = "Edit Help";
            // 
            // howToFindAndReplaceToolStripMenuItem
            // 
            this.howToFindAndReplaceToolStripMenuItem.Name = "howToFindAndReplaceToolStripMenuItem";
            this.howToFindAndReplaceToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.howToFindAndReplaceToolStripMenuItem.Text = "How to Find and Replace";
            this.howToFindAndReplaceToolStripMenuItem.Click += new System.EventHandler(this.howToFindAndReplaceToolStripMenuItem_Click);
            // 
            // howToClearToolStripMenuItem
            // 
            this.howToClearToolStripMenuItem.Name = "howToClearToolStripMenuItem";
            this.howToClearToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.howToClearToolStripMenuItem.Text = "How to Clear";
            this.howToClearToolStripMenuItem.Click += new System.EventHandler(this.howToClearToolStripMenuItem_Click);
            // 
            // Cell
            // 
            this.Cell.AutoSize = true;
            this.Cell.Location = new System.Drawing.Point(13, 28);
            this.Cell.Name = "Cell";
            this.Cell.Size = new System.Drawing.Size(58, 13);
            this.Cell.TabIndex = 2;
            this.Cell.Text = "Cell Name:";
            // 
            // Cell_Contents
            // 
            this.Cell_Contents.AutoSize = true;
            this.Cell_Contents.Location = new System.Drawing.Point(690, 28);
            this.Cell_Contents.Name = "Cell_Contents";
            this.Cell_Contents.Size = new System.Drawing.Size(72, 13);
            this.Cell_Contents.TabIndex = 3;
            this.Cell_Contents.Text = "Cell Contents:";
            this.Cell_Contents.Click += new System.EventHandler(this.label1_Click);
            // 
            // Cell_Content_Type
            // 
            this.Cell_Content_Type.AutoSize = true;
            this.Cell_Content_Type.Location = new System.Drawing.Point(433, 28);
            this.Cell_Content_Type.Name = "Cell_Content_Type";
            this.Cell_Content_Type.Size = new System.Drawing.Size(94, 13);
            this.Cell_Content_Type.TabIndex = 4;
            this.Cell_Content_Type.Text = "Cell Content Type:";
            // 
            // Cell_Value
            // 
            this.Cell_Value.AutoSize = true;
            this.Cell_Value.Location = new System.Drawing.Point(163, 28);
            this.Cell_Value.Name = "Cell_Value";
            this.Cell_Value.Size = new System.Drawing.Size(57, 13);
            this.Cell_Value.TabIndex = 5;
            this.Cell_Value.Text = "Cell Value:";
            // 
            // Cell_name_text
            // 
            this.Cell_name_text.Location = new System.Drawing.Point(78, 28);
            this.Cell_name_text.Name = "Cell_name_text";
            this.Cell_name_text.ReadOnly = true;
            this.Cell_name_text.Size = new System.Drawing.Size(56, 20);
            this.Cell_name_text.TabIndex = 6;
            this.Cell_name_text.TextChanged += new System.EventHandler(this.spreadsheetPanel1_Load);
            // 
            // Cell_Value_text
            // 
            this.Cell_Value_text.Location = new System.Drawing.Point(226, 25);
            this.Cell_Value_text.Name = "Cell_Value_text";
            this.Cell_Value_text.ReadOnly = true;
            this.Cell_Value_text.Size = new System.Drawing.Size(154, 20);
            this.Cell_Value_text.TabIndex = 7;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(533, 25);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 8;
            // 
            // Cell_Contents_text
            // 
            this.Cell_Contents_text.Location = new System.Drawing.Point(768, 25);
            this.Cell_Contents_text.Name = "Cell_Contents_text";
            this.Cell_Contents_text.Size = new System.Drawing.Size(174, 20);
            this.Cell_Contents_text.TabIndex = 9;
            this.Cell_Contents_text.TextChanged += new System.EventHandler(this.Cell_Contents_text_TextChanged);
            this.Cell_Contents_text.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Cell_Contents_text_KeyPress);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.sortToolStripMenuItem.Text = "Sort";
            this.sortToolStripMenuItem.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
            // 
            // howToSortToolStripMenuItem
            // 
            this.howToSortToolStripMenuItem.Name = "howToSortToolStripMenuItem";
            this.howToSortToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.howToSortToolStripMenuItem.Text = "How to Sort";
            this.howToSortToolStripMenuItem.Click += new System.EventHandler(this.howToSortToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 537);
            this.Controls.Add(this.Cell_Contents_text);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.Cell_Value_text);
            this.Controls.Add(this.Cell_name_text);
            this.Controls.Add(this.Cell_Value);
            this.Controls.Add(this.Cell_Content_Type);
            this.Controls.Add(this.Cell_Contents);
            this.Controls.Add(this.Cell);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Open;
        private System.Windows.Forms.ToolStripMenuItem New;
        private System.Windows.Forms.ToolStripMenuItem Save;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeCellContentsToolStripMenuItem;
        private System.Windows.Forms.Label Cell;
        private System.Windows.Forms.Label Cell_Contents;
        private System.Windows.Forms.Label Cell_Content_Type;
        private System.Windows.Forms.Label Cell_Value;
        private System.Windows.Forms.TextBox Cell_name_text;
        private System.Windows.Forms.TextBox Cell_Value_text;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox Cell_Contents_text;
        private System.Windows.Forms.ToolStripMenuItem SaveAsOption;
        private System.Windows.Forms.ToolStripMenuItem Close;
        private System.Windows.Forms.ToolStripMenuItem savingASpreadsheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openingASpreadsheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem averageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem maxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem squareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem squareRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mathHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem summingCellsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findingTheAverageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findingTheMaximumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findingTheMinimumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findTheSquareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findingTheSquareRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findAndReplaceToolTip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howToFindAndReplaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howToClearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trigonometryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cotangentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trignometryHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findAndReplacestringsToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howToSortToolStripMenuItem;
    }
}

