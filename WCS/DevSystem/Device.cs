using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using WCS.PlcSystem;
using System.Linq;
using OpcRcw.Da;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Collections.Generic;
namespace WCS.DevSystem
{


    /// <summary>
    /// 密集备货设备类
    /// </summary>
    public class BESDevice
    {
        #region 自定义变量
        /// <summary>
        /// 设备信息，设备编码，设备名称
        /// </summary>
        public string deviceId, deviceName;
        /// <summary>
        /// 设备所属库区
        /// </summary>
        public string kqId;

          int currTaskId=0;

          public int li_ms_index = 0;//MS
          /// <summary>
          /// 自动命令线程
          /// </summary>
          private Thread autoCmdThread;
        /// <summary>
        /// OPCServer
        /// </summary>
        private OPCServer opcServer;
        /// <summary>
        /// 有关PLC的数据块,命令,返回,控制,故障,载入
        /// </summary>
        public string commandDB, returnDB, controlDB, failureDB, loadDB;
        /// <summary>
        /// 自动命令类型，控制命令类型，状态类型，载入类型
        /// </summary>
        public string commandType, controlType, statusType, loadType;

        private int[] commandHandle = new int[1];
        private int[] returnHandle = new int[1];
        private int[] controlHandle = new int[1];
        private int[] failureHandle = new int[1];
        private int[] loadHandle = new int[1];

        /// <summary>
        /// 设备的位置和尺寸
        /// </summary>
        public Int32 xCoord, yCoord, xInc, yInc;
        /// <summary>
        /// deviceStatus为当前PLC返回的设备状态,"0"为正常状态
        /// </summary>
        public String deviceStatus;
        /// <summary>
        /// 所属程序模块
        /// </summary>
        private String partId;
        /// <summary>
        /// load块信息，任务ID，目的地址，状态
        /// </summary>
        public String taskId, toAddr, loadStatus;
        /// <summary>
        /// 从PLC读出的装载信息；件烟DB102：任务号，拆烟机编号，目的分拣线编号，货物类型 
        /// </summary>
        public String cyjId;
        /// <summary>
        /// 垛烟DB103：任务号，目的通道，货物类型， 垛烟的件烟数量
        /// </summary>
        public int jyNum;
        /// <summary>
        /// 密集通道的空位数量 DB104
        /// </summary>
        public int kongWeiNum;
        /// <summary>
        /// 子任务号1位数字，用于一个任务要分多次下的情况，主要是拆烟机，入库扫描相关
        /// </summary>
        public int subTaskId = 1;
        /// <summary>
        /// 密集通道的空位标志，-1初始情况，0为空位不够，1空位够
        /// </summary>
        public int kongWeiFlag = -1;
        /// <summary>
        /// 通道
        /// </summary>
        public int tongDao;
        /// <summary>
        /// 密集通道有空位时是否申请补货；1申请，0不申请
        /// </summary>
        public String isRequestIn;
        /// <summary>
        /// 前一个申请标志;
        /// </summary>
        public string lastIsRequestIn;
        /// <summary>
        /// return块的信息，返回的任务ID
        /// </summary>
        public String returnTaskId;

         
        /// <summary>
        /// 返回的任务数量
        /// </summary>
        public int returnTaskNum;
        /// <summary>
        /// 返回的完成数量
        /// </summary>
        public int returnFinishedNum = 0;
        /// <summary>
        /// 数据库中的完成数量
        /// </summary>
        public int DBFinishedNum = 0;
        /// <summary>
        /// 返回任务状态
        /// </summary>
        public String returnTaskStatus;
        /// <summary>
        /// 返回目的道
        /// </summary>
        public String returnToChannel;
        /// <summary>
        /// 返回分流路线，输送分流路向 1为分流，2为直行
        /// </summary>
        public String returnTaskDrection;
        /// <summary>
        /// 返回拆烟机编号
        /// </summary>
        public String returnCyjId;
        /// <summary>
        /// 返回目的分拣线
        /// </summary>
        public String returnToSortLine;
        /// <summary>
        /// 返回任务序号
        /// </summary>
        public string returnTaskSeq;
        /// <summary>
        /// 当前扫描到的条码；核对后赋值""
        /// </summary>
        public String currBarCode = "";
        public String currBarCode1 = "";
        public String currBarCode2 = "";
        public String currBarCode3 = "";
        public String currBarCode4 = "0";
        public String currBarCode5 = "";
        public String bill_id = "";
        /// <summary>
        /// 当前条码状态；true条码对，false条码错
        /// </summary>
        public bool barCodeStatus = true;
        /// <summary>
        /// 是否需要核对条码；true需要核对条码，false不需要核对条码
        /// </summary>
        public bool checkBarCode = false;
        /// <summary>
        /// 短条形码
        /// </summary>
        public string shortBarCode;
        /// <summary>
        /// 该设备对应条码扫描器的编号
        /// </summary>
        public string scannerNo = string.Empty;
        /// <summary>
        /// 当为true时，本次的卷烟不验证条码通过，用于条码错误时手工放行。
        /// </summary>
       // public bool mannulCross = false;
        /// <summary>
        /// 
        /// </summary>
        public string barCodeInfo = "";
        /// <summary>
        /// 主窗体
        /// </summary>
        public New_Main_Form mainFrm;
        /// <summary>
        /// 画设备的图片
        /// </summary>
        public PictureBox pic = new PictureBox();
        /// <summary>
        /// 从OPCServer中读出的数据
        /// </summary>
        private String[] readValues = new String[1];
        /// <summary>
        /// 向OPCServer中写的数据
        /// </summary>
        private String[] writeValues = new String[1];
        /// <summary>
        /// 是否已初始化OPCServer
        /// </summary>
        private bool isBindToPLC = false;
        /// <summary>
        /// 保存上次刷新的时间，当两次刷新时间间隔较小（小于100ms）时，不进行刷新。
        /// </summary>
        private Int64 lastRefreshTicks;
        /// <summary>
        /// 当设备为拆烟机时，还需要画拆烟机外的辊道图片
        /// </summary>
        public PictureBox pic1 = new PictureBox();
        public Label lbCyj = new Label();
        /// <summary>
        /// 辊道的位置和尺寸
        /// </summary>
        public Int32 xCoord1, yCoord1, xInc1, yInc1;


