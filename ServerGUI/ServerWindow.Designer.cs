namespace ServerGUI
{
    partial class ServerWindow
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
            this.components = new System.ComponentModel.Container();
            this.playersList = new System.Windows.Forms.ListView();
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTeam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHealth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKills = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssists = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeaths = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKDRatio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bStop = new System.Windows.Forms.Button();
            this.bResart = new System.Windows.Forms.Button();
            this.bRespawn = new System.Windows.Forms.Button();
            this.bKick = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.teamScoreList = new System.Windows.Forms.ListView();
            this.cTeam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cScore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cPlayers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.selectedPlayerID = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.mapsListBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.respawnTimeBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TeamsBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timeBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gameModeBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pointsLimitBox = new System.Windows.Forms.ComboBox();
            this.pointLimitLabel = new System.Windows.Forms.Label();
            this.spawnsLimitBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.selectedPlayerID)).BeginInit();
            this.SuspendLayout();
            // 
            // playersList
            // 
            this.playersList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playersList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colID,
            this.colName,
            this.colTeam,
            this.colHealth,
            this.colKills,
            this.colAssists,
            this.colDeaths,
            this.colKDRatio,
            this.colClass});
            this.playersList.Location = new System.Drawing.Point(149, 12);
            this.playersList.MultiSelect = false;
            this.playersList.Name = "playersList";
            this.playersList.Size = new System.Drawing.Size(507, 155);
            this.playersList.TabIndex = 0;
            this.playersList.UseCompatibleStateImageBehavior = false;
            this.playersList.View = System.Windows.Forms.View.Details;
            // 
            // colID
            // 
            this.colID.Text = "ID";
            this.colID.Width = 40;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 92;
            // 
            // colTeam
            // 
            this.colTeam.Text = "Team";
            this.colTeam.Width = 45;
            // 
            // colHealth
            // 
            this.colHealth.Text = "Health";
            this.colHealth.Width = 46;
            // 
            // colKills
            // 
            this.colKills.Text = "Kills";
            this.colKills.Width = 45;
            // 
            // colAssists
            // 
            this.colAssists.Text = "Assists";
            this.colAssists.Width = 50;
            // 
            // colDeaths
            // 
            this.colDeaths.Text = "Deaths";
            this.colDeaths.Width = 52;
            // 
            // colKDRatio
            // 
            this.colKDRatio.Text = "K/D";
            this.colKDRatio.Width = 47;
            // 
            // colClass
            // 
            this.colClass.Text = "Class";
            // 
            // bStop
            // 
            this.bStop.Location = new System.Drawing.Point(12, 173);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(50, 37);
            this.bStop.TabIndex = 1;
            this.bStop.Text = "Stop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bResart
            // 
            this.bResart.Location = new System.Drawing.Point(68, 173);
            this.bResart.Name = "bResart";
            this.bResart.Size = new System.Drawing.Size(75, 37);
            this.bResart.TabIndex = 1;
            this.bResart.Text = "Restart";
            this.bResart.UseVisualStyleBackColor = true;
            this.bResart.Click += new System.EventHandler(this.bResart_Click);
            // 
            // bRespawn
            // 
            this.bRespawn.Location = new System.Drawing.Point(68, 12);
            this.bRespawn.Name = "bRespawn";
            this.bRespawn.Size = new System.Drawing.Size(75, 26);
            this.bRespawn.TabIndex = 1;
            this.bRespawn.Text = "Respawn";
            this.bRespawn.UseVisualStyleBackColor = true;
            this.bRespawn.Click += new System.EventHandler(this.bRespawn_Click);
            // 
            // bKick
            // 
            this.bKick.Location = new System.Drawing.Point(12, 12);
            this.bKick.Name = "bKick";
            this.bKick.Size = new System.Drawing.Size(50, 26);
            this.bKick.TabIndex = 1;
            this.bKick.Text = "Kick";
            this.bKick.UseVisualStyleBackColor = true;
            this.bKick.Click += new System.EventHandler(this.bKick_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 500;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // teamScoreList
            // 
            this.teamScoreList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cTeam,
            this.cScore,
            this.cPlayers});
            this.teamScoreList.Location = new System.Drawing.Point(12, 70);
            this.teamScoreList.Name = "teamScoreList";
            this.teamScoreList.Size = new System.Drawing.Size(131, 97);
            this.teamScoreList.TabIndex = 2;
            this.teamScoreList.UseCompatibleStateImageBehavior = false;
            this.teamScoreList.View = System.Windows.Forms.View.Details;
            // 
            // cTeam
            // 
            this.cTeam.Text = "Team";
            this.cTeam.Width = 40;
            // 
            // cScore
            // 
            this.cScore.Text = "Score";
            this.cScore.Width = 41;
            // 
            // cPlayers
            // 
            this.cPlayers.Text = "Players";
            this.cPlayers.Width = 46;
            // 
            // selectedPlayerID
            // 
            this.selectedPlayerID.Location = new System.Drawing.Point(68, 44);
            this.selectedPlayerID.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.selectedPlayerID.Name = "selectedPlayerID";
            this.selectedPlayerID.Size = new System.Drawing.Size(75, 20);
            this.selectedPlayerID.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Player ID";
            // 
            // mapsListBox
            // 
            this.mapsListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mapsListBox.FormattingEnabled = true;
            this.mapsListBox.Location = new System.Drawing.Point(586, 186);
            this.mapsListBox.Name = "mapsListBox";
            this.mapsListBox.Size = new System.Drawing.Size(70, 21);
            this.mapsListBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(583, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Map";
            // 
            // respawnTimeBox
            // 
            this.respawnTimeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.respawnTimeBox.FormattingEnabled = true;
            this.respawnTimeBox.Items.AddRange(new object[] {
            "no respawn",
            "1",
            "3",
            "6",
            "9",
            "15",
            "20"});
            this.respawnTimeBox.Location = new System.Drawing.Point(501, 186);
            this.respawnTimeBox.Name = "respawnTimeBox";
            this.respawnTimeBox.Size = new System.Drawing.Size(79, 21);
            this.respawnTimeBox.TabIndex = 7;
            this.respawnTimeBox.SelectedIndexChanged += new System.EventHandler(this.respawnTimeBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(498, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Respawn time";
            // 
            // TeamsBox
            // 
            this.TeamsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TeamsBox.FormattingEnabled = true;
            this.TeamsBox.Items.AddRange(new object[] {
            "2",
            "4"});
            this.TeamsBox.Location = new System.Drawing.Point(449, 186);
            this.TeamsBox.Name = "TeamsBox";
            this.TeamsBox.Size = new System.Drawing.Size(46, 21);
            this.TeamsBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(446, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Teams";
            // 
            // timeBox
            // 
            this.timeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timeBox.FormattingEnabled = true;
            this.timeBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "5",
            "8",
            "10",
            "15",
            "20"});
            this.timeBox.Location = new System.Drawing.Point(397, 186);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(46, 21);
            this.timeBox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(394, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Time limit";
            // 
            // gameModeBox
            // 
            this.gameModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameModeBox.FormattingEnabled = true;
            this.gameModeBox.Items.AddRange(new object[] {
            "Deathmatch",
            "Domination",
            "Capture the flag",
            "Protect the king",
            "Harvester"});
            this.gameModeBox.Location = new System.Drawing.Point(149, 186);
            this.gameModeBox.Name = "gameModeBox";
            this.gameModeBox.Size = new System.Drawing.Size(101, 21);
            this.gameModeBox.TabIndex = 9;
            this.gameModeBox.SelectedIndexChanged += new System.EventHandler(this.gameModeBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(149, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Game mode";
            // 
            // pointsLimitBox
            // 
            this.pointsLimitBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pointsLimitBox.FormattingEnabled = true;
            this.pointsLimitBox.Location = new System.Drawing.Point(256, 186);
            this.pointsLimitBox.Name = "pointsLimitBox";
            this.pointsLimitBox.Size = new System.Drawing.Size(67, 21);
            this.pointsLimitBox.TabIndex = 10;
            // 
            // pointLimitLabel
            // 
            this.pointLimitLabel.AutoSize = true;
            this.pointLimitLabel.Location = new System.Drawing.Point(253, 170);
            this.pointLimitLabel.Name = "pointLimitLabel";
            this.pointLimitLabel.Size = new System.Drawing.Size(33, 13);
            this.pointLimitLabel.TabIndex = 11;
            this.pointLimitLabel.Text = "Frags";
            // 
            // spawnsLimitBox
            // 
            this.spawnsLimitBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.spawnsLimitBox.FormattingEnabled = true;
            this.spawnsLimitBox.Items.AddRange(new object[] {
            "no limit",
            "5",
            "10",
            "20",
            "30",
            "40",
            "50",
            "75",
            "100"});
            this.spawnsLimitBox.Location = new System.Drawing.Point(329, 186);
            this.spawnsLimitBox.Name = "spawnsLimitBox";
            this.spawnsLimitBox.Size = new System.Drawing.Size(62, 21);
            this.spawnsLimitBox.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(326, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Spawns limit";
            // 
            // ServerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 219);
            this.Controls.Add(this.spawnsLimitBox);
            this.Controls.Add(this.pointLimitLabel);
            this.Controls.Add(this.pointsLimitBox);
            this.Controls.Add(this.gameModeBox);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.TeamsBox);
            this.Controls.Add(this.respawnTimeBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mapsListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.selectedPlayerID);
            this.Controls.Add(this.teamScoreList);
            this.Controls.Add(this.bKick);
            this.Controls.Add(this.bRespawn);
            this.Controls.Add(this.bResart);
            this.Controls.Add(this.bStop);
            this.Controls.Add(this.playersList);
            this.MaximumSize = new System.Drawing.Size(684, 289);
            this.MinimumSize = new System.Drawing.Size(684, 38);
            this.Name = "ServerWindow";
            this.Text = "Aaaaaargh! Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.selectedPlayerID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView playersList;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colTeam;
        private System.Windows.Forms.ColumnHeader colHealth;
        private System.Windows.Forms.ColumnHeader colKills;
        private System.Windows.Forms.ColumnHeader colDeaths;
        private System.Windows.Forms.ColumnHeader colKDRatio;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Button bResart;
        private System.Windows.Forms.Button bRespawn;
        private System.Windows.Forms.Button bKick;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.ColumnHeader colClass;
        private System.Windows.Forms.ListView teamScoreList;
        private System.Windows.Forms.ColumnHeader cTeam;
        private System.Windows.Forms.ColumnHeader cScore;
        private System.Windows.Forms.ColumnHeader cPlayers;
        private System.Windows.Forms.NumericUpDown selectedPlayerID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader colAssists;
        private System.Windows.Forms.ComboBox mapsListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox respawnTimeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox TeamsBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox timeBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox gameModeBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox pointsLimitBox;
        private System.Windows.Forms.Label pointLimitLabel;
        private System.Windows.Forms.ComboBox spawnsLimitBox;
        private System.Windows.Forms.Label label7;
    }
}

