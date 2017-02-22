namespace WCS.DevSystem
{
    partial class FrmJT
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
            this.lable1 = new System.Windows.Forms.Label();
            this.lblDeviceInfo = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lable1
            // 
            this.lable1.AutoSize = true;
            this.lable1.Font = new System.Drawing.Font("宋体", 12F);
            this.lable1.Location = new System.Drawing.Point(21, 83);
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size(88, 16);
            this.lable1.TabIndex = 0;
            this.lable1.Text = "状态信息：";
            // 
            // lblDeviceInfo
            // 
            this.lblDeviceInfo.AutoSize = true;
            this.lblDeviceInfo.Font = new System.Drawing.Font("宋体", 12F);
            this.lblDeviceInfo.Location = new System.Drawing.Point(115, 83);
            this.lblDeviceInfo.Name = "lblDeviceInfo";
            this.lblDeviceInfo.Size = new System.Drawing.Size(40, 16);
            this.lblDeviceInfo.TabIndex = 0;
            this.lblDeviceInfo.Text = "状态";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("宋体", 12F);
            this.btnClose.Location = new System.Drawing.Point(94, 180);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 39);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // FrmJT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(250)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(297, 264);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblDeviceInfo);
            this.Controls.Add(this.lable1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmJT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmJT";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lable1;
        private System.Windows.Forms.Label lblDeviceInfo;
        private System.Windows.Forms.Button btnClose;
    }
}