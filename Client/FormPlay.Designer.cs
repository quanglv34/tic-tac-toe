using System;
using System.Windows.Forms;
namespace Client
{
    partial class FormPlay : System.Windows.Forms.Form
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


        private void InitializeComponent()
        {
            this.panelBoard = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxX = new System.Windows.Forms.PictureBox();
            this.pictureBoxO = new System.Windows.Forms.PictureBox();
            this.surrenderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.namePlayer1 = new System.Windows.Forms.Label();
            this.namePlayer2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxO)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBoard
            // 
            this.panelBoard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelBoard.Location = new System.Drawing.Point(12, 4);
            this.panelBoard.Margin = new System.Windows.Forms.Padding(0);
            this.panelBoard.Name = "panelBoard";
            this.panelBoard.Size = new System.Drawing.Size(320, 320);
            this.panelBoard.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 419);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(344, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxX, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxO, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.surrenderButton, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.namePlayer1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.namePlayer2, 2, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 324);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(336, 95);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // pictureBoxX
            // 
            this.pictureBoxX.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pictureBoxX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxX.Enabled = false;
            this.pictureBoxX.Image = global::Client.Properties.Resources.x;
            this.pictureBoxX.Location = new System.Drawing.Point(247, 22);
            this.pictureBoxX.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pictureBoxX.Name = "pictureBoxX";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBoxX, 3);
            this.pictureBoxX.Size = new System.Drawing.Size(42, 42);
            this.pictureBoxX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxX.TabIndex = 2;
            this.pictureBoxX.TabStop = false;
            // 
            // pictureBoxO
            // 
            this.pictureBoxO.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pictureBoxO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxO.Enabled = false;
            this.pictureBoxO.Image = global::Client.Properties.Resources.o;
            this.pictureBoxO.Location = new System.Drawing.Point(46, 22);
            this.pictureBoxO.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pictureBoxO.Name = "pictureBoxO";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBoxO, 3);
            this.pictureBoxO.Size = new System.Drawing.Size(42, 42);
            this.pictureBoxO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxO.TabIndex = 3;
            this.pictureBoxO.TabStop = false;
            // 
            // surrenderButton
            // 
            this.surrenderButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.surrenderButton.Location = new System.Drawing.Point(137, 64);
            this.surrenderButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.surrenderButton.Name = "surrenderButton";
            this.surrenderButton.Size = new System.Drawing.Size(61, 28);
            this.surrenderButton.TabIndex = 4;
            this.surrenderButton.Text = "Give Up";
            this.surrenderButton.UseVisualStyleBackColor = true;
            this.surrenderButton.Click += new System.EventHandler(this.surrenderButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(134, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.label1.Name = "label1";
            this.tableLayoutPanel1.SetRowSpan(this.label1, 3);
            this.label1.Size = new System.Drawing.Size(67, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "VS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // namePlayer1
            // 
            this.namePlayer1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.namePlayer1.AutoSize = true;
            this.namePlayer1.BackColor = System.Drawing.Color.Transparent;
            this.namePlayer1.Location = new System.Drawing.Point(44, 73);
            this.namePlayer1.Name = "namePlayer1";
            this.namePlayer1.Size = new System.Drawing.Size(45, 13);
            this.namePlayer1.TabIndex = 6;
            this.namePlayer1.Text = "Player 1";
            // 
            // namePlayer2
            // 
            this.namePlayer2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.namePlayer2.AutoSize = true;
            this.namePlayer2.BackColor = System.Drawing.Color.Transparent;
            this.namePlayer2.Location = new System.Drawing.Point(246, 73);
            this.namePlayer2.Name = "namePlayer2";
            this.namePlayer2.Size = new System.Drawing.Size(45, 13);
            this.namePlayer2.TabIndex = 7;
            this.namePlayer2.Text = "Player 2";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panelBoard, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(344, 441);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // FormPlay
            // 
            this.ClientSize = new System.Drawing.Size(344, 441);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.MaximizeBox = false;
            this.Name = "FormPlay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxO)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public Panel panelBoard;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBoxX;
        private PictureBox pictureBoxO;
        private Button surrenderButton;
        private Label label1;
        public Label namePlayer1;
        public Label namePlayer2;
        private TableLayoutPanel tableLayoutPanel2;
    }
}