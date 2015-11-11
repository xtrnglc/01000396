namespace View
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
            this.PlayerNameText = new System.Windows.Forms.TextBox();
            this.ServerText = new System.Windows.Forms.TextBox();
            this.PlayerName = new System.Windows.Forms.Label();
            this.Server = new System.Windows.Forms.Label();
            this.FPSlabel = new System.Windows.Forms.Label();
            this.FoodLabel = new System.Windows.Forms.Label();
            this.MassLabel = new System.Windows.Forms.Label();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.FPStext = new System.Windows.Forms.Label();
            this.MassText = new System.Windows.Forms.Label();
            this.WidthText = new System.Windows.Forms.Label();
            this.FoodText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // PlayerNameText
            // 
            this.PlayerNameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerNameText.Location = new System.Drawing.Point(241, 203);
            this.PlayerNameText.Name = "PlayerNameText";
            this.PlayerNameText.Size = new System.Drawing.Size(469, 38);
            this.PlayerNameText.TabIndex = 0;
            this.PlayerNameText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PlayerNameText_KeyPress);
            // 
            // ServerText
            // 
            this.ServerText.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerText.Location = new System.Drawing.Point(241, 273);
            this.ServerText.Name = "ServerText";
            this.ServerText.Size = new System.Drawing.Size(469, 38);
            this.ServerText.TabIndex = 1;
            // 
            // PlayerName
            // 
            this.PlayerName.AutoSize = true;
            this.PlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerName.Location = new System.Drawing.Point(65, 206);
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.Size = new System.Drawing.Size(170, 31);
            this.PlayerName.TabIndex = 2;
            this.PlayerName.Text = "Player Name";
            // 
            // Server
            // 
            this.Server.AutoSize = true;
            this.Server.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Server.Location = new System.Drawing.Point(150, 273);
            this.Server.Name = "Server";
            this.Server.Size = new System.Drawing.Size(94, 31);
            this.Server.TabIndex = 3;
            this.Server.Text = "Server";
            // 
            // FPSlabel
            // 
            this.FPSlabel.AutoSize = true;
            this.FPSlabel.Location = new System.Drawing.Point(845, 9);
            this.FPSlabel.Name = "FPSlabel";
            this.FPSlabel.Size = new System.Drawing.Size(27, 13);
            this.FPSlabel.TabIndex = 4;
            this.FPSlabel.Text = "FPS";
            // 
            // FoodLabel
            // 
            this.FoodLabel.AutoSize = true;
            this.FoodLabel.Location = new System.Drawing.Point(845, 37);
            this.FoodLabel.Name = "FoodLabel";
            this.FoodLabel.Size = new System.Drawing.Size(31, 13);
            this.FoodLabel.TabIndex = 5;
            this.FoodLabel.Text = "Food";
            // 
            // MassLabel
            // 
            this.MassLabel.AutoSize = true;
            this.MassLabel.Location = new System.Drawing.Point(845, 62);
            this.MassLabel.Name = "MassLabel";
            this.MassLabel.Size = new System.Drawing.Size(32, 13);
            this.MassLabel.TabIndex = 6;
            this.MassLabel.Text = "Mass";
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(845, 91);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(35, 13);
            this.WidthLabel.TabIndex = 7;
            this.WidthLabel.Text = "Width";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // FPStext
            // 
            this.FPStext.AutoSize = true;
            this.FPStext.Location = new System.Drawing.Point(899, 9);
            this.FPStext.Name = "FPStext";
            this.FPStext.Size = new System.Drawing.Size(27, 13);
            this.FPStext.TabIndex = 8;
            this.FPStext.Text = "FPS";
            // 
            // MassText
            // 
            this.MassText.AutoSize = true;
            this.MassText.Location = new System.Drawing.Point(899, 62);
            this.MassText.Name = "MassText";
            this.MassText.Size = new System.Drawing.Size(32, 13);
            this.MassText.TabIndex = 10;
            this.MassText.Text = "Mass";
            // 
            // WidthText
            // 
            this.WidthText.AutoSize = true;
            this.WidthText.Location = new System.Drawing.Point(899, 91);
            this.WidthText.Name = "WidthText";
            this.WidthText.Size = new System.Drawing.Size(35, 13);
            this.WidthText.TabIndex = 11;
            this.WidthText.Text = "Width";
            // 
            // FoodText
            // 
            this.FoodText.AutoSize = true;
            this.FoodText.Location = new System.Drawing.Point(899, 37);
            this.FoodText.Name = "FoodText";
            this.FoodText.Size = new System.Drawing.Size(31, 13);
            this.FoodText.TabIndex = 9;
            this.FoodText.Text = "none";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 652);
            this.Controls.Add(this.WidthText);
            this.Controls.Add(this.MassText);
            this.Controls.Add(this.FoodText);
            this.Controls.Add(this.FPStext);
            this.Controls.Add(this.WidthLabel);
            this.Controls.Add(this.MassLabel);
            this.Controls.Add(this.FoodLabel);
            this.Controls.Add(this.FPSlabel);
            this.Controls.Add(this.Server);
            this.Controls.Add(this.PlayerName);
            this.Controls.Add(this.ServerText);
            this.Controls.Add(this.PlayerNameText);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseHover += new System.EventHandler(this.Form1_MouseHover);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PlayerNameText;
        private System.Windows.Forms.TextBox ServerText;
        private System.Windows.Forms.Label PlayerName;
        private System.Windows.Forms.Label Server;
        private System.Windows.Forms.Label FPSlabel;
        private System.Windows.Forms.Label FoodLabel;
        private System.Windows.Forms.Label MassLabel;
        private System.Windows.Forms.Label WidthLabel;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Label WidthText;
        private System.Windows.Forms.Label MassText;
        private System.Windows.Forms.Label FoodText;
        private System.Windows.Forms.Label FPStext;
    }
}

