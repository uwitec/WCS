using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WCS.DevSystem;
//using System.Data.OracleClient;
using MySql.Data.MySqlClient;
using WCS.Data;
using WCS.PlcSystem;
using System.IO.Ports;
using System.Configuration;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using WCS.Business;
using System.Runtime.InteropServices;
using System.Xml;
using System.Data.SqlClient;

namespace WCS
{
    public partial class New_Main_Form : Form
    {
        #region 定义变量
        //画连接线
        //Graphics g0;
        Graphics g1;
        //Graphics g;
        public MySqlConnection dbConn;

        public MySqlConnection dbConn1;

        //public SqlConnection dbConnWMS;
        //public string PortArray = string.Empty;//所有的复核分流和补货分流口
        public static string pickNoArray = string.Empty;//所有的扫描设备编号集合
        public bool dbIsConned = false;
        public bool plcIsOK = false;
        private Thread cmdDeviceThread;
        private Thread noCmdDeviceThread;
        private TaskAutoRun taskautorun;
        //public bool cmdDeviceThreadStatus = false; //false 线程没有启动或休眠状态，true 正在进行操作状态。
        public bool nonCmdDeviceThreadStatus = false; //false 线程没有启动或休眠状态，true 正在进行操作状态。
        public bool thrBarCodeScanIsRuning = false;
        /// <summary>
        /// false 线程没有启动或休眠状态，true 正在进行操作状态。
        /// </summary>
        public bool cmdDeviceThreadStatus = false;
        public bool deviceInited = false;
        public int workModel;
        //public bool IsRegistered = false;//系统是否注册成功
        //public long max_time = 0;
        //public long used_time = 0;
        //int li_Register_time = 0;
        /// <summary>
        /// 设备功能控制字典
        /// </summary>
        public Dictionary<string, DeviceBase.DeviceControl> deviceControlDic = new Dictionary<string, DeviceBase.DeviceControl>();
        /// <summary>
        /// 设备状态字典
        /// </summary>
        public Dictionary<string, DeviceBase.DeviceStatus> deviceStatusDic = new Dictionary<string, DeviceBase.DeviceStatus>();
        /// <summary>
        /// 可以下命令的设备
        /// </summary>
        public Dictionary<string, BESDevice> cmdDevices = new Dictionary<string, BESDevice>();

        /// <summary>
        /// 命令设备的OPCServer
        /// </summary>
        public OPCServer cmdDeviceOpcServer = new OPCServer();
        
        ///// <summary>
        ///// 设备故障字典
        ///// </summary>
        //public Dictionary<string, DeviceBase.DeviceFailure> deviceFailureDic = new Dictionary<string, DeviceBase.DeviceFailure>();

        public PlcSystemMS plcSystemMS;  //连接PLC时创建该类，主站类
        //public PlcSystemCS plcSystemCS;  //连接PLC时创建该类，从站类
        //public FHFL_Dev fhfl_dev;
        public static Thread ScannerThread = null;//扫描线程1
        public static Thread ScannerThread2 = null;//扫描线程1
        public SSJ_Dev ssj_dev;
        public JT_Dev jt_dev;
        public WDL_Dev wdl_dev;
        public KZAN_Dev kzan_dev;

        private SerialPort sp = new SerialPort();
        public BarCodeSocket barCodeSocket;
        public BarCodeSocket2 barCodeSocket2;

        public BarCodeSocket3 barCodeSocket3;

        delegate void AddContentToListViewCallback(string content, MsgType type, ListView lvInfo);
        delegate int MyScanMsgOperationLogicDelegate(string parameter);
        WcsDBOperation WCSDBobj;

       // public static Thread ScannerThread = null;//扫描线程1

        bool if_must_close = false;//是否必须关闭窗口
        //public string filename = Application.StartupPath + "\\" + "picking.xml";
        //public string dbConnectionString = "Server='10.21.19.210';UserId=root;Password=mysql;Database=sxjf_wcs;Allow User Variables=True";
      //  public string dbConnectionStringWms = "Server='192.168.0.2';UserId=nodes;Password=nodes;Database=wmsx;Allow User Variables=True";
        public string dbConnectionStringWms = "Server=localhost;UserId=root;Password=123321;Database=zshm_wcs;Allow User Variables=True";
      
