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
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeCellContentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Cell = new System.Windows.Forms.Label();
            this.Cell_Contents = new System.Windows.Forms.Label();
            this.Cell_Content_Type = new System.Windows.Forms.Label();
            this.Cell_Value = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Location = new System.Drawing.Point(1, 54);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1130, 483);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1131, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem1.Text = "Open";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeSelectionToolStripMenuItem,
            this.changeCellContentsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // changeSelectionToolStripMenuItem
            // 
            this.changeSelectionToolStripMenuItem.Name = "changeSelectionToolStripMenuItem";
            this.changeSelectionToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.changeSelectionToolStripMenuItem.Text = "Change Selection";
            this.changeSelectionToolStripMenuItem.Click += new System.EventHandler(this.changeSelectionToolStripMenuItem_Click);
            // 
            // changeCellContentsToolStripMenuItem
            // 
            this.changeCellContentsToolStripMenuItem.Name = "changeCellContentsToolStripMenuItem";
            this.changeCellContentsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.changeCellContentsToolStripMenuItem.Text = "Change Cell Contents";
            this.changeCellContentsToolStripMenuItem.Click += new System.EventHandler(this.changeCellContentsToolStripMenuItem_Click);
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
            this.Cell_Value.Location = new System.Drawing.Point(213, 28);
            this.Cell_Value.Name = "Cell_Value";
            this.Cell_Value.Size = new System.Drawing.Size(57, 13);
            this.Cell_Value.TabIndex = 5;
            this.Cell_Value.Text = "Cell Value:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(78, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.Form1_Load);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(768, 28);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 7;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(276, 28);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 8;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(533, 28);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 537);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
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
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeCellContentsToolStripMenuItem;
        private System.Windows.Forms.Label Cell;
        private System.Windows.Forms.Label Cell_Contents;
        private System.Windows.Forms.Label Cell_Content_Type;
        private System.Windows.Forms.Label Cell_Value;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
    }
}

