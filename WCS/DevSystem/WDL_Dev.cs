using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace WCS.DevSystem
{
    public class WDL_Dev
    {
        public New_Main_Form mainFrm;
        public String[] deviceId; //设备编号
        public String[] deviceName; //设备名称
        public String[] devicePos; //所在TABPAGE
        public String[] showInf; //显示信息
        public string[] commandType;

        public int[] xCoord;             //图形的坐标
        public int[] yCoord;
        public int[] xInc;
        public int[] yInc;
        public Label[] pic;

        public WDL_Dev(New_Main_Form mainfrom, DataTable bt)
        {
            this.mainFrm = mainfrom;
            deviceId = new String[bt.Rows.Count];
            deviceName = new String[bt.Rows.Count];
            devicePos = new String[bt.Rows.Count];
            showInf = new String[bt.Rows.Count];
            commandType = new String[bt.Rows.Count];

            xCoord = new int[bt.Rows.Count];
            yCoord = new int[bt.Rows.Count];
            xInc = new int[bt.Rows.Count];
            yInc = new int[bt.Rows.Count];

            pic = new Label[bt.Rows.Count];

            int i = 0;
            foreach (DataRow row in bt.Rows)
            {
                deviceId[i] = row["device_id"].ToString();
                deviceName[i] = row["device_name"].ToString();
                devicePos[i] = row["device_pos"].ToString();
                showInf[i] = row["show_inf"].ToString();
                commandType[i] = row["command_type"].ToString();

                xCoord[i] = Convert.ToInt32("0" + row["x_coord"].ToString());
                yCoord[i] = Convert.ToInt32("0" + row["y_coord"].ToString());
                xInc[i] = Convert.ToInt32("0" + row["x_inc"].ToString());
                yInc[i] = Convert.ToInt32("0" + row["y_inc"].ToString());

                pic[i] = new Label();
                pic[i].Size = new Size(xInc[i], yInc[i]);
                pic[i].Location = new Point(xCoord[i], yCoord[i]);
                if (commandType[i] == "BZQ")
                {
                    pic[i].BackColor = Color.DeepSkyBlue;
                    pic[i].Font = new Font("宋体", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                }
                else
                {
                    pic[i].BackColor = Color.Gray;
                }                
                pic[i].BorderStyle = BorderStyle.FixedSingle;
                pic[i].Tag = i.ToString();
                pic[i].Name = row["device_name"].ToString();
                pic[i].AutoSize = false;
                pic[i].Text = showInf[i];
                pic[i].TextAlign = ContentAlignment.MiddleCenter;
                i++;
            }
        }


    }
}