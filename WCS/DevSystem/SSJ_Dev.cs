using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using WCS.PlcSystem;
using OpcRcw.Da;
using System.Runtime.InteropServices;

namespace WCS.DevSystem
{
    public class SSJ_Dev : DeviceBase, IOPCDataCallback
    {
        public New_Main_Form mainFrm;
        public OPCServerAsyn opcServerAsyn;  //OPCServer对象
        //private object[] readValues;
        //public int tag;//用来保存设备属性的索引
        public int li_ms_index = 0;//MS
        public SSJ_Dev(New_Main_Form mainfrom, DataTable bt)
        {
            this.mainFrm = mainfrom;
            deviceId = new String[bt.Rows.Count];
            deviceName = new String[bt.Rows.Count];
            devicePos = new String[bt.Rows.Count];
            showInf = new String[bt.Rows.Count];

            //commandDB = new String[bt.Rows.Count];
            //returnDB = new String[bt.Rows.Count];
            controlDB = new String[bt.Rows.Count];
            statusDB = new String[bt.Rows.Count];
            //failureDB = new String[bt.Rows.Count];

            //commandHandle = new int[bt.Rows.Count];
            //returnHandle = new int[bt.Rows.Count];
            controlHandle = new int[bt.Rows.Count];
            statusHandle = new int[bt.Rows.Count];
            //failureHandle = new int[bt.Rows.Count];

            //commandType = new String[bt.Rows.Count];
            controlType = new String[bt.Rows.Count];
            statusType = new String[bt.Rows.Count];
            //failureType = new String[bt.Rows.Count];
            //loadType = new String[bt.Rows.Count];

            deviceStatus = new int[bt.Rows.Count];
            //deviceFailure = new int[bt.Rows.Count];

            xCoord = new int[bt.Rows.Count];
            yCoord = new int[bt.Rows.Count];
            xInc = new int[bt.Rows.Count];
            yInc = new int[bt.Rows.Count];

            //kqId = new String[bt.Rows.Count];
            //partId = new String[bt.Rows.Count];
            pic = new Label[bt.Rows.Count];
            //readValues = new object[failureDB.Length];

            int i = 0;
            foreach (DataRow row in bt.Rows)
            {
                deviceId[i] = row["device_id"].ToString();
                deviceName[i] = row["device_name"].ToString();
                devicePos[i] = row["device_pos"].ToString();
                showInf[i] = row["show_inf"].ToString();

                controlDB[i] = row["control_db"].ToString();
                statusDB[i] = row["status_db"].ToString();
                //failureDB[i] = row["failure_db"].ToString(); 

                xCoord[i] = Convert.ToInt32("0" + row["x_coord"].ToString());
                yCoord[i] = Convert.ToInt32("0" + row["y_coord"].ToString());
                xInc[i] = Convert.ToInt32("0" + row["x_inc"].ToString());
                yInc[i] = Convert.ToInt32("0" + row["y_inc"].ToString());
                //commandType[i] = row["command_type"].ToString();
                controlType[i] = row["control_type"].ToString();
                statusType[i] = row["status_type"].ToString();
                //failureType[i] = row["failure_type"].ToString();
                if (statusType[i] == "MS")
                {
                    li_ms_index = i;
                }
                //kqId[i] = row["kq_id"].ToString();
                //partId[i] = row["system_part"].ToString();

                pic[i] = new Label();
                pic[i].Size = new Size(xInc[i], yInc[i]);
                pic[i].Location = new Point(xCoord[i], yCoord[i]);
                pic[i].BackColor = Color.DeepSkyBlue;
                pic[i].BorderStyle = BorderStyle.FixedSingle;
                pic[i].Tag = i.ToString();
                pic[i].Name = row["device_name"].ToString();
                pic[i].AutoSize = false;
                pic[i].Text = showInf[i];
                pic[i].TextAlign = ContentAlignment.MiddleCenter;
                pic[i].DoubleClick += new System.EventHandler(this.pic_DoubleClick);
                i++;
            }
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public void pic_DoubleClick(object sender, EventArgs e)
        {
            int tag;//用来保存设备属性的索引
            tag = Convert.ToInt32((sender as Label).Tag.ToString());
            //RefreshDisplayFailure(tag);
            FrmSSJ frmsmfl = new FrmSSJ(this, tag);
            frmsmfl.ShowDialog();
        }

        public void RefreshDisplay()
        {
            //for (int i = 0; i < pic.Length; i++)
            //{
            if (mainFrm.plcSystemMS.plcStatus == false)//plc主柜
            {
                pic[li_ms_index].BackColor = Color.Black;
            }
            else
            {
                if (mainFrm.deviceStatusDic.ContainsKey(statusType[li_ms_index] + deviceStatus[li_ms_index].ToString()) == true)
                {
                    switch (mainFrm.deviceStatusDic[statusType[li_ms_index] + deviceStatus[li_ms_index].ToString()].statusColor)
                    {
                        case "深绿色":
                            pic[li_ms_index].BackColor = Color.DarkGreen;
                            break;
                        case "红色":
                            pic[li_ms_index].BackColor = Color.Red;
                            break;
                        case "橙色":
                            pic[li_ms_index].BackColor = Color.Orange;
                            break;
                        case "浅绿色":
                            pic[li_ms_index].BackColor = Color.Lime;
                            break;
                        case "灰色":
                            pic[li_ms_index].BackColor = Color.Gray;
                            break;
                        case "蓝色":
                            pic[li_ms_index].BackColor = Color.Blue;
                            break;
                        case "黄色":
                            pic[li_ms_index].BackColor = Color.Yellow;
                            break;
                        case "青绿色":
                            pic[li_ms_index].BackColor = Color.Aqua;
                            break;
                    }
                }
            }
                
            //}
        }

        public void RefreshDisplay(int i)
        {
            if (statusType[i] == "MS" && mainFrm.plcSystemMS.plcStatus == false)//plc主柜
            {
                pic[i].BackColor = Color.Black;
            }
            else
            {
                if (mainFrm.deviceStatusDic.ContainsKey(statusType[i] + deviceStatus[i].ToString()) == true)
                {
                    switch (mainFrm.deviceStatusDic[statusType[i] + deviceStatus[i].ToString()].statusColor)
                    {
                        case "深绿色":
                            pic[i].BackColor = Color.DarkGreen;
                            break;
                        case "红色":
                            pic[i].BackColor = Color.Red;
                            break;
                        case "橙色":
                            pic[i].BackColor = Color.Orange;
                            break;
                        case "浅绿色":
                            pic[i].BackColor = Color.Lime;
                            break;
                        case "灰色":
                            pic[i].BackColor = Color.Gray;
                            break;
                        case "蓝色":
                            pic[i].BackColor = Color.Blue;
                            break;
                        case "黄色":
                            pic[i].BackColor = Color.Yellow;
                            break;
                        case "青绿色":
                            pic[li_ms_index].BackColor = Color.Aqua;
                            break;
                    }
                }
            }
        }

        // 手动命令
        public bool ManulCommand(int tag, string cmdId)
        {
            //停止状态	1
            //急停状态	2
            //故障状态	3
            //手动状态	4
            //功能停止	5
            //节能状态	6
            //运行状态	7
            switch (mainFrm.deviceControlDic[controlType[tag] + cmdId].condition)
            {
                case "0":  //只有在手动状态才能下命令
                    if (mainFrm.plcSystemMS.is_auto) 
                        MessageBox.Show("不满足指令条件,必须工作在手动模式！");
                    else
                        ManualCommandWrite(tag, cmdId);
                    break;
                case "1":  //无条件的命令
                    ManualCommandWrite(tag, cmdId);
                    break;
                case "2":  //只有发生故障时，才能下的命令
                    ////验证条件
                    //if (deviceStatus[tag] == 3) //故障状态	3
                        ManualCommandWrite(tag, cmdId);
                    //else
                    //    MessageBox.Show("不满足指令条件，设备必须在故障状态！");
                        break;
                default:
                    MessageBox.Show("系统没有设置该指令的下发条件！");
                    break;
            }
            return true;
        }

        public void ManualCommandWrite(int tag, string cmdId)
        {
            object[] writeValues = new object[1];
            int[] handle = new int[1];
            writeValues[0] = cmdId;
            handle[0] = controlHandle[tag];
            opcServer.SyncWrite(writeValues, handle);

            //items[0] = DateTime.Now.ToString("HH:mm:ss");
            //items[1] = deviceId[tag] + "手动命令" + mainFrm.deviceControlDic[controlType[tag] + cmdId].controlDesc;
            //items[2] = writeValues[0].ToString();
            //mainFrm.listViewCommand.Items.Add(new ListViewItem(items)); 
        }

        public override bool BindToPLC()
        {
            //同步OPC
            opcServer = new OPCServer();
            if (!opcServer.Connect()) return false;
            if (!opcServer.AddGroup()) return false;
            int client = 1;

            if (controlDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] controlitem = new OPCITEMDEF[controlDB.Length];
                for (int i = 0; i < controlDB.Length; i++)
                {

                    controlitem[i].szAccessPath = "";
                    controlitem[i].bActive = 1;
                    controlitem[i].hClient = client;
                    controlitem[i].dwBlobSize = 1;
                    controlitem[i].pBlob = IntPtr.Zero;
                    controlitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    controlitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", controlDB[i]); //状态DB
                    client++;
                }
                if (!opcServer.AddItems(controlitem, controlHandle)) return false;
            }

            //if (failureDB != null)
            //{
            //    OpcRcw.Da.OPCITEMDEF[] failureitem = new OPCITEMDEF[failureDB.Length];
            //    for (int i = 0; i < failureDB.Length; i++)
            //    {
            //        failureitem[i].szAccessPath = "";
            //        failureitem[i].bActive = 1;
            //        failureitem[i].hClient = client;
            //        failureitem[i].dwBlobSize = 1;
            //        failureitem[i].pBlob = IntPtr.Zero;
            //        failureitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;

            //        failureitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", failureDB[i]); //状态DB
            //        client++;
            //    }

            //    if (!opcServer.AddItems(failureitem, failureHandle)) return false;
            //}

            //异步OPC
            if (!BindToPLCAsyn()) return false;

            isBindToPLC = true;
            return isBindToPLC;
        }

