namespace WCS.DevSystem
{
    partial class FrmSSJ
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbDeviceFailure = new System.Windows.Forms.Label();
            this.lbDeviceStatus = new System.Windows.Forms.Label();
            this.lbPlcStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(229, 348);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 36);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSendCommand.Location = new System.Drawing.Point(54, 348);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(96, 36);
            this.btnSendCommand.TabIndex = 13;
            this.btnSendCommand.Text = "发送命令";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new System.EventHandler(this.btnSendCommand_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(3, 107);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 222);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "点动控制";
            // 
            // lbDeviceFailure
            // 
            this.lbDeviceFailure.AutoSize = true;
            this.lbDeviceFailure.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDeviceFailure.Location = new System.Drawing.Point(0, 42);
            this.lbDeviceFailure.Name = "lbDeviceFailure";
            this.lbDeviceFailure.Size = new System.Drawing.Size(88, 16);
            this.lbDeviceFailure.TabIndex = 11;
            this.lbDeviceFailure.Text = "设备故障：";
            this.lbDeviceFailure.Visible = false;
            // 
            // lbDeviceStatus
            // 
            this.lbDeviceStatus.AutoSize = true;
            this.lbDeviceStatus.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDeviceStatus.Location = new System.Drawing.Point(0, 10);
            this.lbDeviceStatus.Name = "lbDeviceStatus";
            this.lbDeviceStatus.Size = new System.Drawing.Size(88, 16);
            this.lbDeviceStatus.TabIndex = 10;
            this.lbDeviceStatus.Text = "设备状态：";
            // 
            // lbPlcStatus
            // 
            this.lbPlcStatus.AutoSize = true;
            this.lbPlcStatus.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPlcStatus.Location = new System.Drawing.Point(0, 74);
            this.lbPlcStatus.Name = "lbPlcStatus";
            this.lbPlcStatus.Size = new System.Drawing.Size(88, 16);
            this.lbPlcStatus.TabIndex = 15;
            this.lbPlcStatus.Text = "主柜连接：";
            // 
            // FrmSSJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(250)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(396, 391);
            this.Controls.Add(this.lbPlcStatus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSendCommand);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbDeviceFailure);
            this.Controls.Add(this.lbDeviceStatus);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSSJ";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmSSJ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSendCommand;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbDeviceFailure;
        private System.Windows.Forms.Label lbDeviceStatus;
        private System.Windows.Forms.Label lbPlcStatus;
    }
}