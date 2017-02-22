using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WCS
{
    public partial class ForSetckk : Form
    {
        private New_Main_Form mainFrm;
        public ForSetckk(New_Main_Form mainFrm)
        {
            InitializeComponent();
            this.mainFrm = mainFrm;
             MySqlConnection dbConn1;
            dbConn1=new MySqlConnection(mainFrm.dbConnectionString);
            dbConn1.Open();
            string sqlStr = " select ckkid,ckkname,ckkstatus  from idx_ckk order by ckkid";
             DataSet ds =  MySqlHelper.ExecuteDataset(dbConn1, sqlStr);
             if (ds.Tables[0].Rows.Count > 0)
             { 
               int i = 0;
               for (; i < ds.Tables[0].Rows.Count; i++)
               { 
                       if((ds.Tables[0].Rows[i])["ckkid"].ToString()=="1")
                        {
                            if ((ds.Tables[0].Rows[i])["ckkstatus"].ToString() == "1")
                            {
                                ckk1Status.Text = "可用";
                            }
                            else
                            {
                                ckk1Status.Text = "不可用";
                            }
                        }

                       if ((ds.Tables[0].Rows[i])["ckkid"].ToString() == "2")
                       {
                           if ((ds.Tables[0].Rows[i])["ckkstatus"].ToString() == "1")
                           {
                               ckk2Status.Text = "可用";
                           }
                           else
                           {
                               ckk2Status.Text = "不可用";
                           }
                       }


                       if ((ds.Tables[0].Rows[i])["ckkid"].ToString() == "3")
                       {
                           if ((ds.Tables[0].Rows[i])["ckkstatus"].ToString() == "1")
                           {
                               ckk3Status.Text = "可用";
                           }
                           else
                           {
                               ckk3Status.Text = "不可用";
                           }
                       }


                       if ((ds.Tables[0].Rows[i])["ckkid"].ToString() == "4")
                       {
                           if ((ds.Tables[0].Rows[i])["ckkstatus"].ToString() == "1")
                           {
                               ckk4Status.Text = "可用";
                           }
                           else
                           {
                               ckk4Status.Text = "不可用";
                           }
                       }
               }
             }
             dbConn1.Close();
        }

        private void btApp1_Click(object sender, EventArgs e)
        {
            MySqlConnection dbConn1;
            dbConn1 = new MySqlConnection(mainFrm.dbConnectionString);
            dbConn1.Open();
            string strSql = "update idx_ckk set ckkstatus=@status  where ckkid=1";


            MySqlParameter ptno = new MySqlParameter("@status", MySqlDbType.VarChar, 10);
            if( ckk1Status.Text == "可用")
               ptno.Value = "1";
            else
                ptno.Value = "0";
            MySqlCommand mc = new MySqlCommand(strSql, dbConn1);
          
            mc.Parameters.Add(ptno);
            mc.ExecuteNonQuery();
            dbConn1.Close();
        }

        private void btApp2_Click(object sender, EventArgs e)
        {
            MySqlConnection dbConn1;
            dbConn1 = new MySqlConnection(mainFrm.dbConnectionString);
            dbConn1.Open();
            string strSql = "update idx_ckk set ckkstatus=@status  where ckkid=2";


            MySqlParameter ptno = new MySqlParameter("@status", MySqlDbType.VarChar, 10);
            if (ckk2Status.Text == "可用")
                ptno.Value = "1";
            else
                ptno.Value = "0";
            MySqlCommand mc = new MySqlCommand(strSql, dbConn1);

            mc.Parameters.Add(ptno);
            mc.ExecuteNonQuery();
            dbConn1.Close();
        }

        private void btApp3_Click(object sender, EventArgs e)
        {
            MySqlConnection dbConn1;
            dbConn1 = new MySqlConnection(mainFrm.dbConnectionString);
            dbConn1.Open();
            string strSql = "update idx_ckk set ckkstatus=@status  where ckkid=3";


            MySqlParameter ptno = new MySqlParameter("@status", MySqlDbType.VarChar, 10);
            if (ckk3Status.Text == "可用")
                ptno.Value = "1";
            else
                ptno.Value = "0";
            MySqlCommand mc = new MySqlCommand(strSql, dbConn1);

            mc.Parameters.Add(ptno);
            mc.ExecuteNonQuery();
            dbConn1.Close();
        }

        private void btApp4_Click(object sender, EventArgs e)
        {
            MySqlConnection dbConn1;
            dbConn1 = new MySqlConnection(mainFrm.dbConnectionString);
            dbConn1.Open();
            string strSql = "update idx_ckk set ckkstatus=@status  where ckkid=4";


            MySqlParameter ptno = new MySqlParameter("@status", MySqlDbType.VarChar, 10);
            if (ckk4Status.Text == "可用")
                ptno.Value = "1";
            else
                ptno.Value = "0";
            MySqlCommand mc = new MySqlCommand(strSql, dbConn1);

            mc.Parameters.Add(ptno);
            mc.ExecuteNonQuery();
            dbConn1.Close();
        }
    }
}
