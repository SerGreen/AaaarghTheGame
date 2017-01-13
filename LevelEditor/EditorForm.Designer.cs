namespace LevelEditor
{
    partial class EditorForm
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
            this.screen = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.fixBridge = new System.Windows.Forms.Button();
            this.toolsFocus = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericWidth = new System.Windows.Forms.NumericUpDown();
            this.numericHeight = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.newMap = new System.Windows.Forms.Button();
            this.mapListBox = new System.Windows.Forms.ComboBox();
            this.save = new System.Windows.Forms.Button();
            this.load = new System.Windows.Forms.Button();
            this.down = new System.Windows.Forms.Button();
            this.right = new System.Windows.Forms.Button();
            this.up = new System.Windows.Forms.Button();
            this.left = new System.Windows.Forms.Button();
            this.backgroundBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.screen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // screen
            // 
            this.screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screen.Location = new System.Drawing.Point(0, 0);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(1069, 510);
            this.screen.TabIndex = 0;
            this.screen.TabStop = false;
            this.screen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screen_MouseDown);
            this.screen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.screen_MouseMove);
            this.screen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.screen_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.renderTimer);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.screen);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.backgroundBox);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.fixBridge);
            this.splitContainer1.Panel2.Controls.Add(this.toolsFocus);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.numericWidth);
            this.splitContainer1.Panel2.Controls.Add(this.numericHeight);
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.newMap);
            this.splitContainer1.Panel2.Controls.Add(this.mapListBox);
            this.splitContainer1.Panel2.Controls.Add(this.save);
            this.splitContainer1.Panel2.Controls.Add(this.load);
            this.splitContainer1.Panel2.Controls.Add(this.down);
            this.splitContainer1.Panel2.Controls.Add(this.right);
            this.splitContainer1.Panel2.Controls.Add(this.up);
            this.splitContainer1.Panel2.Controls.Add(this.left);
            this.splitContainer1.Size = new System.Drawing.Size(1069, 546);
            this.splitContainer1.SplitterDistance = 510;
            this.splitContainer1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(260, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "fix";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fixBridge
            // 
            this.fixBridge.Location = new System.Drawing.Point(229, 5);
            this.fixBridge.Name = "fixBridge";
            this.fixBridge.Size = new System.Drawing.Size(25, 23);
            this.fixBridge.TabIndex = 11;
            this.fixBridge.Text = "fix";
            this.fixBridge.UseVisualStyleBackColor = true;
            this.fixBridge.Click += new System.EventHandler(this.fixBridge_Click);
            // 
            // toolsFocus
            // 
            this.toolsFocus.Location = new System.Drawing.Point(148, 5);
            this.toolsFocus.Name = "toolsFocus";
            this.toolsFocus.Size = new System.Drawing.Size(75, 23);
            this.toolsFocus.TabIndex = 10;
            this.toolsFocus.Text = "Focus tools";
            this.toolsFocus.UseVisualStyleBackColor = true;
            this.toolsFocus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolsFocus_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(506, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "X";
            // 
            // numericWidth
            // 
            this.numericWidth.Location = new System.Drawing.Point(454, 7);
            this.numericWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericWidth.Name = "numericWidth";
            this.numericWidth.Size = new System.Drawing.Size(46, 20);
            this.numericWidth.TabIndex = 8;
            this.numericWidth.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // numericHeight
            // 
            this.numericHeight.Location = new System.Drawing.Point(526, 7);
            this.numericHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericHeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericHeight.Name = "numericHeight";
            this.numericHeight.Size = new System.Drawing.Size(49, 20);
            this.numericHeight.TabIndex = 7;
            this.numericHeight.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(581, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(102, 20);
            this.textBox1.TabIndex = 6;
            // 
            // newMap
            // 
            this.newMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newMap.Location = new System.Drawing.Point(689, 5);
            this.newMap.Name = "newMap";
            this.newMap.Size = new System.Drawing.Size(75, 23);
            this.newMap.TabIndex = 5;
            this.newMap.Text = "New";
            this.newMap.UseVisualStyleBackColor = true;
            this.newMap.Click += new System.EventHandler(this.newMap_Click);
            // 
            // mapListBox
            // 
            this.mapListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mapListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mapListBox.FormattingEnabled = true;
            this.mapListBox.Location = new System.Drawing.Point(851, 7);
            this.mapListBox.Name = "mapListBox";
            this.mapListBox.Size = new System.Drawing.Size(121, 21);
            this.mapListBox.TabIndex = 4;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.save.Location = new System.Drawing.Point(770, 5);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 3;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // load
            // 
            this.load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.load.Location = new System.Drawing.Point(978, 5);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(75, 23);
            this.load.TabIndex = 2;
            this.load.Text = "Load";
            this.load.UseVisualStyleBackColor = true;
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // down
            // 
            this.down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.down.Location = new System.Drawing.Point(105, 5);
            this.down.Name = "down";
            this.down.Size = new System.Drawing.Size(25, 23);
            this.down.TabIndex = 1;
            this.down.Text = "\\/";
            this.down.UseVisualStyleBackColor = true;
            this.down.Click += new System.EventHandler(this.down_Click);
            // 
            // right
            // 
            this.right.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.right.Location = new System.Drawing.Point(43, 5);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(25, 23);
            this.right.TabIndex = 0;
            this.right.Text = ">";
            this.right.UseVisualStyleBackColor = true;
            this.right.Click += new System.EventHandler(this.right_Click);
            // 
            // up
            // 
            this.up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.up.Location = new System.Drawing.Point(74, 5);
            this.up.Name = "up";
            this.up.Size = new System.Drawing.Size(25, 23);
            this.up.TabIndex = 0;
            this.up.Text = "/\\";
            this.up.UseVisualStyleBackColor = true;
            this.up.Click += new System.EventHandler(this.up_Click);
            // 
            // left
            // 
            this.left.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.left.Location = new System.Drawing.Point(12, 5);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(25, 23);
            this.left.TabIndex = 0;
            this.left.Text = "<";
            this.left.UseVisualStyleBackColor = true;
            this.left.Click += new System.EventHandler(this.left_Click);
            // 
            // backgroundBox
            // 
            this.backgroundBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.backgroundBox.FormattingEnabled = true;
            this.backgroundBox.Location = new System.Drawing.Point(346, 6);
            this.backgroundBox.Name = "backgroundBox";
            this.backgroundBox.Size = new System.Drawing.Size(102, 21);
            this.backgroundBox.TabIndex = 13;
            this.backgroundBox.SelectedIndexChanged += new System.EventHandler(this.backgroundBox_SelectedIndexChanged);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 546);
            this.Controls.Add(this.splitContainer1);
            this.Name = "EditorForm";
            this.Text = "Editor : Object tool";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditorForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.screen)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox screen;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button down;
        private System.Windows.Forms.Button right;
        private System.Windows.Forms.Button up;
        private System.Windows.Forms.Button left;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button load;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button newMap;
        private System.Windows.Forms.ComboBox mapListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericWidth;
        private System.Windows.Forms.NumericUpDown numericHeight;
        private System.Windows.Forms.Button toolsFocus;
        private System.Windows.Forms.Button fixBridge;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox backgroundBox;

    }
}

