using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WCS.DevSystem
{
    public partial class FrmSSJ : Form
    {
        public RadioButton[] rdButton;
        private SSJ_Dev ssj_dev;
        //private string deviceID;
        //private string deviceName;
        private int tag;

        private string strCommand;
        public FrmSSJ(SSJ_Dev ssj_dev, int tag)
        {
            InitializeComponent();
            this.ssj_dev = ssj_dev;
            this.tag = tag;
            this.Text = ssj_dev.deviceId[tag] + "的状态及手动控制窗口";

            //是否进行手动控制
            if (ssj_dev.controlType[tag].Trim().Length > 0)
            {
                rdButton = new RadioButton[ssj_dev.mainFrm.deviceControlDic.Count];
                int i = 0;
                foreach (string str in ssj_dev.mainFrm.deviceControlDic.Keys)
                {
                    if (ssj_dev.mainFrm.deviceControlDic[str].controlType == ssj_dev.controlType[tag])
                    {
                        rdButton[i] = new RadioButton();
                        rdButton[i].Name = ssj_dev.mainFrm.deviceControlDic[str].controlId;
                        rdButton[i].Text = ssj_dev.mainFrm.deviceControlDic[str].controlDesc;
                        rdButton[i].Location = new Point(20 + i % 2 * 160, 20 + 30 * (int)(i / 2));
                        rdButton[i].Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                        rdButton[i].Size = new Size(150, 20);
                        this.groupBox1.Controls.Add(rdButton[i]);
                        rdButton[i].CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
                        i++;
                    }
                }          
                rdButton[0].Checked = true;
                strCommand = rdButton[0].Name;
            }
            else
            {
                this.groupBox1.Visible = false;
                btnSendCommand.Visible = false;
            }
            RefrushStatus(tag);
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                strCommand = ((RadioButton)sender).Name;//Name绑定的是Control_ID
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RefrushStatus(int tag)
        {
            //1 状态
            if (ssj_dev.mainFrm.deviceStatusDic.ContainsKey(ssj_dev.statusType[tag] + ssj_dev.deviceStatus[tag].ToString()) == false)
            {

                lbDeviceStatus.Text = "设备状态：未知状态，值：" + ssj_dev.deviceStatus[tag].ToString();
            }
            else
            {
                lbDeviceStatus.Text = "设备状态:" + ssj_dev.mainFrm.deviceStatusDic[ssj_dev.statusType[tag] + ssj_dev.deviceStatus[tag].ToString()].statusDesc;
            }

            ////2 故障
            //if (ssj_dev.mainFrm.deviceFailureDic.ContainsKey(ssj_dev.failureType[tag] + ssj_dev.deviceFailure[tag].ToString()) == false)
            //{

            //    lbDeviceFailure.Text = "设备故障：未知故障，值：" + ssj_dev.deviceFailure[tag].ToString();
            //}
            //else
            //{
            //    lbDeviceFailure.Text = "设备故障:" + ssj_dev.mainFrm.deviceFailureDic[ssj_dev.failureType[tag] + ssj_dev.deviceFailure[tag].ToString()].failureDesc;
            //}

            //3,主柜连接
            if (ssj_dev.statusType[tag] == "MS")//plc主柜  
            {
                lbPlcStatus.Visible = true;
                if (ssj_dev.mainFrm.plcSystemMS.plcStatus == true)
                {
                    lbPlcStatus.Text = "主柜连接：正常";
                }
                else
                {
                    lbPlcStatus.Text = "主柜连接：断开";
                }
            }
            else
            {
                lbPlcStatus.Visible = false;
            }
            
        }

        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            ssj_dev.ManulCommand(tag, strCommand);
        }
    }
}