        public string dbConnectionString = "Server=localhost;UserId=root;Password=123321;Database=zshm_wcs;Allow User Variables=True";
        DataSet ds_redo_Paint = new DataSet();//保存重新划线数据
        bool if_redo_Paint = false;//是否重新划线
        #endregion

        public New_Main_Form()
        {
            InitializeComponent();
            WCSDBobj = WcsDBOperation.GetInstance(this);
        }

        private void InitListView()
        {
            //位置
            //panelKQ.Location.X + panelKQ.Width +5
            listViewCommand.Location = new Point(640, 0);
            listViewCommand.Size = new Size(tabPage1.Width - 700, tabPage1.Height);

            listViewCommand.Columns.Add("时间", (int)(listViewCommand.Width * 0.17), HorizontalAlignment.Left);
            listViewCommand.Columns.Add("命令报文", (int)(listViewCommand.Width * 3.0), HorizontalAlignment.Left);
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            if (!dbIsConned)
            {
                dbIsConned = DBIsConnect();
            }
            if (dbIsConned)
            {
                InitialDev();
            }
            setMenu();
        }
        
        private bool DBIsConnect()
        {
            //1,连接数据库
            try
            {
                dbConn = new MySqlConnection(dbConnectionString);
                dbConn.Open();
                this.lblDBConStatus.Text = "数据库连接成功！";
                //DataBase.dbConn = dbConn;
                //DataBase.RemotedbConn = dbConnWMS;

                AddSuccessToListView("wcs数据库连接成功！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "wcs数据库连接成功");
            }
            catch (Exception ex)
            {
                AddErrToListView("wcs数据库连接异常！" + ex.Message.ToString());
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "wcs数据库连接异常！" + ex.Message.ToString());
                return false;
            }
            
            return true; 
        }

        private bool InitialDev()
        {
            DataSet ds_dev = new DataSet();
            #region 无动力输送线
            //ds_dev = MySqlHelper.ExecuteDataset(dbConn, "select * from td_device_dic t where t.command_type='XC' order by device_id");
            ds_dev = MySqlHelper.ExecuteDataset(dbConn, "select * from td_device_dic t where t.command_type='WDL' order by device_id");
            wdl_dev = new WDL_Dev(this, ds_dev.Tables[0]);
            for (int i = 0; i < ds_dev.Tables[0].Rows.Count; i++)
            {
                this.tabPage1.Controls.Add(wdl_dev.pic[i]);
                wdl_dev.pic[i].BringToFront();
            }
            #endregion

            #region 初始化急停按钮盒
            ds_dev = MySqlHelper.ExecuteDataset(dbConn, "select * from td_device_dic t where t.command_type='ANH' order by device_id");
            jt_dev = new JT_Dev(this, ds_dev.Tables[0]);
            for (int i = 0; i < ds_dev.Tables[0].Rows.Count; i++)
            {
                this.tabPage1.Controls.Add(jt_dev.pic[i]);
                jt_dev.pic[i].BringToFront();
            }
            #endregion

            #region 初始化无命令设备：输送机、分流器、移栽机、电柜
            ds_dev = MySqlHelper.ExecuteDataset(dbConn, "select * from td_device_dic t where t.command_type='' order by control_type,device_id");
            ssj_dev = new SSJ_Dev(this,ds_dev.Tables[0]);
            for (int i = 0; i < ds_dev.Tables[0].Rows.Count; i++)
            {
                this.tabPage1.Controls.Add(ssj_dev.pic[i]);
                ssj_dev.pic[i].BringToFront();
            }
            #endregion

            #region 初始化各个楼层的点动控制：各个楼层清错、手动自动、系统启动停止
            ds_dev = MySqlHelper.ExecuteDataset(dbConn, "select * from td_device_dic t where t.command_type='KZAN' order by device_id");
            kzan_dev = new KZAN_Dev(this, ds_dev.Tables[0]);
            for (int i = 0; i < ds_dev.Tables[0].Rows.Count; i++)
            {
                this.tabPage1.Controls.Add(kzan_dev.pic[i]);
                kzan_dev.pic[i].BringToFront();
            }
            #endregion

            #region 初始化辅助设备
            // 画线
            ds_redo_Paint = MySqlHelper.ExecuteDataset(dbConn,
                "select * from td_device_sub_line t where t.device_type='1' order by t.device_pos desc,t.device_id");
            if (ds_redo_Paint.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_redo_Paint.Tables[0].Rows)
                {
                    ////判断设备的位置
                    //switch (row["device_pos"].ToString())
                    //{
                    //    case "0":  //主界面
                    //        g = g0;
                    //        break;
                    //    case "1":  //tabPage1
                    //        g = g1;
                    //        break;
                    //    default:
                    //        g = g0;
                    //        break;
                    //}
                    DeviceSubLine device = new DeviceSubLine(this, row, g1);
                    Application.DoEvents();
                }
                if_redo_Paint = true;
            }
            #endregion