        //绑定异步OPC
        public bool BindToPLCAsyn()
        {
            opcServerAsyn = new OPCServerAsyn();
            if (!opcServerAsyn.Connect()) return false;
            if (!opcServerAsyn.AddGroup(this)) return false;
            int client = 1;

            if (statusDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] statusitem = new OPCITEMDEF[statusDB.Length];
                for (int i = 0; i < statusDB.Length; i++)
                {
                    statusitem[i].szAccessPath = "";
                    statusitem[i].bActive = 1;
                    statusitem[i].hClient = client;
                    statusitem[i].dwBlobSize = 1;
                    statusitem[i].pBlob = IntPtr.Zero;
                    statusitem[i].vtRequestedDataType = (int)System.Runtime.InteropServices.VarEnum.VT_BSTR;
                    statusitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", statusDB[i]); //状态DB
                    client++;
                }
                if (!opcServerAsyn.AddItems(statusitem, statusHandle)) return false;
            }
            //异步读
            opcServerAsyn.ReadPlc(statusHandle);
            //订阅
            if (!opcServerAsyn.DataChange()) return false;

            return true;
        }

        #region IOPCDataCallback 成员

        public void OnCancelComplete(int dwTransid, int hGroup)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;
            try
            {
                for (int i = 0; i < dwCount; i++)
                {
                    if (pErrors[i] == 0)
                    {
                        deviceStatus[phClientItems[i] - 1] = int.Parse(pvValues[i].ToString());
                        RefreshDisplay(phClientItems[i] - 1);
                    }

                }
            }
            catch (System.Exception error)
            {
                mainFrm.AddErrToListView(String.Format("更新输送设备状态失败:-{0}", error.Message));
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + String.Format("更新输送设备状态失败:-{0}", error.Message));
                //MessageBox.Show(String.Format("更新输送设备状态失败:-{0}", error.Message),
                //    "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;
            try
            {
                for (int i = 0; i < dwCount; i++)
                {
                    if (pErrors[i] == 0)
                    {
                        deviceStatus[phClientItems[i] - 1] = int.Parse(pvValues[i].ToString());
                        RefreshDisplay(phClientItems[i] - 1);
                    }

                }
            }
            catch (System.Exception error)
            {
                mainFrm.AddErrToListView(String.Format("更新输送设备状态失败:-{0}", error.Message));
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + String.Format("更新输送设备状态失败:-{0}", error.Message));
                //MessageBox.Show(String.Format("更新输送设备状态失败:-{0}", error.Message),
                //    "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
