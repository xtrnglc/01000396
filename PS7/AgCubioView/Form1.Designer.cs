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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 562);
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
            this.Load += new System.EventHandler(this.Form1_Load);
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
    }
}