            #region 初始化设备字典
            //1初始化设备控制字典
            ds_dev = MySqlHelper.ExecuteDataset(dbConn,"select concat(Control_type,Control_id) 'key',Control_type,Control_id,Control_desc,t.condition " +
                                                          "from td_device_control_dic t order by Control_type,control_id");
            DeviceBase.DeviceControl tmpControl;
            foreach (DataRow row in ds_dev.Tables[0].Rows)
            {
                tmpControl.controlType = row["Control_type"].ToString();
                tmpControl.controlId = row["Control_id"].ToString();
                tmpControl.controlDesc = row["Control_desc"].ToString();
                tmpControl.condition = row["condition"].ToString();
                deviceControlDic.Add(row["key"].ToString(), tmpControl);
            }
            //2初始化设备状态字典
            ds_dev = MySqlHelper.ExecuteDataset(dbConn,
                "select concat(status_type,status_id),status_desc,status_kind,status_color from td_device_status_dic order by status_type,status_id");
            DeviceBase.DeviceStatus tmpStatus;
            foreach (DataRow row in ds_dev.Tables[0].Rows)
            {
                tmpStatus.statusDesc = row.ItemArray[1].ToString();
                tmpStatus.statusKind = row.ItemArray[2].ToString();
                tmpStatus.statusColor = row.ItemArray[3].ToString();
                deviceStatusDic.Add(row.ItemArray[0].ToString(), tmpStatus);
            }

            ////3初始化设备故障字典
            //ds_dev = MySqlHelper.ExecuteDataset(dbConn, "select concat(failure_type,failure_id),failure_desc,failure_kind "
            //    + " from td_device_failure_dic order by failure_type,failure_id");
            //DeviceBase.DeviceFailure tmpFailure;
            //foreach (DataRow row in ds_dev.Tables[0].Rows)
            //{
            //    tmpFailure.failureDesc = row.ItemArray[1].ToString();
            //    tmpFailure.failureKind = row.ItemArray[2].ToString();
            //    deviceFailureDic.Add(row.ItemArray[0].ToString(), tmpFailure);
            //}
            #endregion

            ds_dev.Dispose();
            return true;
        }

