namespace DatasheetProofer
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.loadDatasheetButton = new System.Windows.Forms.Button();
            this.loadScriptsButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDatasheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadScriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(12, 612);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(992, 117);
            this.textBox1.TabIndex = 0;
            // 
            // loadDatasheetButton
            // 
            this.loadDatasheetButton.Location = new System.Drawing.Point(911, 19);
            this.loadDatasheetButton.Name = "loadDatasheetButton";
            this.loadDatasheetButton.Size = new System.Drawing.Size(75, 38);
            this.loadDatasheetButton.TabIndex = 2;
            this.loadDatasheetButton.Text = "Load Datasheet";
            this.loadDatasheetButton.UseVisualStyleBackColor = true;
            this.loadDatasheetButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // loadScriptsButton
            // 
            this.loadScriptsButton.Enabled = false;
            this.loadScriptsButton.Location = new System.Drawing.Point(911, 63);
            this.loadScriptsButton.Name = "loadScriptsButton";
            this.loadScriptsButton.Size = new System.Drawing.Size(75, 36);
            this.loadScriptsButton.TabIndex = 3;
            this.loadScriptsButton.Text = "Load Scripts...";
            this.loadScriptsButton.UseVisualStyleBackColor = true;
            this.loadScriptsButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(899, 541);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.loadDatasheetButton);
            this.groupBox1.Controls.Add(this.loadScriptsButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(992, 566);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datasheet";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1016, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDatasheetToolStripMenuItem,
            this.loadScriptsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(35, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // loadDatasheetToolStripMenuItem
            // 
            this.loadDatasheetToolStripMenuItem.Name = "loadDatasheetToolStripMenuItem";
            this.loadDatasheetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadDatasheetToolStripMenuItem.Text = "Load Datasheet";
            this.loadDatasheetToolStripMenuItem.Click += new System.EventHandler(this.loadDatasheetToolStripMenuItem_Click);
            // 
            // loadScriptsToolStripMenuItem
            // 
            this.loadScriptsToolStripMenuItem.Enabled = false;
            this.loadScriptsToolStripMenuItem.Name = "loadScriptsToolStripMenuItem";
            this.loadScriptsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadScriptsToolStripMenuItem.Text = "Load Scripts";
            this.loadScriptsToolStripMenuItem.Click += new System.EventHandler(this.loadScriptsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 741);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button loadDatasheetButton;
        private System.Windows.Forms.Button loadScriptsButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadDatasheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadScriptsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

