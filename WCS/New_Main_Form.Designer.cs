namespace WCS
{
    partial class New_Main_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(New_Main_Form));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblDBConStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPLCStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWorkStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageListView = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listViewCommand = new System.Windows.Forms.ListView();
            this.btnConn = new System.Windows.Forms.ToolStripButton();
            this.btnConnPLC = new System.Windows.Forms.ToolStripButton();
            this.btnInitDev = new System.Windows.Forms.ToolStripButton();
            this.btnStrat = new System.Windows.Forms.ToolStripButton();
            this.btnESC = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConn,
            this.toolStripSeparator1,
            this.btnConnPLC,
            this.toolStripSeparator2,
            this.btnInitDev,
            this.toolStripSeparator3,
            this.btnStrat,
            this.toolStripSeparator4,
            this.toolStripButton1,
            this.btnESC});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1284, 56);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 56);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 56);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 56);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 56);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblDBConStatus,
            this.lblPLCStatus,
            this.lblWorkStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 707);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1284, 26);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblDBConStatus
            // 
            this.lblDBConStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblDBConStatus.Name = "lblDBConStatus";
            this.lblDBConStatus.Size = new System.Drawing.Size(96, 21);
            this.lblDBConStatus.Text = "数据库尚未连接";
            // 
            // lblPLCStatus
            // 
            this.lblPLCStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblPLCStatus.Name = "lblPLCStatus";
            this.lblPLCStatus.Size = new System.Drawing.Size(81, 21);
            this.lblPLCStatus.Text = "尚未连接PLC";
            // 
            // lblWorkStatus
            // 
            this.lblWorkStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblWorkStatus.Name = "lblWorkStatus";
            this.lblWorkStatus.Size = new System.Drawing.Size(60, 21);
            this.lblWorkStatus.Text = "尚未联机";
            // 
            // imageListView
            // 
            this.imageListView.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListView.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListView.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl1.Location = new System.Drawing.Point(0, 57);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1552, 647);
            this.tabControl1.TabIndex = 33;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.LightCyan;
            this.tabPage1.Controls.Add(this.listViewCommand);
            this.tabPage1.Font = new System.Drawing.Font("宋体", 14F);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1544, 617);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "运行监控";
            this.tabPage1.Paint += new System.Windows.Forms.PaintEventHandler(this.tabPage1_Paint);
            // 
            // listViewCommand
            // 
            this.listViewCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCommand.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewCommand.FullRowSelect = true;
            this.listViewCommand.Location = new System.Drawing.Point(732, 0);
            this.listViewCommand.Name = "listViewCommand";
            this.listViewCommand.Size = new System.Drawing.Size(440, 611);
            this.listViewCommand.TabIndex = 5;
            this.listViewCommand.UseCompatibleStateImageBehavior = false;
            this.listViewCommand.View = System.Windows.Forms.View.Details;
            // 
            // btnConn
            // 
            this.btnConn.Image = ((System.Drawing.Image)(resources.GetObject("btnConn.Image")));
            this.btnConn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnConn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConn.Name = "btnConn";
            this.btnConn.Size = new System.Drawing.Size(72, 53);
            this.btnConn.Text = "连接数据库";
            this.btnConn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnConn.Click += new System.EventHandler(this.btnConn_Click);
            // 
            // btnConnPLC
            // 
            this.btnConnPLC.Image = ((System.Drawing.Image)(resources.GetObject("btnConnPLC.Image")));
            this.btnConnPLC.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnConnPLC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnPLC.Name = "btnConnPLC";
            this.btnConnPLC.Size = new System.Drawing.Size(57, 53);
            this.btnConnPLC.Text = "连接PLC";
            this.btnConnPLC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnConnPLC.Click += new System.EventHandler(this.btnConnPLC_Click);
            // 
            // btnInitDev
            // 
            this.btnInitDev.Image = ((System.Drawing.Image)(resources.GetObject("btnInitDev.Image")));
            this.btnInitDev.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnInitDev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInitDev.Name = "btnInitDev";
            this.btnInitDev.Size = new System.Drawing.Size(72, 53);
            this.btnInitDev.Text = "初始化设备";
            this.btnInitDev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnInitDev.Click += new System.EventHandler(this.btnInitDev_Click);
            // 
            // btnStrat
            // 
            this.btnStrat.Image = ((System.Drawing.Image)(resources.GetObject("btnStrat.Image")));
            this.btnStrat.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStrat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStrat.Name = "btnStrat";
            this.btnStrat.Size = new System.Drawing.Size(60, 53);
            this.btnStrat.Text = "联机工作";
            this.btnStrat.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnStrat.Click += new System.EventHandler(this.btnStrat_Click);
            // 
            // btnESC
            // 
            this.btnESC.Image = ((System.Drawing.Image)(resources.GetObject("btnESC.Image")));
            this.btnESC.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnESC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnESC.Name = "btnESC";
            this.btnESC.Size = new System.Drawing.Size(60, 53);
            this.btnESC.Text = "退出系统";
            this.btnESC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnESC.Click += new System.EventHandler(this.btnESC_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(60, 53);
            this.toolStripButton1.Text = "设置出口";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // New_Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(250)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1284, 733);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "New_Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "输送设备控制系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.New_Main_Form_FormClosing);
            this.Load += new System.EventHandler(this.New_Main_Form_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnConn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnConnPLC;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblDBConStatus;
        private System.Windows.Forms.ImageList imageListView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnInitDev;
        private System.Windows.Forms.ToolStripButton btnStrat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnESC;
        private System.Windows.Forms.ToolStripStatusLabel lblPLCStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblWorkStatus;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.ListView listViewCommand;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}