        /// <summary>
        /// 返回item句柄
        /// </summary>
        private int[] returnItemHandle = new int[1];
        /// <summary>
        /// 从OPCServer中读出的数据
        /// </summary>
        private String[] readValuesSJ00 = new String[1];
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化密集备货设备
        /// </summary>
        /// <param name="mainFrm">主窗体</param>
        /// <param name="row">一行设备信息</param>
        public BESDevice(New_Main_Form mainFrm, DataRow row, OPCServer opcServer)
        {
            this.mainFrm = mainFrm;
            this.opcServer = opcServer;
            deviceId = row["device_id"].ToString();
            commandDB = row["command_db"].ToString();
            returnDB = row["return_db"].ToString();
            controlDB = row["control_db"].ToString();
            failureDB = row["status_db"].ToString();
            loadDB = row["load_db"].ToString();
            deviceName = row["device_name"].ToString();
            commandType = row["command_type"].ToString();
            controlType = row["control_type"].ToString();
            statusType = row["status_type"].ToString();
            loadType = row["load_type"].ToString();
            xCoord = Convert.ToInt32("0" + row["x_coord"].ToString()); //字符串前面+“0”是为了字符串为空时，转为数字0
            yCoord = Convert.ToInt32("0" + row["y_coord"].ToString());
            xInc = Convert.ToInt32("0" + row["x_inc"].ToString());
            yInc = Convert.ToInt32("0" + row["y_inc"].ToString());
            kqId = row["kq_id"].ToString();
            partId = row["system_part"].ToString();
            scannerNo = row["scanner_no"].ToString();
            if (row["is_check_barcode"].ToString() == "1")
                checkBarCode = true;
            else
                checkBarCode = false;

            pic.Location = new Point(xCoord, yCoord);
            pic.Size = new Size(xInc, yInc);
            pic.BackColor = Color.LimeGreen;
            pic.Name = deviceName;
            // 添加图片双击事件
            pic.DoubleClick += new System.EventHandler(this.pic_DoubleClick);
           
          
            lastRefreshTicks = DateTime.Now.Ticks - 100000000;


            //autoCmdThread = new Thread(new ThreadStart(AutoCmd));
            //autoCmdThread.IsBackground = true;
            //if (!autoCmdThread.IsAlive)
            //    autoCmdThread.Start();
        }
        #endregion

