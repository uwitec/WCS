using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using WCS;

namespace WCS.DevSystem
{
    /// <summary>
    /// 辅助设备类：连接线、无任务的输送线、作业台等
    /// </summary>
    public class DeviceSubLine
    {
        #region 自定义变量
        /// <summary>
        /// 设备信息，设备编码，设备名称
        /// </summary>
        public string deviceId, deviceName, devicePos, showInf, deviceType;

        /// <summary>
        /// 设备的位置和尺寸
        /// </summary>
        public Int32 x_start, y_start, x_end, y_end, xInc, yInc;

        /// <summary>
        /// 主窗体
        /// </summary>
        public New_Main_Form mainFrm;
        /// <summary>
        /// 画设备的图片
        /// </summary>
        //public PictureBox pic = new PictureBox();
        public Label pic = new Label();

        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化密集备货设备
        /// </summary>
        /// <param name="mainFrm">主窗体</param>
        /// <param name="row">一行设备信息</param>
        public DeviceSubLine(New_Main_Form mainFrm, DataRow row, Graphics g)
        {
            this.mainFrm = mainFrm;

            deviceId = row["device_id"].ToString();

            deviceName = row["device_name"].ToString();
            deviceType = row["device_type"].ToString();//1直线，2矩形

            x_start = Convert.ToInt32("0" + row["x_start"].ToString()); //字符串前面+“0”是为了字符串为空时，转为数字0
            y_start = Convert.ToInt32("0" + row["y_start"].ToString());
            x_end = Convert.ToInt32("0" + row["x_end"].ToString()); //字符串前面+“0”是为了字符串为空时，转为数字0
            y_end = Convert.ToInt32("0" + row["y_end"].ToString());

            xInc = Convert.ToInt32("0" + row["x_inc"].ToString());
            yInc = Convert.ToInt32("0" + row["y_inc"].ToString());

            devicePos = row["device_pos"].ToString();
            showInf = row["show_inf"].ToString();
            if (deviceType == "1")
            {
                g.DrawLine(new Pen(Color.Lime), x_start, y_start, x_end, y_end);
            }
            else
            {
                pic.Location = new Point(x_start, y_start);
                pic.Size = new Size(xInc, yInc);
                pic.BackColor = Color.LightGray;
                pic.BorderStyle = BorderStyle.FixedSingle;
                pic.Name = deviceName;
                pic.AutoSize = false;
                pic.Text = showInf;
                pic.TextAlign = ContentAlignment.MiddleCenter;
            }

        }
        #endregion

    }
}
