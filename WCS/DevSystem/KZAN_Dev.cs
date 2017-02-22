using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WCS.PlcSystem;
using OpcRcw.Da;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace WCS.DevSystem
{
    public class KZAN_Dev
    {
        public String[] deviceId; //设备编号
        public String[] deviceName; //设备名称
        public String[] devicePos; //所在TABPAGE
        public String[] showInf; //显示信息

        public OPCServer opcServer = new OPCServer();  //OPCServer对象
        public string[] commandDB;
        public string[] returnDB;
        public int[] commandHandle;

        //public string[] device_status;   //设备状态
        public int[] xCoord;             //图形的坐标
        public int[] yCoord;
        public int[] xInc;
        public int[] yInc;

        public bool isBindToPLC = false;  //是否已初始化OPCServer
        //public String[] deviceStatus; //deviceStatus为当前PLC返回的设备状态
        public Button[] pic;
        
        public New_Main_Form mainFrm;
        public KZAN_Dev(New_Main_Form mainfrom, DataTable bt)
        {
            this.mainFrm = mainfrom;
            deviceId = new String[bt.Rows.Count];
            deviceName = new String[bt.Rows.Count];
            devicePos = new String[bt.Rows.Count];
            showInf = new String[bt.Rows.Count];

            commandDB = new String[bt.Rows.Count];
            returnDB = new String[bt.Rows.Count];
            commandHandle = new int[bt.Rows.Count];

            xCoord = new int[bt.Rows.Count];
            yCoord = new int[bt.Rows.Count];
            xInc = new int[bt.Rows.Count];
            yInc = new int[bt.Rows.Count];
            pic = new Button[bt.Rows.Count];
            int i = 0;
            foreach (DataRow row in bt.Rows)
            {
                deviceId[i] = row["device_id"].ToString();
                deviceName[i] = row["device_name"].ToString();
                devicePos[i] = row["device_pos"].ToString();
                showInf[i] = row["show_inf"].ToString();

                commandDB[i] = row["command_db"].ToString();
                returnDB[i] = row["return_db"].ToString();

                xCoord[i] = Convert.ToInt32("0" + row["x_coord"].ToString());
                yCoord[i] = Convert.ToInt32("0" + row["y_coord"].ToString());
                xInc[i] = Convert.ToInt32("0" + row["x_inc"].ToString());
                yInc[i] = Convert.ToInt32("0" + row["y_inc"].ToString());

                pic[i] = new Button();
                pic[i].Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                pic[i].Size = new Size(xInc[i], yInc[i]);
                pic[i].Location = new Point(xCoord[i], yCoord[i]);
                pic[i].BackColor = Color.Transparent;
                //pic[i].BorderStyle = BorderStyle.FixedSingle;
                pic[i].Tag = i.ToString();
                pic[i].Name = row["device_name"].ToString();
                pic[i].AutoSize = false;
                pic[i].Text = showInf[i];
                pic[i].TextAlign = ContentAlignment.MiddleCenter;
                pic[i].Click += new System.EventHandler(this.pic_Click);
                
                i++;
            }
        }

        public void pic_Click(object sender, EventArgs e)
        {
            int tag;//用来保存设备属性的索引
            tag = Convert.ToInt32((sender as Button).Tag.ToString());
            ManualCommandWrite(tag, returnDB[tag]);
        }

        // 手动命令
        public void ManualCommandWrite(int tag, string cmdId)
        {
            object[] writeValues = new object[1];
            int[] handle = new int[1];
            writeValues[0] = cmdId;
            handle[0] = commandHandle[tag];
            opcServer.SyncWrite(writeValues, handle);

            //items[0] = DateTime.Now.ToString("HH:mm:ss");
            //items[1] = deviceId[tag] + "手动命令" + mainFrm.deviceControlDic[controlType[tag] + cmdId].controlDesc;
            //items[2] = writeValues[0].ToString();
            //mainFrm.listViewCommand.Items.Add(new ListViewItem(items)); 
        }
        public bool BindToPLC()
        {
            opcServer = new OPCServer();
            if (!opcServer.Connect()) return false;
            if (!opcServer.AddGroup()) return false;
            int client = 1;
            if (commandDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] commandItem = new OPCITEMDEF[commandDB.Length];
                for (int i = 0; i < commandDB.Length; i++)
                {
                    commandItem[i].szAccessPath = "";
                    commandItem[i].bActive = 1;
                    commandItem[i].hClient = client;
                    commandItem[i].dwBlobSize = 1;
                    commandItem[i].pBlob = IntPtr.Zero;
                    commandItem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    commandItem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", commandDB[i]); 
                    client++;
                }
                if (!opcServer.AddItems(commandItem, commandHandle)) return false;
            }       
            isBindToPLC = true;
            return isBindToPLC;
        }
    }
}
