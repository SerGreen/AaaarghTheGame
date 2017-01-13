namespace Client
{
    partial class GameWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameWindow));
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.teamBox = new System.Windows.Forms.ComboBox();
            this.classBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.loginBox = new System.Windows.Forms.GroupBox();
            this.joinButton = new System.Windows.Forms.Button();
            this.titleBackground = new System.Windows.Forms.PictureBox();
            this.screen = new System.Windows.Forms.PictureBox();
            this.loginBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.screen)).BeginInit();
            this.SuspendLayout();
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(210, 32);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(100, 20);
            this.ipTextBox.TabIndex = 2;
            this.ipTextBox.Text = "localhost";
            this.ipTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.join_KeyDown);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(9, 32);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(100, 20);
            this.nameTextBox.TabIndex = 3;
            this.nameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.join_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "IP address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Team";
            // 
            // teamBox
            // 
            this.teamBox.DisplayMember = "0";
            this.teamBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.teamBox.FormattingEnabled = true;
            this.teamBox.Items.AddRange(new object[] {
            "Team 1",
            "Team 2",
            "Team 3",
            "Team 4"});
            this.teamBox.Location = new System.Drawing.Point(9, 87);
            this.teamBox.Name = "teamBox";
            this.teamBox.Size = new System.Drawing.Size(100, 21);
            this.teamBox.TabIndex = 7;
            this.teamBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.join_KeyDown);
            // 
            // classBox
            // 
            this.classBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classBox.FormattingEnabled = true;
            this.classBox.Items.AddRange(new object[] {
            "Warrior",
            "Berserker",
            "Bowman",
            "Arbalester",
            "Knight",
            "Thief"});
            this.classBox.Location = new System.Drawing.Point(210, 87);
            this.classBox.Name = "classBox";
            this.classBox.Size = new System.Drawing.Size(100, 21);
            this.classBox.TabIndex = 8;
            this.classBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.join_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(278, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Class";
            // 
            // loginBox
            // 
            this.loginBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loginBox.Controls.Add(this.label1);
            this.loginBox.Controls.Add(this.label4);
            this.loginBox.Controls.Add(this.nameTextBox);
            this.loginBox.Controls.Add(this.classBox);
            this.loginBox.Controls.Add(this.label3);
            this.loginBox.Controls.Add(this.label2);
            this.loginBox.Controls.Add(this.teamBox);
            this.loginBox.Controls.Add(this.ipTextBox);
            this.loginBox.Controls.Add(this.joinButton);
            this.loginBox.Location = new System.Drawing.Point(415, 274);
            this.loginBox.Name = "loginBox";
            this.loginBox.Size = new System.Drawing.Size(318, 136);
            this.loginBox.TabIndex = 10;
            this.loginBox.TabStop = false;
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point(122, 58);
            this.joinButton.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(75, 23);
            this.joinButton.TabIndex = 1;
            this.joinButton.Text = "Join";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.JoinBtn_Click);
            // 
            // titleBackground
            // 
            this.titleBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBackground.Image = ((System.Drawing.Image)(resources.GetObject("titleBackground.Image")));
            this.titleBackground.InitialImage = null;
            this.titleBackground.Location = new System.Drawing.Point(0, 0);
            this.titleBackground.Name = "titleBackground";
            this.titleBackground.Size = new System.Drawing.Size(854, 480);
            this.titleBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.titleBackground.TabIndex = 11;
            this.titleBackground.TabStop = false;
            // 
            // screen
            // 
            this.screen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(19)))), ((int)(((byte)(3)))));
            this.screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screen.Location = new System.Drawing.Point(0, 0);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(854, 480);
            this.screen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.screen.TabIndex = 0;
            this.screen.TabStop = false;
            this.screen.Visible = false;
            this.screen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.screen_MouseClick);
            this.screen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameWindow_MouseMove);
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 480);
            this.Controls.Add(this.loginBox);
            this.Controls.Add(this.titleBackground);
            this.Controls.Add(this.screen);
            this.MaximumSize = new System.Drawing.Size(1721, 998);
            this.MinimumSize = new System.Drawing.Size(870, 518);
            this.Name = "GameWindow";
            this.Text = "Aaaaaagh!!!";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameWindow_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameWindow_MouseMove);
            this.loginBox.ResumeLayout(false);
            this.loginBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.screen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox screen;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox teamBox;
        private System.Windows.Forms.ComboBox classBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox loginBox;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.PictureBox titleBackground;
    }
}

