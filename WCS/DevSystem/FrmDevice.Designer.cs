namespace WCS.DevSystem
{
    partial class FrmDevice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDevice));
            this.btExit = new System.Windows.Forms.Button();
            this.btSendCommand = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbDeviceStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chkIsRequestIn = new System.Windows.Forms.CheckBox();
            this.lvTask = new System.Windows.Forms.ListView();
            this.chkIsRefresh = new System.Windows.Forms.CheckBox();
            this.btMannullCross = new System.Windows.Forms.Button();
            this.chkScanner = new System.Windows.Forms.CheckBox();
            this.btEdit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btExit
            // 
            this.btExit.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btExit.Image = ((System.Drawing.Image)(resources.GetObject("btExit.Image")));
            this.btExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btExit.Location = new System.Drawing.Point(727, 455);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(95, 30);
            this.btExit.TabIndex = 6;
            this.btExit.Text = "关闭";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSendCommand
            // 
            this.btSendCommand.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btSendCommand.Image = ((System.Drawing.Image)(resources.GetObject("btSendCommand.Image")));
            this.btSendCommand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btSendCommand.Location = new System.Drawing.Point(605, 455);
            this.btSendCommand.Name = "btSendCommand";
            this.btSendCommand.Size = new System.Drawing.Size(101, 30);
            this.btSendCommand.TabIndex = 5;
            this.btSendCommand.Text = "发送指令";
            this.btSendCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btSendCommand.UseVisualStyleBackColor = true;
            this.btSendCommand.Click += new System.EventHandler(this.btSendCommand_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lbDeviceStatus);
            this.panel1.Location = new System.Drawing.Point(1, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(832, 38);
            this.panel1.TabIndex = 4;
            // 
            // lbDeviceStatus
            // 
            this.lbDeviceStatus.AutoSize = true;
            this.lbDeviceStatus.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDeviceStatus.Location = new System.Drawing.Point(7, 9);
            this.lbDeviceStatus.Name = "lbDeviceStatus";
            this.lbDeviceStatus.Size = new System.Drawing.Size(120, 16);
            this.lbDeviceStatus.TabIndex = 0;
            this.lbDeviceStatus.Text = "设备状态：正常";
            // 
            // chkIsRequestIn
            // 
            this.chkIsRequestIn.Location = new System.Drawing.Point(232, 461);
            this.chkIsRequestIn.Name = "chkIsRequestIn";
            this.chkIsRequestIn.Size = new System.Drawing.Size(104, 24);
            this.chkIsRequestIn.TabIndex = 10;
            // 
            // lvTask
            // 
            this.lvTask.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lvTask.FullRowSelect = true;
            this.lvTask.GridLines = true;
            this.lvTask.Location = new System.Drawing.Point(1, 47);
            this.lvTask.MultiSelect = false;
            this.lvTask.Name = "lvTask";
            this.lvTask.Size = new System.Drawing.Size(832, 404);
            this.lvTask.TabIndex = 8;
            this.lvTask.UseCompatibleStateImageBehavior = false;
            this.lvTask.View = System.Windows.Forms.View.Details;
            // 
            // chkIsRefresh
            // 
            this.chkIsRefresh.AutoSize = true;
            this.chkIsRefresh.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsRefresh.Location = new System.Drawing.Point(25, 467);
            this.chkIsRefresh.Name = "chkIsRefresh";
            this.chkIsRefresh.Size = new System.Drawing.Size(82, 18);
            this.chkIsRefresh.TabIndex = 9;
            this.chkIsRefresh.Text = "自动刷新";
            this.chkIsRefresh.UseVisualStyleBackColor = true;
            // 
            // btMannullCross
            // 
            this.btMannullCross.Location = new System.Drawing.Point(176, 457);
            this.btMannullCross.Name = "btMannullCross";
            this.btMannullCross.Size = new System.Drawing.Size(75, 23);
            this.btMannullCross.TabIndex = 2;
            // 
            // chkScanner
            // 
            this.chkScanner.Location = new System.Drawing.Point(122, 467);
            this.chkScanner.Name = "chkScanner";
            this.chkScanner.Size = new System.Drawing.Size(104, 24);
            this.chkScanner.TabIndex = 1;
            // 
            // btEdit
            // 
            this.btEdit.Location = new System.Drawing.Point(139, 465);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(75, 23);
            this.btEdit.TabIndex = 0;
            // 
            // FrmDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(834, 499);
            this.Controls.Add(this.btEdit);
            this.Controls.Add(this.chkScanner);
            this.Controls.Add(this.btMannullCross);
            this.Controls.Add(this.chkIsRefresh);
            this.Controls.Add(this.lvTask);
            this.Controls.Add(this.chkIsRequestIn);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btSendCommand);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDevice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备状态显示处理";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSendCommand;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbDeviceStatus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox chkIsRequestIn;
        private System.Windows.Forms.ListView lvTask;
        private System.Windows.Forms.CheckBox chkIsRefresh;
        private System.Windows.Forms.Button btMannullCross;
        private System.Windows.Forms.CheckBox chkScanner;
        private System.Windows.Forms.Button btEdit;
    }
}