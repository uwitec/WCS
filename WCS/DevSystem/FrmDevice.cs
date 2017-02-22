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
    /// <summary>
    /// 设备状态窗口
    /// </summary>
    public partial class FrmDevice : Form
    {
        /// <summary>
        /// 设备类对象
        /// </summary>
        public BESDevice device;
        /// <summary>
        /// 单选按钮数组，用于设备手动功能
        /// </summary>
        public RadioButton[] rdButton;
        /// <summary>
        /// 命令ID
        /// </summary>
        public String strCommand;

        #region 构造函数
        /// <summary>
        /// 初始化设备状态窗体
        /// </summary>
        /// <param name="device">设备对象</param>
        public FrmDevice(BESDevice device)
        {
            InitializeComponent();

            this.device = device;
            this.Text = device.deviceId + device.deviceName + "的状态及手动控制窗口";
            rdButton = new RadioButton[device.mainFrm.deviceControlDic.Count];
            int i = 0;
            if (device.commandDB.Length > 2 || device.deviceId.Substring(0, 2) == "SM") //不通用
            {
                foreach (string str in device.mainFrm.deviceControlDic.Keys)
                {
                    if (device.mainFrm.deviceControlDic[str].controlType == device.controlType)
                    {
                        rdButton[i] = new RadioButton();
                        rdButton[i].Name = device.mainFrm.deviceControlDic[str].controlId;
                        rdButton[i].Text = device.mainFrm.deviceControlDic[str].controlDesc;
                        rdButton[i].Location = new Point(20 + i % 6 * 135, 55 + 25 * (int)(i / 6));
                        rdButton[i].Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                        rdButton[i].Size = new Size(125, 20);
                        this.Controls.Add(rdButton[i]);
                        rdButton[i].CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
                        i++;
                    }
                }
            }
            else
            {
                foreach (string str in device.mainFrm.deviceControlDic.Keys)
                {
                    if (device.mainFrm.deviceControlDic[str].controlType == device.controlType)
                    {
                        rdButton[i] = new RadioButton();
                        rdButton[i].Name = device.mainFrm.deviceControlDic[str].controlId;
                        rdButton[i].Text = device.mainFrm.deviceControlDic[str].controlDesc;
                        rdButton[i].Location = new Point(20 + i % 4 * 135, 55 + 25 * (int)(i / 4));
                        rdButton[i].Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                        rdButton[i].Size = new Size(125, 20);
                        this.Controls.Add(rdButton[i]);
                        rdButton[i].CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
                        i++;
                    }
                }
            }
            rdButton[0].Checked = true;
            strCommand = rdButton[0].Name;
            Panel panel = new Panel();
            panel.Location = new Point(1, 46);
            panel.Size = new Size(panel1.Width, rdButton[i - 1].Bottom - 35);
            panel.BorderStyle = BorderStyle.Fixed3D;
            this.Controls.Add(panel);
            if (device.commandDB.Length > 200 || device.deviceId.Substring(0, 2) == "SM1") //不通用
            {
                lvTask.Columns.Add("任务顺序", (int)(lvTask.Width * 0.0), HorizontalAlignment.Center); //每个任务的唯一ID
                lvTask.Columns.Add("任务号", (int)(lvTask.Width * 0.08), HorizontalAlignment.Center);
                lvTask.Columns.Add("顺序号", (int)(lvTask.Width * 0.08), HorizontalAlignment.Center);
                lvTask.Columns.Add("卷烟名称", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
                lvTask.Columns.Add("起始地址", (int)(lvTask.Width * 0.09), HorizontalAlignment.Center);
                lvTask.Columns.Add("目的地址", (int)(lvTask.Width * 0.09), HorizontalAlignment.Center);
                lvTask.Columns.Add("起始通道", (int)(lvTask.Width * 0.09), HorizontalAlignment.Center);
                lvTask.Columns.Add("目的通道", (int)(lvTask.Width * 0.09), HorizontalAlignment.Center);
                lvTask.Columns.Add("计划数", (int)(lvTask.Width * 0.072), HorizontalAlignment.Center);
                lvTask.Columns.Add("写入数", (int)(lvTask.Width * 0.072), HorizontalAlignment.Center);
                lvTask.Columns.Add("完成数", (int)(lvTask.Width * 0.072), HorizontalAlignment.Center);
                lvTask.Columns.Add("状态", (int)(lvTask.Width * 0.06), HorizontalAlignment.Center);
                lvTask.Location = new Point(lvTask.Location.X, panel.Bottom + 7);

                chkIsRefresh.Location = new Point(lvTask.Location.X + 50, lvTask.Bottom + 12);
                chkIsRequestIn.Location = new Point(chkIsRefresh.Location.X + 115, lvTask.Bottom + 12);
                if (device.deviceId.Substring(0, 2) == "SM")  //扫描
                {
                    chkIsRefresh.Location = new Point(lvTask.Location.X + 30, lvTask.Bottom + 12);
                    chkIsRequestIn.Location = new Point(chkIsRefresh.Location.X + 115, lvTask.Bottom + 12);
                    chkScanner.Location = new Point(chkIsRequestIn.Location.X + 115, lvTask.Bottom + 12);
                    btMannullCross.Location = new Point(chkScanner.Location.X + 115, lvTask.Bottom + 7);
                    btEdit.Location = new Point(btMannullCross.Location.X + 115, lvTask.Bottom + 7);
                    btSendCommand.Location = new Point(btEdit.Location.X + 115, lvTask.Bottom + 7);
                    btExit.Location = new Point(btSendCommand.Location.X + 115, lvTask.Bottom + 7);
                    if (device.scannerNo == "" )
                    {
                        btMannullCross.Enabled = false;
                        chkScanner.Enabled = false;
                    }
                   
                }
                else if (device.deviceId.Substring(0, 2) == "DY") //叠烟机
                {
                    chkScanner.Visible = false;
                    btMannullCross.Visible = false;
                    btEdit.Location = new Point(chkIsRequestIn.Location.X + 125, lvTask.Bottom + 7);
                    btSendCommand.Location = new Point(btEdit.Location.X + 125, lvTask.Bottom + 7);
                    btExit.Location = new Point(btSendCommand.Location.X + 125, lvTask.Bottom + 7);
                }
                else
                {
                    chkScanner.Visible = false;
                    btMannullCross.Visible = false;
                    btEdit.Visible = false;
                    btSendCommand.Location = new Point(chkIsRequestIn.Location.X + 125, lvTask.Bottom + 7);
                    btExit.Location = new Point(btSendCommand.Location.X + 125, lvTask.Bottom + 7);
                }
                this.Size = new Size(this.Width, btExit.Bottom + btExit.Height + 13);
            }
            else
            {
                lvTask.Visible = false;
                chkScanner.Visible = false;
                btMannullCross.Visible = false;
                chkIsRefresh.Location = new Point(chkIsRefresh.Location.X, lvTask.Bottom + 12);
                chkIsRequestIn.Location = new Point(chkIsRefresh.Location.X + 125, panel.Bottom + 12);
                btSendCommand.Location = new Point(chkIsRequestIn.Location.X + 125, panel.Bottom + 7);
                btExit.Location = new Point(btSendCommand.Location.X + 125, panel.Bottom + 7);
                this.Size = new Size(566, btExit.Bottom + btExit.Height + 13);
            }
            chkIsRefresh.Checked = false;
            //panel1.Size = new Size(this.Width - 8, panel1.Height);
            panel.Size = new Size(panel1.Width, panel.Height);

            chkScanner.Checked = device.checkBarCode;

            if (device.deviceId.Substring(0, 4) == "MJTD") //密集通道
            {
                if (device.isRequestIn == "0")
                    chkIsRequestIn.Checked = false;
                else
                    chkIsRequestIn.Checked = true;
            }
            else
            {
                chkIsRequestIn.Enabled = false;
            }
            //if(device.mainFrm.runModel != "T")
            //    chkIsRequestIn.Enabled = false;
            RefreshStatus();
           

        }
        #endregion

        /// <summary>
        /// radioButton点击选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                strCommand = ((RadioButton)sender).Name;
        }
        /// <summary>
        /// 页面初始化载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConvyer_Load(object sender, EventArgs e)
        {
            RefreshStatus();
        }
        /// <summary>
        /// 刷新设备状态
        /// </summary>
        private void RefreshStatus()
        {
            if (device.mainFrm.deviceStatusDic.ContainsKey(device.statusType + device.deviceStatus) == false)
            {
                if (device.deviceStatus == "0")
                {
                    lbDeviceStatus.Text = "状态：正常";
                }
                else
                {
                    lbDeviceStatus.Text = "状态：未知错误，值：" + device.deviceStatus;
                }
                if (device.loadType == "MJTD")
                {
                    lbDeviceStatus.Text += "，空位:" + device.kongWeiNum;
                }
            }
            else
            {
                lbDeviceStatus.Text = "状态:" + device.mainFrm.deviceStatusDic[device.statusType + device.deviceStatus].statusDesc;
                if (device.loadDB.Length > 2)
                {
                    switch (device.loadType)
                    {
                        case "MJTD": //密集通道的信息
                            lbDeviceStatus.Text += "," + "空位" + device.kongWeiNum.ToString();
                            break;
                        case "DY":  //垛烟信息
                            if (device.loadStatus == "0")
                                lbDeviceStatus.Text += ",无货";
                            else
                                lbDeviceStatus.Text += ",有货," + "任务" + device.taskId + ",目的" + device.toAddr + ",件烟数" + device.jyNum.ToString();
                            break;
                       
                        default:
                            break;
                    }
                }
                if (device.deviceId.Substring(0, 3) == "CYJ")
                {
                    // 拆烟机
                    if (device.returnTaskId != null)
                        lbDeviceStatus.Text += ",任务" + device.returnTaskId;
                    if (device.returnToSortLine != null)
                        lbDeviceStatus.Text += ",目的" + device.returnToSortLine;
                    if (device.returnTaskStatus != null)
                        lbDeviceStatus.Text += ",状态" + device.returnTaskStatus;
                }
                if (device.returnDB.Length > 2)  //显示返回信息
                {
                    switch (device.commandType)
                    {
                        case "SYK":   //上烟口
                            lbDeviceStatus.Text += ",任务" + device.returnTaskId;
                            lbDeviceStatus.Text += ",任务数" + device.returnTaskNum.ToString();
                            lbDeviceStatus.Text += ",完成数" + device.returnFinishedNum.ToString();
                            lbDeviceStatus.Text += ",状态" + device.returnTaskStatus;
                            break;
                        case "SMFL":  //扫描核对分流
                            lbDeviceStatus.Text += ",任务" + device.returnTaskId;
                            lbDeviceStatus.Text += ",方向" + device.returnTaskDrection;
                            lbDeviceStatus.Text += ",状态" + device.returnTaskStatus;
                            //lbDeviceStatus.Text += "," + DataBaseInterface.GetProductName(device.returnTaskId.Substring(0, device.returnTaskId.Length - 1));
                            break;
                        case "DYJ":   //叠烟机
                            lbDeviceStatus.Text += ",任务" + device.returnTaskId;
                            lbDeviceStatus.Text += ",目的" + device.returnToChannel;
                            lbDeviceStatus.Text += ",任务数" + device.returnTaskNum.ToString();
                            lbDeviceStatus.Text += ",完成数" + device.returnFinishedNum.ToString();
                            lbDeviceStatus.Text += ",状态" + device.returnTaskStatus;
                            break;
                    }
                }
                if (device.deviceId.Substring(0, 2) == "SM")
                {
                    if (device.barCodeStatus == false)
                        lbDeviceStatus.Text += "," + device.barCodeInfo;
                }
            }
        }
       
        
        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSendCommand_Click(object sender, EventArgs e)
        {
            device.ManulCommand(strCommand);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

      
    }
}
