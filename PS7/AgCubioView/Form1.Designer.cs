namespace AgCubioView
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
            this.PlayerNameLabel = new System.Windows.Forms.Label();
            this.PlayerNameTextBox = new System.Windows.Forms.TextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.ServerTextBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.FPSlabel = new System.Windows.Forms.Label();
            this.Foodlabel = new System.Windows.Forms.Label();
            this.Mass = new System.Windows.Forms.Label();
            this.Widthlabel = new System.Windows.Forms.Label();
            this.Widthtext = new System.Windows.Forms.Label();
            this.Masstext = new System.Windows.Forms.Label();
            this.foodtext = new System.Windows.Forms.Label();
            this.FPStext = new System.Windows.Forms.Label();
            this.food = new System.Windows.Forms.Label();
            this.foodtextbox = new System.Windows.Forms.Label();
            this.MassLabel = new System.Windows.Forms.Label();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.masstextright = new System.Windows.Forms.Label();
            this.playerseatenlabel = new System.Windows.Forms.Label();
            this.playerseatentextbox = new System.Windows.Forms.Label();
            this.TimeAliveLabel = new System.Windows.Forms.Label();
            this.timealivetext = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // PlayerNameLabel
            // 
            this.PlayerNameLabel.AutoSize = true;
            this.PlayerNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerNameLabel.Location = new System.Drawing.Point(44, 116);
            this.PlayerNameLabel.Name = "PlayerNameLabel";
            this.PlayerNameLabel.Size = new System.Drawing.Size(178, 31);
            this.PlayerNameLabel.TabIndex = 0;
            this.PlayerNameLabel.Text = "Player Name:";
            // 
            // PlayerNameTextBox
            // 
            this.PlayerNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerNameTextBox.Location = new System.Drawing.Point(228, 116);
            this.PlayerNameTextBox.Name = "PlayerNameTextBox";
            this.PlayerNameTextBox.Size = new System.Drawing.Size(241, 38);
            this.PlayerNameTextBox.TabIndex = 1;
            this.PlayerNameTextBox.TextChanged += new System.EventHandler(this.PlayerNameTextBox_TextChanged);
            this.PlayerNameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PlayerNameTextBox_KeyPress_1);
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerLabel.Location = new System.Drawing.Point(120, 172);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(102, 31);
            this.ServerLabel.TabIndex = 2;
            this.ServerLabel.Text = "Server:";
            this.ServerLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerTextBox.Location = new System.Drawing.Point(228, 172);
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.Size = new System.Drawing.Size(241, 38);
            this.ServerTextBox.TabIndex = 3;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(270, 235);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(143, 53);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // FPSlabel
            // 
            this.FPSlabel.AutoSize = true;
            this.FPSlabel.Location = new System.Drawing.Point(1118, 20);
            this.FPSlabel.Name = "FPSlabel";
            this.FPSlabel.Size = new System.Drawing.Size(27, 13);
            this.FPSlabel.TabIndex = 5;
            this.FPSlabel.Text = "FPS";
            // 
            // Foodlabel
            // 
            this.Foodlabel.AutoSize = true;
            this.Foodlabel.Location = new System.Drawing.Point(1118, 49);
            this.Foodlabel.Name = "Foodlabel";
            this.Foodlabel.Size = new System.Drawing.Size(31, 13);
            this.Foodlabel.TabIndex = 6;
            this.Foodlabel.Text = "Food";
            // 
            // Mass
            // 
            this.Mass.AutoSize = true;
            this.Mass.Location = new System.Drawing.Point(1118, 72);
            this.Mass.Name = "Mass";
            this.Mass.Size = new System.Drawing.Size(32, 13);
            this.Mass.TabIndex = 7;
            this.Mass.Text = "Mass";
            // 
            // Widthlabel
            // 
            this.Widthlabel.AutoSize = true;
            this.Widthlabel.Location = new System.Drawing.Point(1118, 100);
            this.Widthlabel.Name = "Widthlabel";
            this.Widthlabel.Size = new System.Drawing.Size(35, 13);
            this.Widthlabel.TabIndex = 8;
            this.Widthlabel.Text = "Width";
            // 
            // Widthtext
            // 
            this.Widthtext.AutoSize = true;
            this.Widthtext.Location = new System.Drawing.Point(1176, 100);
            this.Widthtext.Name = "Widthtext";
            this.Widthtext.Size = new System.Drawing.Size(13, 13);
            this.Widthtext.TabIndex = 12;
            this.Widthtext.Text = "0";
            this.Widthtext.UseWaitCursor = true;
            // 
            // Masstext
            // 
            this.Masstext.AutoSize = true;
            this.Masstext.Location = new System.Drawing.Point(1176, 72);
            this.Masstext.Name = "Masstext";
            this.Masstext.Size = new System.Drawing.Size(13, 13);
            this.Masstext.TabIndex = 11;
            this.Masstext.Text = "0";
            // 
            // foodtext
            // 
            this.foodtext.AutoSize = true;
            this.foodtext.Location = new System.Drawing.Point(1176, 49);
            this.foodtext.Name = "foodtext";
            this.foodtext.Size = new System.Drawing.Size(29, 13);
            this.foodtext.TabIndex = 10;
            this.foodtext.Text = "false";
            // 
            // FPStext
            // 
            this.FPStext.AutoSize = true;
            this.FPStext.Location = new System.Drawing.Point(1176, 20);
            this.FPStext.Name = "FPStext";
            this.FPStext.Size = new System.Drawing.Size(13, 13);
            this.FPStext.TabIndex = 9;
            this.FPStext.Text = "0";
            // 
            // food
            // 
            this.food.AutoSize = true;
            this.food.Location = new System.Drawing.Point(703, 28);
            this.food.Name = "food";
            this.food.Size = new System.Drawing.Size(62, 13);
            this.food.TabIndex = 13;
            this.food.Text = "Food Eaten";
            this.food.Click += new System.EventHandler(this.food_Click);
            // 
            // foodtextbox
            // 
            this.foodtextbox.AutoSize = true;
            this.foodtextbox.Location = new System.Drawing.Point(786, 28);
            this.foodtextbox.Name = "foodtextbox";
            this.foodtextbox.Size = new System.Drawing.Size(13, 13);
            this.foodtextbox.TabIndex = 14;
            this.foodtextbox.Text = "0";
            this.foodtextbox.Click += new System.EventHandler(this.foodtextbox_Click);
            // 
            // MassLabel
            // 
            this.MassLabel.AutoSize = true;
            this.MassLabel.Location = new System.Drawing.Point(733, 55);
            this.MassLabel.Name = "MassLabel";
            this.MassLabel.Size = new System.Drawing.Size(32, 13);
            this.MassLabel.TabIndex = 15;
            this.MassLabel.Text = "Mass";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // masstextright
            // 
            this.masstextright.AutoSize = true;
            this.masstextright.Location = new System.Drawing.Point(786, 55);
            this.masstextright.Name = "masstextright";
            this.masstextright.Size = new System.Drawing.Size(13, 13);
            this.masstextright.TabIndex = 16;
            this.masstextright.Text = "0";
            // 
            // playerseatenlabel
            // 
            this.playerseatenlabel.AutoSize = true;
            this.playerseatenlabel.Location = new System.Drawing.Point(703, 79);
            this.playerseatenlabel.Name = "playerseatenlabel";
            this.playerseatenlabel.Size = new System.Drawing.Size(72, 13);
            this.playerseatenlabel.TabIndex = 17;
            this.playerseatenlabel.Text = "Players Eaten";
            this.playerseatenlabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // playerseatentextbox
            // 
            this.playerseatentextbox.AutoSize = true;
            this.playerseatentextbox.Location = new System.Drawing.Point(786, 79);
            this.playerseatentextbox.Name = "playerseatentextbox";
            this.playerseatentextbox.Size = new System.Drawing.Size(13, 13);
            this.playerseatentextbox.TabIndex = 18;
            this.playerseatentextbox.Text = "0";
            // 
            // TimeAliveLabel
            // 
            this.TimeAliveLabel.AutoSize = true;
            this.TimeAliveLabel.Location = new System.Drawing.Point(703, 101);
            this.TimeAliveLabel.Name = "TimeAliveLabel";
            this.TimeAliveLabel.Size = new System.Drawing.Size(56, 13);
            this.TimeAliveLabel.TabIndex = 19;
            this.TimeAliveLabel.Text = "Time Alive";
            // 
            // timealivetext
            // 
            this.timealivetext.AutoSize = true;
            this.timealivetext.Location = new System.Drawing.Point(786, 101);
            this.timealivetext.Name = "timealivetext";
            this.timealivetext.Size = new System.Drawing.Size(13, 13);
            this.timealivetext.TabIndex = 20;
            this.timealivetext.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 562);
            this.Controls.Add(this.timealivetext);
            this.Controls.Add(this.TimeAliveLabel);
            this.Controls.Add(this.playerseatentextbox);
            this.Controls.Add(this.playerseatenlabel);
            this.Controls.Add(this.masstextright);
            this.Controls.Add(this.MassLabel);
            this.Controls.Add(this.foodtextbox);
            this.Controls.Add(this.food);
            this.Controls.Add(this.Widthtext);
            this.Controls.Add(this.Masstext);
            this.Controls.Add(this.foodtext);
            this.Controls.Add(this.FPStext);
            this.Controls.Add(this.Widthlabel);
            this.Controls.Add(this.Mass);
            this.Controls.Add(this.Foodlabel);
            this.Controls.Add(this.FPSlabel);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ServerTextBox);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.PlayerNameTextBox);
            this.Controls.Add(this.PlayerNameLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PlayerNameLabel;
        private System.Windows.Forms.TextBox PlayerNameTextBox;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.TextBox ServerTextBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label FPSlabel;
        private System.Windows.Forms.Label Foodlabel;
        private System.Windows.Forms.Label Mass;
        private System.Windows.Forms.Label Widthlabel;
        private System.Windows.Forms.Label Widthtext;
        private System.Windows.Forms.Label Masstext;
        private System.Windows.Forms.Label foodtext;
        private System.Windows.Forms.Label FPStext;
        private System.Windows.Forms.Label food;
        private System.Windows.Forms.Label foodtextbox;
        private System.Windows.Forms.Label MassLabel;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Label masstextright;
        private System.Windows.Forms.Label playerseatenlabel;
        private System.Windows.Forms.Label playerseatentextbox;
        private System.Windows.Forms.Label timealivetext;
        private System.Windows.Forms.Label TimeAliveLabel;
    }
}