        private void AutoCmd()
        {
           
                mainFrm.cmdDeviceThreadStatus = true;
                if (mainFrm.dbIsConned)  //连接数据库后，才有可能刷新状态数据和显示
                {
                  RefreshStatus();
                  RefreshDisplay();
                }
                //处理条码扫描
                //if (mainform.deviceInited)
                //{
                //    mainform.barCodeScan.Scanning();
                //}
                mainFrm.cmdDeviceThreadStatus = false;
                //联机工作状态，任务处理
                #region 联机运行
                if (mainFrm.dbIsConned && mainFrm.deviceInited && mainFrm.plcIsOK && mainFrm.workModel == 1)
                {
                    mainFrm.cmdDeviceThreadStatus = true;




                    #region 扫描命令设备
                    SmflTaskPress();
                    #endregion
                    mainFrm.cmdDeviceThreadStatus = false;
                }
                #endregion
                Thread.Sleep(50);
           
        }

      
        /// <summary>
        /// 图片的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void pic_DoubleClick(object sender, EventArgs e)
        {
            FrmDevice frmDevice = new FrmDevice(this);
            // 测试用，弹出该设备ID+设备名称，正式使用去掉
           // MessageBox.Show(this.deviceId + this.deviceName);
            frmDevice.ShowDialog();

        }

        /// <summary>
        /// 更新设备状态
        /// </summary>
        /// <returns></returns>
        public bool RefreshStatus()
        {
            //try
            //{
            if ((DateTime.Now.Ticks - lastRefreshTicks) < 2000000)
                return true;

            if (!isBindToPLC)
                BindToPLC(); //如果没有初始化OPCServer，先初始化OPCServer

            if (failureDB.Length > 2)
            {
                if (!opcServer.SyncRead(readValues, failureHandle))  //读设备状态
                {
                    MessageBox.Show(string.Format("{0} {1} 读取状态信息失败\r\n{2}", this.deviceId, this.deviceName, this.failureDB));
                }
                deviceStatus = readValues[0];
            }
         
            if (returnDB.Length > 2)  //处理返回信息
            {
                string[] strDB;
                if (!opcServer.SyncRead(readValues, returnHandle))//;     //读设备返回信息
                {
                    MessageBox.Show(string.Format("{0} {1} 读取反馈信息失败\r\n{2}", this.deviceId, this.deviceName, this.returnDB));
                }
                else
                {
                    strDB = readValues[0].Substring(1, readValues[0].Length - 2).Split('|');
                    switch (commandType)
                    {
                      
                        case "SMFL":  //扫描核对分流
                            returnTaskId = strDB[0];
                            returnTaskDrection = strDB[1]; //输送分流路向 1为分流，2为直行
                            returnTaskStatus = strDB[4];
                          
                            break;
                    
                    }
                }
            }
            lastRefreshTicks = DateTime.Now.Ticks;
            return true;
            //}
            //catch (System.Exception error)
            //{
            //    MessageBox.Show(String.Format("更新输送机状态失败:-{0}", error.Message),
            //        "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}
        }

        public void RefreshDisplay()
        {
            //刷新设备的显示
            //if (failureDB.Length > 2 && loadDB.Length < 2) //只有错误信息的情况
            //{
                if (commandType == "SM" || commandType == "SMFL")
                {
                    if (deviceStatus == "0" && barCodeStatus == true)
                        pic.BackColor = Color.LimeGreen;
                    else
                        pic.BackColor = Color.Red;

                }
                else
                {
                    if (deviceStatus == "0")
                        pic.BackColor = Color.LimeGreen;
                    else
                        pic.BackColor = Color.Red;
                }
            //}
            //if (failureDB.Length > 2 && loadDB.Length >= 2) //既有错误信息，又有装载信息的情况
            //{
            //    if (loadType == "MJTD")
            //    {
            //        if (deviceStatus == "0")
            //            pic.BackColor = Color.Lime;
            //        else
            //            pic.BackColor = Color.Red;
            //    }
            //    if (loadType == "DY" || loadType == "JY")
            //    {
            //        if (deviceStatus == "0")
            //        {
            //            if (loadStatus == "0")
            //                pic.BackColor = Color.LimeGreen;
            //            else
            //                pic.BackColor = Color.Green;
            //        }
            //        else
            //        {
            //            if (loadStatus == "0")
            //                pic.BackColor = Color.Magenta;
            //            else
            //                pic.BackColor = Color.Red;
            //        }
            //        if (this.controlType == "CYJ")  //不通用
            //        {
            //            if (deviceStatus == "0")
            //            {
            //                if (loadStatus == "0")
            //                    pic1.BackColor = Color.LimeGreen;
            //                else
            //                    pic1.BackColor = Color.Green;
            //            }
            //            else
            //            {
            //                if (loadStatus == "0")
            //                    pic1.BackColor = Color.Magenta;
            //                else
            //                    pic1.BackColor = Color.Red;
            //            }
            //        }
            //    }
            //}



            if (mainFrm.plcSystemMS.plcStatus == false)//plc主柜
            {
                pic.BackColor = Color.Black;
            }
            else
            {
                if (mainFrm.deviceStatusDic.ContainsKey(statusType + deviceStatus.ToString()) == true)
                {
                    switch (mainFrm.deviceStatusDic[statusType + deviceStatus.ToString()].statusColor)
                    {
                        case "深绿色":
                            pic.BackColor = Color.DarkGreen;
                            break;
                        case "红色":
                            pic.BackColor = Color.Red;
                            break;
                        case "橙色":
                            pic.BackColor = Color.Orange;
                            break;
                        case "浅绿色":
                            pic.BackColor = Color.Lime;
                            break;
                        case "灰色":
                            pic.BackColor = Color.Gray;
                            break;
                        case "蓝色":
                            pic.BackColor = Color.Blue;
                            break;
                        case "黄色":
                            pic.BackColor = Color.Yellow;
                            break;
                        case "青绿色":
                            pic.BackColor = Color.Aqua;
                            break;
                    }
                }
            }


        }

