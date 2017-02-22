using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using WCS.PlcSystem;
using OpcRcw.Da;
using System.Runtime.InteropServices;

namespace WCS.DevSystem
{
    public class JT_Dev : IOPCDataCallback
    {
        public New_Main_Form mainFrm;
        public String[] deviceId; //设备编号
        public String[] deviceName; //设备名称
        public String[] devicePos; //所在TABPAGE
        public String[] showInf; //显示信息

        public OPCServerAsyn opcServer;  //OPCServer对象
        public string[] statusDB;
        public string[] statusType;
        public int[] statusHandle;

        public int[] xCoord;             //图形的坐标
        public int[] yCoord;
        public int[] xInc;
        public int[] yInc;

        public bool isBindToPLC = false;  //是否已初始化OPCServer
        public int[] deviceStatus; //deviceStatus为当前PLC返回的设备状态
        //public PictureBox[] pic;//显示设备图片
        public Label[] pic;

        public JT_Dev(New_Main_Form mainfrom, DataTable bt)
        {
            this.mainFrm = mainfrom;
            deviceId = new String[bt.Rows.Count];
            deviceName = new String[bt.Rows.Count];
            devicePos = new String[bt.Rows.Count];
            showInf = new String[bt.Rows.Count];

            //commandDB = new String[bt.Rows.Count];
            //returnDB = new String[bt.Rows.Count];
            //controlDB = new String[bt.Rows.Count];
            statusDB = new String[bt.Rows.Count];
            //failureDB = new String[bt.Rows.Count];

            //commandHandle = new int[bt.Rows.Count];
            //returnHandle = new int[bt.Rows.Count];
            //controlHandle = new int[bt.Rows.Count];
            statusHandle = new int[bt.Rows.Count];
            //failureHandle = new int[bt.Rows.Count];

            //commandType = new String[bt.Rows.Count];
            //controlType = new String[bt.Rows.Count];
            statusType = new String[bt.Rows.Count];
            //failureType = new String[bt.Rows.Count];
            //loadType = new String[bt.Rows.Count];

            deviceStatus = new int[bt.Rows.Count];
            xCoord = new int[bt.Rows.Count];
            yCoord = new int[bt.Rows.Count];
            xInc = new int[bt.Rows.Count];
            yInc = new int[bt.Rows.Count];

            pic = new Label[bt.Rows.Count];
            //readValues = new object[failureDB.Length];

            int i = 0;
            foreach (DataRow row in bt.Rows)
            {
                deviceId[i] = row["device_id"].ToString();
                deviceName[i] = row["device_name"].ToString();
                devicePos[i] = row["device_pos"].ToString();
                showInf[i] = row["show_inf"].ToString();

                //controlDB[i] = row["control_db"].ToString();
                statusDB[i] = row["status_db"].ToString();
                //failureDB[i] = row["failure_db"].ToString();

                xCoord[i] = Convert.ToInt32("0" + row["x_coord"].ToString());
                yCoord[i] = Convert.ToInt32("0" + row["y_coord"].ToString());
                xInc[i] = Convert.ToInt32("0" + row["x_inc"].ToString());
                yInc[i] = Convert.ToInt32("0" + row["y_inc"].ToString());
                //commandType[i] = row["command_type"].ToString();
                //controlType[i] = row["control_type"].ToString();
                statusType[i] = row["status_type"].ToString();
                //failureType[i] = row["failure_type"].ToString();

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
            FrmJT frmsmfl = new FrmJT(this, tag);
            frmsmfl.ShowDialog();
        }

        public void RefreshDisplay(int i)
        {
            //for (int i = 0; i < pic.Length; i++)
            //{
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
                    }
                }
            //}
        }

        public bool BindToPLC()
        {
            opcServer = new OPCServerAsyn();
            if (!opcServer.Connect()) return false;
            if (!opcServer.AddGroup(this)) return false;
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

                if (!opcServer.AddItems(statusitem, statusHandle)) return false;
            }
            //异步读
            opcServer.ReadPlc(statusHandle);
            //订阅
            if (!opcServer.DataChange()) return false;

            isBindToPLC = true;
            return isBindToPLC;
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
                mainFrm.AddErrToListView(String.Format("更新急停按钮状态失败:-{0}", error.Message));
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + String.Format("更新急停按钮状态失败:-{0}", error.Message)); 
                //MessageBox.Show(String.Format("更新急停按钮状态失败:-{0}", error.Message),
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
                mainFrm.AddErrToListView(String.Format("更新急停按钮状态失败:-{0}", error.Message));
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + String.Format("更新急停按钮状态失败:-{0}", error.Message));
                //MessageBox.Show(String.Format("更新急停按钮状态失败:-{0}", error.Message),
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