        #region 重新画线
        private void tabPage1_Paint(object sender, PaintEventArgs e)
        {
            if (if_redo_Paint == true)
            {
                redo_Paint("1");
            }
            
        }
        //重新画线
        private void redo_Paint(string ls_device_pos)
        {
            try
            {
                if (dbIsConned)
                {
                    //画线
                    if (ds_redo_Paint.Tables[0].Rows.Count > 0)
                    {
                        ////判断设备的位置
                        //switch (ls_device_pos)
                        //{
                        //    case "0":  //主界面
                        //        g = g0;
                        //        break;
                        //    case "1":  //tabPage1
                        //        g = g1;
                        //        break;
                        //    default:
                        //        g = g0;
                        //        break;
                        //}
                        foreach (DataRow row in ds_redo_Paint.Tables[0].Rows)
                        {
                            DeviceSubLine device = new DeviceSubLine(this, row, g1);
                            Application.DoEvents();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AddErrToListView("重新画线异常：" + ex.Message.ToString());
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "重新画线异常：" + ex.Message.ToString());
            } 
        }
        #endregion

        //刷新主窗口界面任务显示
        private void RefreshListView()
        {   

        }

        private void New_Main_Form_Load(object sender, EventArgs e)
        {
            InitListView();
            taskautorun = new TaskAutoRun(this);

            noCmdDeviceThread = new Thread(new ThreadStart(taskautorun.AutoRun));
            noCmdDeviceThread.IsBackground = true;
            if (!noCmdDeviceThread.IsAlive)
            {
                noCmdDeviceThread.Start();
            }

            cmdDeviceThread = new Thread(new ThreadStart(taskautorun.CmdDeviceAutoRun));
            if (!cmdDeviceThread.IsAlive)  //启动任务管理进程；
                cmdDeviceThread.Start();
            //画连接线
            //g0 = this.CreateGraphics();
            g1 = this.tabPage1.CreateGraphics();
            #region  初始化读码器
            barCodeSocket = new BarCodeSocket(this);
            barCodeSocket.OpenSocket("192.168.0.94", "2021");
            #endregion

            barCodeSocket2 = new BarCodeSocket2(this);
            barCodeSocket2.OpenSocket("192.168.0.93", "2022");


            barCodeSocket3 = new BarCodeSocket3(this);
            barCodeSocket3.OpenSocket("192.168.0.95", "1702");

            btnConn_Click(null,null);
            if (!dbIsConned) return;
            //// 删除历史数据
            //delete_history_data();

            btnConnPLC_Click(null, null);
            //timer1.Enabled = true;
            RefreshListView();


           
        }

        private void New_Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (if_must_close)
            {
                return;
            }
            DialogResult dialogResult = MessageBox.Show("是否要退出系统?", "系统提示",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {                
                taskautorun.closing = true;
                long ticks = DateTime.Now.Ticks / 1000000; //0.1s
                while (nonCmdDeviceThreadStatus) //等待线程在没有启动或休眠状态
                {
                    if ((DateTime.Now.Ticks / 1000000 - ticks) > 100) break; //如果等了10s线程还没有在休眠状态，强行退出；
                }
                try
                {
                    if (dbConn != null)
                        if (dbConn.State == ConnectionState.Open) dbConn.Close();
                }
                catch (Exception ex)
                {
                    return;
                }
                
            }
        }

        private void btnESC_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConnPLC_Click(object sender, EventArgs e)
        {
            Form frm = new Form();
            frm.Text = "正在连接PLC，请耐心等候......";
            frm.ClientSize = new System.Drawing.Size(292, 0);
            frm.ControlBox = false;
            frm.MaximizeBox = false;  //是否显示最大话按钮
            frm.MinimizeBox = false;//是否显示最小化按钮
            frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
            plcIsOK = PLCConnection();
            frm.Close();
            if (!plcIsOK)
            {
                this.lblPLCStatus.Text = "连接PLC失败！";
                AddErrToListView("连接PLC失败！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() +"连接PLC失败！");
            }
            else
            {
                this.lblPLCStatus.Text = "连接PLC成功！";
                AddSuccessToListView("连接PLC成功！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString()+"连接PLC成功！");
            }
            if (!plcIsOK) return;
            btnInitDev_Click(null,null);
            if (!deviceInited) return;
            btnStrat_Click(null, null);
            setMenu();
        }

        private void setMenu()
        {
            btnConn.Enabled = !dbIsConned;
            btnConnPLC.Enabled = !plcIsOK && dbIsConned;
            btnInitDev.Enabled = !deviceInited && plcIsOK && dbIsConned;
            btnStrat.Enabled = deviceInited && plcIsOK && dbIsConned && (workModel != 1);
            //btnStopWork.Enabled = dbIsConned ;

            //if (!IsRegistered)
            //{
            //    workModel = 0;
            //}
        }

        private bool PLCConnection() //非通用
        {
            //初始化PLCSystem类
            plcSystemMS = new PlcSystemMS(this); //只有一个主站
            if (!plcSystemMS.RefreshStatus()) return false;


            if (!cmdDeviceOpcServer.Connect()) return false;
            if (!cmdDeviceOpcServer.AddGroup()) return false;

            return true;
        }

        private void btnStrat_Click(object sender, EventArgs e)
        {
            try
            {
                if (dbIsConned && plcIsOK && deviceInited)
                {
                    if (ScannerThread == null)
                    {
                        ScannerThread = new Thread(new ThreadStart(barCodeSocket.Scanning));
                        ScannerThread.IsBackground = true;
                        if (!ScannerThread.IsAlive)
                        {
                            ScannerThread.Start();
                        }
                        barCodeSocket.WorkStatusRun = true;
                    }
                    if (ScannerThread2 == null)
                    {
                        ScannerThread2 = new Thread(new ThreadStart(barCodeSocket2.Scanning));
                        ScannerThread2.IsBackground = true;
                        if (!ScannerThread2.IsAlive)
                        {
                            ScannerThread2.Start();
                        }
                        barCodeSocket2.WorkStatusRun = true;
                    }
                    workModel = 1;
                    lblWorkStatus.Text = "开始联机工作！";
                    setMenu();
                }
            }
            catch (Exception ex)
            {
                AddErrToListView("联机工作异常！" + ex.Message.ToString());
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "联机工作异常！" + ex.Message.ToString());
                return;
            }

            if (workModel == 1)
            {
                AddSuccessToListView("开始联机工作！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "开始联机工作！");
            }

        }
        #region 扫描头线程开始
        internal void ReadResponseThread()
        {
                string strErr = string.Empty;
                try
                {
                    AddSuccessToListView("线程1信息读取线程开始工作");

                    for (; ; )
                    {
                        if (workModel == 1)
                        {
                            //System.Threading.Thread.Sleep(new System.TimeSpan(0, 0, 0, 0, 200));
                            string response = string.Empty;
                            string res = string.Empty;
                            lock (sp)
                            {
                                res = " ";
                                System.Text.StringBuilder b = new StringBuilder();
                                while (res.Length > 0)//comport
                                {
                                    //byte[] smsbuf = ss_port.Read(1);
                                    //res = Encoding.ASCII.GetString(smsbuf);
                                    //byte[] by_barcodes =new byte[20];
                                    //sp.Read(by_barcodes,0,12);
                                    //res = sp.ReadExisting();
                                    //res = sp.ReadTo("\r\n");//COM2
                                    res = sp.ReadTo("}");//COM2
                                    if (res.Length > 0)
                                    {
                                        b.Append(res);
                                        break;
                                    }
                                }
                                response = b.ToString();
                            }

                            if (response.Length > 0)
                            {
                                AddSuccessToListView("====================================================");
                                AddSuccessToListView("成功读取到扫描信息: " + response);
                                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "成功读到扫描信息:" + response);
                                //启动线委托线程操作
                                string[] msgArray = null;
                                DataOperation.ChangeScannerData2StrLst(response, ref msgArray);

                                if (msgArray != null)
                                {
                                    for (int i = 0; i < msgArray.Length; i++)
                                    {
                                        string tmpMsg = (string)msgArray[i];
                                        MyScanMsgOperationLogicDelegate myMethod = new MyScanMsgOperationLogicDelegate(this.ScanMsgOperationLogic);
                                        myMethod.BeginInvoke(tmpMsg, new AsyncCallback(AfterMyMothod3), null);
                                    }
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(new System.TimeSpan(0, 0, 0, 0, 30));
                    }
                    
                }
                catch (ThreadAbortException e)
                {
                    strErr = "读取信息ThreadAbortException:" + e.Message;
                    AddErrToListView(strErr);
                    LogHelper.LogSimlpleString(strErr); ;
                }
                catch (Exception ex)
                {
                    strErr = "读取信息出现异常:" + ex.Message;
                    AddErrToListView(strErr);
                    LogHelper.LogSimlpleString(strErr);
                }
        }

        #endregion

        public void AfterMyMothod3(IAsyncResult result)
        {
            AsyncResult async = (AsyncResult)result;
            MyScanMsgOperationLogicDelegate DelegateInstance = (MyScanMsgOperationLogicDelegate)async.AsyncDelegate;
        }

        #region 扫描处理方法
        public int ScanMsgOperationLogic(string strScanMsg)
        {
            //int result = -1;
            string strErrorMsg = string.Empty;
            string PLCreturnID = string.Empty;
            string PLCcommandText = string.Empty;
            if (strScanMsg.Length <= 4)
            {
                return 0;
            }
            try
            {
                //解析扫描结果
                string strScannerId = strScanMsg.Substring(1, 2);
                string strScannerCode = strScanMsg.Substring(3);          
            }
            catch (Exception ex)
            {
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G")+"处理扫描信息" + strScanMsg + "出现异常" + ex.Message);
                AddErrToListView("处理扫描信息" + strScanMsg + "出现异常" + ex.Message);
            }
            return 0;
        }
        #endregion

        public void AddSuccessToListView(String content)
        {
            AddToListView(content, MsgType.Infomation, listViewCommand);
        }

        public void AddErrToListView(string content)
        {
            AddToListView(content, MsgType.Error, listViewCommand);
        }
        #region 添加信息到ListView方法
        public void AddToListView(string content, MsgType type, ListView lvInfo)
        {
            if (lvInfo.InvokeRequired)
            {
                AddContentToListViewCallback d = new AddContentToListViewCallback(AddToListView);
                this.Invoke(d, new object[] { content, type, lvInfo });
            }
            else
            {
                if (lvInfo.Items.Count > 2000)
                { lvInfo.Items.Clear(); }
                ListViewItem it = new ListViewItem();
                it.Text = DateTime.Now.ToString();
                if (type == MsgType.Error)
                {
                    it.ForeColor = Color.Red;
                }
                else
                {
                    it.ForeColor = Color.Black;
                }
                it.SubItems.Add(content);
                lvInfo.Items.Insert(0, it);
            }
        }
        #endregion

        #region 信息枚举类型
        public enum MsgType
        {
            Infomation = 1, Error = 0
        }
        #endregion

        private void btnStopWork_Click(object sender, EventArgs e)
        {
            //workModel = 0;
            //setMenu();
            //RefreshListView();
        }

        private void btnInitDev_Click(object sender, EventArgs e)
        {
            //smfl_dev.BindToPLC(100);

            ssj_dev.BindToPLC();
            jt_dev.BindToPLC();
            kzan_dev.BindToPLC();
            //////////////////

            DataSet ds_dev = new DataSet();
            //初始化命令设备
            ds_dev = MySqlHelper.ExecuteDataset(dbConn, "select * from td_device_dic where command_db like 'DB%' and device_id like '1%' order by device_id");

            if (ds_dev.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_dev.Tables[0].Rows)
                {
                    BESDevice device = new BESDevice(this, row, cmdDeviceOpcServer);
                    cmdDevices.Add(row.ItemArray[0].ToString(), device);//将设备添加到设备字典中
                    // frm.Text = string.Format("正在初始化设备{0}接口，请耐心等候......", device.deviceName);
                    //临时注销
                    device.BindToPLC();
                    Thread.Sleep(600);
                    this.tabPage1.Controls.Add(device.pic);
                    device.pic.BringToFront();
                    Application.DoEvents();
                }
            }
         

            deviceInited = true;
            if (!deviceInited)
            {
                AddErrToListView("初始化设备失败！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "初始化设备失败！");
            }
            else
            {
                AddSuccessToListView("初始化设备成功！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "初始化设备成功！");
            }
            setMenu();
        }

    }
}