        /// <summary>
        /// 手动命令
        /// </summary>
        /// <param name="cmdId">命令ID</param>
        /// <returns></returns>
        public bool ManulCommand(string cmdId)
        {
            if (!RefreshStatus())
                return false;
          
            switch (mainFrm.deviceControlDic[controlType + cmdId].condition)
            {
                case "0":  //只有在手动状态才能下命令
                    //验证条件,plc在运行状态，验证手动状态
                  
                        ManualCommandWrite(cmdId);
                    break;
                case "1":  //无条件的命令
                    ManualCommandWrite(cmdId);
                    break;
                case "2":  //只有发生故障时，才能下的命令
                    //验证条件
                    if (deviceStatus != "0") //0为正常状态
                        ManualCommandWrite(cmdId);
                    else
                        MessageBox.Show("不满足指令条件，设备必须在故障状态！");
                    break;
                default:
                    MessageBox.Show("系统没有设置该指令的下发条件！");
                    break;
            }
            return true;
        }
        /// <summary>
        /// 手动命令写入，并添加命令列表
        /// </summary>
        /// <param name="cmdId">命令ID</param>
        public void ManualCommandWrite(string cmdId)
        {
            writeValues[0] = cmdId;
            opcServer.SyncWrite(writeValues, controlHandle);
            string[] items = new string[3];
            items[0] = DateTime.Now.ToString("HH:mm:ss");
            items[1] = deviceId + "手动命令" + mainFrm.deviceControlDic[controlType + cmdId].controlDesc;
            items[2] = writeValues[0];
            mainFrm.listViewCommand.Items.Add(new ListViewItem(items));
        }
        /// <summary>
        /// 任务处理
        /// </summary>
        /// <param name="row">行任务数据</param>
        /// <returns></returns>
        public bool TaskPress()
        {
            //判断设备的类型
            switch (commandType)
            {
               
                case "SMFL":  //扫描核对分流
                    return SmflTaskPress();
               
                default:
                    MessageBox.Show("未知命令类型:" + commandType);
                    break;
            }
            return true;
        }
        
      
        /// <summary>
        /// 扫描核对
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool SmTaskPress(DataRow row)
        {
            // 如果当前的条码和数据库中的条码不一致，则封锁辊道
            if (checkBarCode == false || currBarCode == row["short_barcode"].ToString())
            {
                barCodeStatus = true;
                currBarCode = " ";
            }
            else
            {
                barCodeStatus = false;  //
                writeValues[0] = "05";   //不通用，封锁滚道
                opcServer.SyncWrite(writeValues, controlHandle);
                string[] items = new string[3];
                items[0] = DateTime.Now.ToString("HH:mm:ss");
                items[1] = deviceId + "自动命令，条码扫描封锁辊道";
                items[2] = writeValues[0];
                mainFrm.listViewCommand.Items.Add(new ListViewItem(items));
            }
            return true;
        }
        /// <summary>
        /// 扫描核对分流，需要对任务进行拆分
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool SmflTaskPress()
        {
            
            
            if (returnTaskStatus != "10")
                return true; //不在可写入新任务状态，直接返回

            DataSet ds_dev = new DataSet();
            if (scannerNo.Length < 2)
                return true;
            string bzwms = "";




            //初始化命令设备
              MySqlConnection dbConn1;
            dbConn1=new MySqlConnection(mainFrm.dbConnectionString);
            dbConn1.Open();

            MySqlConnection dbConnwms;
            dbConnwms = new MySqlConnection(mainFrm.dbConnectionStringWms);
            dbConnwms.Open();

            ds_dev = MySqlHelper.ExecuteDataset(dbConn1, "select BARCODE from td_wcs_barcode where SCNNER_NO ='" + scannerNo + "'");
            currBarCode = ds_dev.Tables[0].Rows[0][0].ToString();
            if (currBarCode.Length < 2)
            {
                dbConn1.Close();
                dbConnwms.Close();
                return true;
            }

           
            //调用
            string url = "http://192.168.0.3/wms/electronAutoController/getIdxConveyor?ctCode=" + currBarCode + "&lightNo=" + Convert.ToInt32(scannerNo);


          


            string strError = string.Empty;
            string  result = HttpHelper.GetHtml(url, out strError);
          
            zshmJson zshm = jsonHelp.DeserializeJsonToObject<zshmJson>(result);
            DataTable tblDatas = new DataTable("Datas");

            LogHelper.LogSimlpleString(DateTime.Now.ToString() + "-" + currBarCode + "-" + scannerNo+"-"+result);
      

            if (zshm.status.ToUpper() == "SUCCESS")
            {


                DataColumn dc = null;
                dc = tblDatas.Columns.Add("billId", Type.GetType("System.String"));
                dc = tblDatas.Columns.Add("chCode", Type.GetType("System.String"));
                dc = tblDatas.Columns.Add("ctCode", Type.GetType("System.String"));
                dc = tblDatas.Columns.Add("priority", Type.GetType("System.String"));

                for (int i = 0; i < zshm.result.Length; i++)
                {
                    DataRow newRow;
                    newRow = tblDatas.NewRow();
                    newRow["billId"] = zshm.result[i].billId.ToString().Trim();
                    newRow["chCode"] = zshm.result[i].chCode.ToString().Trim();
                    newRow["ctCode"] = zshm.result[i].ctCode.ToString().Trim();
                    newRow["priority"] = zshm.result[i].priority.ToString().Trim();

                    tblDatas.Rows.Add(newRow);
                }

            }
            currBarCode1="0";
            if (tblDatas.Rows.Count > 0)
            {
                for (int i = 0; i < tblDatas.Rows.Count; i++)
                {
                    if (tblDatas.Rows[i][1].ToString() == Convert.ToInt32(scannerNo).ToString())

                        currBarCode1 = "1";
                }
                currBarCode2 = tblDatas.Rows.Count.ToString();

                bill_id = tblDatas.Rows[0][0].ToString();
            }
            else
            {
                currBarCode2 = "0";
                bill_id = "0";
            }







           // ds_dev = MySqlHelper.ExecuteDataset(dbConnwms, "select count(0) num  from idx_conveyor where ct_code='" + currBarCode + "' and ch_code='" + Convert.ToInt32( scannerNo).ToString() + "' ");
          //  currBarCode1 = tblDatas


          

            //ds_dev = MySqlHelper.ExecuteDataset(dbConnwms, "select count(0) num ,IFNULL(max(bill_id),0) bill_id  from idx_conveyor where ct_code='" + currBarCode + "' ");
            //currBarCode2 = ds_dev.Tables[0].Rows[0][0].ToString();
            //bill_id = ds_dev.Tables[0].Rows[0][1].ToString();
          
            
            if (currBarCode1 != "0")
            {
                ds_dev = MySqlHelper.ExecuteDataset(dbConn1, "select count(0) num  from idx_conveyor_log where ct_code='" + currBarCode + "' and ch_code='" + Convert.ToInt32(scannerNo).ToString() + "' and bill_id='" + bill_id + "'");
                currBarCode4 = ds_dev.Tables[0].Rows[0][0].ToString();
            }



            string info = "1";
            if (currBarCode1 == "0")
                info = "1";
            else if (currBarCode2 == "1" || Convert.ToInt32(currBarCode4)>=2)
                info = "3";
            else
               info = "2";

            

            if (scannerNo == "21")
            {
                if (currBarCode2 != "0" || currBarCode=="NoRead")
                    info = "2";
                else
                    info = "1";
            }

            ds_dev = MySqlHelper.ExecuteDataset(dbConn1, "select BARCODE2 from td_wcs_barcode where SCNNER_NO ='22'");
            currBarCode3 = ds_dev.Tables[0].Rows[0][0].ToString();
            if (currBarCode3.Trim().Length < 1 && scannerNo == "22")
            {
                dbConn1.Close();
                dbConnwms.Close();
                return true;
            }
            string outResult = "-1";
            string outPort = "0";
            if (scannerNo == "22")
            {
                try
                {

                    string url1 = "http://192.168.0.3/wms/electronAutoController/saveAutoLineWeigh?ctCode=" + currBarCode + "&soWeight=" + Convert.ToDouble(currBarCode3) * 1000 ;


                    string strError1 = string.Empty;
                    string result1 = HttpHelper.GetHtml(url1, out strError1);

                    zshmCzJson zshmCz = jsonHelp.DeserializeJsonToObject<zshmCzJson>(result1);
                    DataTable tblDataTa = new DataTable("Datas");

                    LogHelper.LogSimlpleString(DateTime.Now.ToString() + "-" + currBarCode +"-"+ Convert.ToDouble(currBarCode3) * 1000 + "-"+ result1);


                    if (zshmCz.status.ToUpper() == "SUCCESS")
                    {

                        bzwms = "0";

                        DataColumn dc = null;
                        dc = tblDataTa.Columns.Add("outResult", Type.GetType("System.String"));
                        dc = tblDataTa.Columns.Add("outPort", Type.GetType("System.String"));

                        if (zshmCz.result.Length > 0)
                        {

                            for (int i = 0; i < 1; i++)
                            {
                                DataRow newRow;
                                newRow = tblDataTa.NewRow();
                                newRow["outResult"] = zshmCz.result[i].outResult.ToString().Trim();

                                if (newRow["outResult"].ToString() == "0")
                                    newRow["outPort"] = zshmCz.result[i].outPort.ToString().Trim();




                                tblDataTa.Rows.Add(newRow);
                            }



                            outResult = tblDataTa.Rows[0][0].ToString();
                            if (outResult == "0")
                                outPort = tblDataTa.Rows[0][1].ToString();
                            else
                            {

                                outPort = "5";
                                bzwms = "1";
                            }
                        }
                        else
                        {
                            outPort = "5";
                            bzwms = "1";
                        }



                    }
                    else
                    {
                        bzwms = "1";
                        outPort = "5";
                    
                    }

                    //MySqlCommand cmd = new MySqlCommand();
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "P_AUTO_ONLINE_WEIGHT";
                    //cmd.Parameters.Add("@V_CT_CODE", MySqlDbType.VarChar, 30);
                    ////注意输出参数要设置大小,否则size默认为0,
                    //cmd.Parameters.Add("@V_SO_WEIGHT", MySqlDbType.Int32, 11);
                    ////设置参数的类型为输出参数,默认情况下是输入,
                    //cmd.Parameters.Add("@V_RESULT", MySqlDbType.Int32, 11);
                    //cmd.Parameters["@V_RESULT"].Direction = ParameterDirection.Output;
                    ////为参数赋值
                    //cmd.Parameters["@V_CT_CODE"].Value = currBarCode;
                    //cmd.Parameters["@V_SO_WEIGHT"].Value = Convert.ToDouble(currBarCode3) * 1000;
                    //cmd.Connection = dbConnwms;
                    ////执行
                    //cmd.ExecuteNonQuery();
                    //bzwms = cmd.Parameters["@V_RESULT"].Value.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                    LogHelper.LogSimlpleString(DateTime.Now.ToString() + "-121" + ex.ToString());
                }
                


                if (bzwms == "1")
                    info = "5";
                else
                {
                    if (outPort == "0")
                    {

                        mainFrm.zckfp++;
                        if (mainFrm.zckfp > 4)
                            mainFrm.zckfp = 1;

                        ds_dev = MySqlHelper.ExecuteDataset(dbConn1, "select ckkstatus from idx_ckk where ckkid ='" + mainFrm.zckfp + "'");
                        currBarCode5 = ds_dev.Tables[0].Rows[0][0].ToString();

                        while (currBarCode5 != "1")
                        {
                            mainFrm.zckfp++;
                            if (mainFrm.zckfp > 4)
                                mainFrm.zckfp = 1;
                            ds_dev = MySqlHelper.ExecuteDataset(dbConn1, "select ckkstatus from idx_ckk where ckkid ='" + mainFrm.zckfp + "'");
                            currBarCode5 = ds_dev.Tables[0].Rows[0][0].ToString();

                        }



                        info = mainFrm.zckfp.ToString();
                    }
                    else
                    {
                        info = outPort;
                    }
                }
            }


            currTaskId = ( Convert.ToInt32( returnTaskId) *999)%10000;
            
            
            writeValues[0] = String.Format("{{{0}|{1}|{2}|0|0}}", currTaskId, info, 0, 0, 0);
              
            
            for (int i = 0; i < 3; i++)
                {
                    opcServer.SyncWrite(writeValues, commandHandle);
                    Thread.Sleep(120);
                    mainFrm.AddErrToListView(writeValues[0]+"-"+scannerNo);
                    if (scannerNo == "22")
                    {
                        LogHelper.LogSimlpleString(DateTime.Now.ToString() + writeValues[0] + "-" + scannerNo + "-" + currBarCode + "-" + currBarCode1 + "-" + currBarCode2 + "-" + bzwms + "-" + Convert.ToDouble(currBarCode3) * 1000);
                    }
                    else
                    {
                        LogHelper.LogSimlpleString(DateTime.Now.ToString() + writeValues[0] + "-" + scannerNo + "-" + currBarCode + "-" + currBarCode1 + "-" + currBarCode2);
             
                    }
                    RefreshStatus();
                    if (returnTaskId == currTaskId.ToString() && returnTaskStatus != "2") //写成功判断
                    {
                        //数据库任务处理

                        string strSql = "update td_wcs_barcode set BARCODE2='' , BARCODE  = @barcode where SCNNER_NO = @scnnerno";
                                    MySqlParameter pt=new MySqlParameter("@barcode",MySqlDbType.VarChar,30);  
                                    pt.Value="";

                                      MySqlParameter ptno=new MySqlParameter("@scnnerno",MySqlDbType.VarChar,10);  
                                      ptno.Value= scannerNo;
                                      MySqlCommand mc = new MySqlCommand(strSql, dbConn1);
                                    mc.Parameters.Add(pt);
                                    mc.Parameters.Add(ptno);
                                    mc.ExecuteNonQuery();


                                    //if (info == "2" && (scannerNo != "21" || scannerNo != "22"))
                                    //{
                                    //    strSql = "update idx_conveyor set status  =2 where ct_code= @barcode1 and ch_code = @scnnerno1";
                                    //    MySqlParameter pt1 = new MySqlParameter("@barcode1", MySqlDbType.VarChar, 30);
                                    //    pt1.Value = currBarCode;

                                    //    MySqlParameter ptno1 = new MySqlParameter("@scnnerno1", MySqlDbType.VarChar, 10);
                                    //    ptno1.Value = Convert.ToInt32( scannerNo).ToString();
                                    //    MySqlCommand mc1 = new MySqlCommand(strSql, dbConn1);
                                    //    mc1.Parameters.Add(pt1);
                                    //    mc1.Parameters.Add(ptno1);
                                    //    mc1.ExecuteNonQuery();
                                    //}


                                    mainFrm.AddErrToListView("update" + scannerNo);
                                    if (info != "1" || scannerNo == "22")
                                    {

                                        strSql = "insert into idx_conveyor_log(ct_code,ch_code,bill_id,status) values(  @barcode1 , @scnnerno1,@bill_id,@status)";
                                        MySqlParameter pt2 = new MySqlParameter("@barcode1", MySqlDbType.VarChar, 30);
                                        pt2.Value = currBarCode;

                                        MySqlParameter ptno2 = new MySqlParameter("@scnnerno1", MySqlDbType.VarChar, 10);
                                        ptno2.Value = Convert.ToInt32(scannerNo).ToString();

                                        MySqlParameter ptno3 = new MySqlParameter("@bill_id", MySqlDbType.VarChar, 50);
                                        ptno3.Value = bill_id.ToString();
                                        MySqlParameter ptno4 = new MySqlParameter("@status", MySqlDbType.VarChar, 10);
                                        ptno4.Value = info.ToString() + "-" + bzwms+"-"+currBarCode+"-"+currBarCode3;
                                        MySqlCommand mc2 = new MySqlCommand(strSql, dbConn1);
                                        mc2.Parameters.Add(pt2);
                                        mc2.Parameters.Add(ptno2);
                                        mc2.Parameters.Add(ptno3);
                                        mc2.Parameters.Add(ptno4);
                                        mc2.ExecuteNonQuery();
                                    }

                        subTaskId++;
                        currBarCode = " ";
                        barCodeStatus = true;
                        break;
                    }
                    
                }
                dbConn1.Close();
                dbConnwms.Close();


           // currBarCode = DataBaseInterface.GetCurrentBarCode(scannerNo);

            //if (checkBarCode == false || currBarCode == row["short_barcode"].ToString() || currBarCode == "NoRead") //条件满足写分流任务,SJ031G
            //{
              

            //    taskId = row["task_id"].ToString();
            //    subTaskId = subTaskId % 10;
            //    string currTaskId;
            //    if (taskId.Length > 8)
            //        currTaskId = taskId.Substring(taskId.Length - 8) + subTaskId.ToString();
            //    else
            //        currTaskId = taskId + subTaskId.ToString();

            //    string taskSeq = row["task_seq"].ToString();
            //    string commandInfo = row["command_info"].ToString();
            //    writeValues[0] = String.Format("{{{0}|{1}|{2}|0|0}}", currTaskId, commandInfo, 0);
            //    for (int i = 0; i < 3; i++)
            //    {
            //        opcServer.SyncWrite(writeValues, commandHandle);
            //        Thread.Sleep(110);
            //        RefreshStatus();
            //        if (returnTaskId == currTaskId && returnTaskStatus != "2") //写成功判断
            //        {
            //            //数据库任务处理
            //            DataBaseInterface.SmflTaskPress(taskSeq); //扫描分流任务处理
            //            DataBaseInterface.ClearCurrentBarCode(scannerNo); //清除条码
            //            subTaskId++;
            //            currBarCode = " ";
            //            barCodeStatus = true;
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    barCodeStatus = false;
            //    barCodeInfo = "正确为：" + row["short_barcode"].ToString() + "实际为：" + currBarCode;
            //    mainFrm.AddListSystemEvent(deviceName + row["TO_ADDR"].ToString() + barCodeInfo);
            //}
            return true;
        }
       

        /// <summary>
        /// 修改是否验证条码的标志
        /// </summary>
        /// <returns></returns>
        public bool SetBarCodeCheckFlag()
        {
            //string flag;
            //if (checkBarCode)
            //    flag = "1";
            //else
            //    flag = "0";
            //string strSql = "update td_device_dic set is_check_barcode  = '" + flag + "' where device_id = '" + deviceId + "'";
            //OracleHelper.ExecuteNonQuery(mainFrm.dbConn, CommandType.Text, strSql);
            return true;
        }
       
       
        /// <summary>
        /// 初始化，绑定PLC
        /// </summary>
        public void BindToPLC()
        {
            OpcRcw.Da.OPCITEMDEF[] Items = new OPCITEMDEF[1];
            Items[0].szAccessPath = "";
            Items[0].bActive = 1;
            Items[0].hClient = 1;
            Items[0].dwBlobSize = 1;
            Items[0].pBlob = IntPtr.Zero;
            Items[0].vtRequestedDataType = (int)VarEnum.VT_BSTR;

            if (commandDB.Length > 2)
            {
                Items[0].szItemID = string.Format("S7:[S7 connection_1]{0}", commandDB); //状态DB
                opcServer.AddItems(Items, commandHandle);
            }
            if (returnDB.Length > 2)
            {
                Items[0].szItemID = string.Format("S7:[S7 connection_1]{0}", returnDB); //状态DB
                opcServer.AddItems(Items, returnHandle);
            }
            if (controlDB.Length > 2)
            {
                Items[0].vtRequestedDataType = (int)VarEnum.VT_UI2;
                Items[0].szItemID = string.Format("S7:[S7 connection_1]{0}", controlDB); //状态DB
                opcServer.AddItems(Items, controlHandle);
            }
            if (failureDB.Length > 2)
            {
                Items[0].szItemID = string.Format("S7:[S7 connection_1]{0}", failureDB); //状态DB
                opcServer.AddItems(Items, failureHandle);
            }
            if (loadDB.Length > 2)
            {
                if (statusType == "MJTD")
                    Items[0].vtRequestedDataType = (int)VarEnum.VT_UI2;
                else
                    Items[0].vtRequestedDataType = (int)VarEnum.VT_BSTR;

                Items[0].szItemID = string.Format("S7:[S7 connection_1]{0}", loadDB); //状态DB
                opcServer.AddItems(Items, loadHandle);
            }
            isBindToPLC = true;
        }
    }

    #region 设备字典结构体定义
    /// <summary>
    /// 设备状态字典用
    /// </summary>
    public struct DeviceStatus  //设备状态字典用
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        public String statusDesc;
        /// <summary>
        /// 状态性质
        /// </summary>
        public String statusKind;
    }
    /// <summary>
    /// 设备控制(四个字符串类型成员变量)
    /// </summary>
    public struct DeviceControl
    {
        /// <summary>
        /// 控制类型
        /// </summary>
        public String controlType;
        /// <summary>
        /// 控制ID
        /// </summary>
        public String controlId;
        /// <summary>
        /// 控制名称
        /// </summary>
        public String controlDesc;
        /// <summary>
        /// 控制模式，0：手动模式下操作 1：任何模式下 2：故障模式下
        /// </summary>
        public String condition;
    }
    #endregion
}
