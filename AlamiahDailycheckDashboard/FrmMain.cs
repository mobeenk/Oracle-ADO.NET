using AlamiahDailycheckDashboard.Helpers;
using System.Timers;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using Excel = Microsoft.Office.Interop.Excel;

namespace AlamiahDailycheckDashboard
{
    public partial class FrmMain : Form
    {
       
     
        private bool inProcess = false;

        public List<int> r0_list = new List<int>();
        public List<int> r1_list = new List<int>();
        public List<int> r1_400_list = new List<int>();
        //jmsumra
        //devjms34
        public FrmMain()
        {

            InitializeComponent();
            generate_excelReport.Enabled = false;

            InitializeTimerJMS(); //task run schedudle 
            InitializeTimerGroups();
            InitializeTimerWebservice();

            total_mofa_label();

            checkLog.Text = " ";
            richTextBox1.Text = " ";

            this.FormClosed += Form1_FormClosed;
            indicatorLevel.ForeColor = Color.Green;


            ;

        }


        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string picPath = @"C:\DC\Screenshots\forcedClose.jpg";
            PrintScreen(picPath);
        }

        public void total_mofa_label()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Consts.connection))
                using (OracleCommand cmd = new OracleCommand(SQLQueries.total_mofa_query, conn))
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    DataRow[] dr = dataTable.Select();
                    //*******************************
                    //    DataColumn dataColumn = dr[0].Table.ColumnName;
                    string cellData = " ";
                    string average = " ";
                    int number = 0;
                    //string arrow_up = char.ConvertFromUtf32(0x2191) + char.ConvertFromUtf32(0x2191) + char.ConvertFromUtf32(0x2191);
                    //string arrow_down = char.ConvertFromUtf32(0x2193) + char.ConvertFromUtf32(0x2193) + char.ConvertFromUtf32(0x2193);
                    for (int i = 0; i < dataTable.Select().Length; i++) //loop rows
                    {
                        foreach (DataColumn c in dr[i].Table.Columns)  //loop through the columns. 
                        {
                            if (c.ColumnName.Equals("prev"))
                            {
                                cellData = dataTable.Rows[i][c.ColumnName].ToString();
                                l_mofa.Text = ConfigurationManager.AppSettings["label_pmofa"] + cellData;
                            }

                            if (c.ColumnName.Equals("curr"))
                            {
                                cellData = dataTable.Rows[i][c.ColumnName].ToString();
                                cur_mofa.Text = ConfigurationManager.AppSettings["label_cmofa"] + cellData;

                            }
                            if (c.ColumnName.Equals("growth"))
                            {
                                average = dataTable.Rows[i][c.ColumnName].ToString();
                                number = int.Parse(average);
                                if (number > 0)
                                {
                                    mofa_increase.Text = average + " زيادة ";
                                    mofa_increase.ForeColor = Color.MediumSeaGreen;
                                }
                                else if (number < 0)
                                {
                                    mofa_increase.Text = average + " نقص ";
                                    mofa_increase.ForeColor = Color.Red;
                                }
                                else if (number == 0)
                                {
                                    mofa_increase.Text = " Zero ";
                                    mofa_increase.ForeColor = Color.Gray;
                                }

                            }
                            if (c.ColumnName.Equals("per"))
                            {
                                cellData = dataTable.Rows[i][c.ColumnName].ToString();

                                if (number > 0)
                                {
                                    cellData += " + " + "نسبة الزيادة";
                                    perc.Text = cellData;
                                    perc.ForeColor = Color.Green;
                                }
                                else
                                {
                                    cellData += " - " + "نسبة الهبوط";
                                    perc.Text = cellData;
                                    perc.ForeColor = Color.Red;
                                }
                            }
                        }
                    }//for 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
        public void check_trx()
        {

         
            dg_trx.DataSource = DBHelper.connect_oracle(SQLQueries.trx_query);
            dg_trx.Columns[0].Width = 40; dg_trx.Columns[0].HeaderText = "CNT";
            dg_trx.Columns[1].Width = 40; dg_trx.Columns[1].HeaderText = "TYPE";
            dg_trx.Columns[2].Width = 40; dg_trx.Columns[2].HeaderText = "STATUS";
            dg_trx.Columns[3].Width = 70; dg_trx.Columns[3].HeaderText = "VALID";
            dg_trx.Columns[4].Width = 600;

            foreach (DataGridViewColumn c in dg_trx.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);
            }
        }
        public void max_functions()
        {
            dg_max.DataSource = DBHelper.connect_oracle(SQLQueries.max_query);
            dg_max.Columns[0].HeaderText = "MOFA";
            dg_max.Columns[1].HeaderText = "PAYMENT";
            dg_max.Columns[2].HeaderText = "VOUCHER";
            foreach (DataGridViewColumn c in dg_max.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);
            }

        }
        public void elm_deleted()
        {
            dg_elm_deleted.DataSource = DBHelper.connect_oracle(SQLQueries.max_query);
        }
        public void elm_status()
        {
            dg_elm.DataSource = null;

            try
            {
                using (OracleConnection conn = new OracleConnection(Consts.connection))
                using (OracleCommand cmd = new OracleCommand(SQLQueries.elm_status_query, conn))
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    DataRow[] dr = dataTable.Select();
                    //*******************************
                    dg_elm.AutoGenerateColumns = false;
                    //Set Columns Count
                    dg_elm.ColumnCount = 4;
                    //Add Columns
                    dg_elm.Columns[0].Name = "LAST_CREATE"; dg_elm.Columns[0].HeaderText = "LAST_CREATE"; dg_elm.Columns[0].DataPropertyName = "LAST_CREATE";
                    dg_elm.Columns[1].Name = "LAST_MODIFY"; dg_elm.Columns[1].HeaderText = "LAST_MODIFY"; dg_elm.Columns[1].DataPropertyName = "LAST_MODIFY";
                    dg_elm.Columns[2].Name = "CREATED"; dg_elm.Columns[2].HeaderText = "CREATED"; dg_elm.Columns[2].DataPropertyName = "CREATED";
                    dg_elm.Columns[3].Name = "MODIFIED"; dg_elm.Columns[3].HeaderText = "MODIFIED"; dg_elm.Columns[3].DataPropertyName = "MODIFIED";

                    dg_elm.DataSource = dataTable;
                    //    DataColumn dataColumn = dr[0].Table.ColumnName;
                    for (int i = 0; i < dataTable.Select().Length; i++) //loop rows
                    {
                        foreach (DataColumn c in dr[i].Table.Columns)  //loop through the columns. 
                        {
                            if (c.ColumnName.Equals("M_CREATED"))
                            {
                                string cellData = dataTable.Rows[i][c.ColumnName].ToString();
                                if (int.Parse(cellData) >= 0)
                                {
                                    int number; // = int.Parse(cellData);
                                    int.TryParse(cellData, out number);
                                    // number of minutes
                                    if (number >= 25 && number < 50)
                                    {
                                        dg_elm.Rows[i].Cells[2].Style.BackColor = Color.Yellow;
                                    }
                                    else if (number >= 50)
                                    {
                                        dg_elm.Rows[i].Cells[2].Style.BackColor = Color.Red;

                                    }
                                }
                            }
                            if (c.ColumnName.Equals("M_MODIFIED"))
                            {
                                string cellData = dataTable.Rows[i][c.ColumnName].ToString();
                                if (int.Parse(cellData) >= 0)
                                {
                                    //int number = int.Parse(cellData);
                                    int number; // = int.Parse(cellData);
                                    int.TryParse(cellData, out number);
                                    // number of minutes
                                    if (number >= 25 && number < 50)
                                    {
                                        dg_elm.Rows[i].Cells[3].Style.BackColor = Color.Yellow;
                                    }
                                    else if (number >= 50)
                                    {
                                        dg_elm.Rows[i].Cells[3].Style.BackColor = Color.Red;
                                    }
                                }
                            }

                        }
                    }//for 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (DataGridViewColumn c in dg_elm.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);
            }


        }
        public void groups_request0()
        {
            request0.DataSource = DBHelper.connect_oracle(SQLQueries.groups_request0_query);
        }
        public void groups10_400()
        {
            g10_400.DataSource = DBHelper.connect_oracle(SQLQueries.groups10_400_query);
        }//jms_out
        public void sms_check()
        {

         
            sms.DataSource = null;

            try
            {
                using (OracleConnection conn = new OracleConnection(Consts.connection))
                using (OracleCommand cmd = new OracleCommand(SQLQueries.sms_check_query, conn))
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    DataRow[] dr = dataTable.Select();
                    //*******************************
                    sms.AutoGenerateColumns = false;
                    //Set Columns Count
                    sms.ColumnCount = 3;
                    //Add Columns
                    sms.Columns[0].Name = "COUNT"; sms.Columns[0].HeaderText = "COUNT"; sms.Columns[0].DataPropertyName = "COUNT";
                    sms.Columns[1].Name = "GENERATED_SINCE"; sms.Columns[1].HeaderText = "GENERATED_SINCE"; sms.Columns[1].DataPropertyName = "GENERATED_SINCE";
                    sms.Columns[2].Name = "SJM_MSG"; sms.Columns[2].HeaderText = "SJM_MSG"; sms.Columns[2].DataPropertyName = "SJM_MSG";
                    sms.Columns[2].Width = 300;
                    sms.DataSource = dataTable;

                   // sms.Rows[0].Cells[0].Selected = false;
                    //    DataColumn dataColumn = dr[0].Table.ColumnName;
                    for (int i = 0; i < dataTable.Select().Length; i++) //loop rows
                    {
                        foreach (DataColumn c in dr[i].Table.Columns)  //loop through the columns. 
                        {
                            if (c.ColumnName.Equals("MINUTES"))
                            {
                                string cellData = dataTable.Rows[i][c.ColumnName].ToString();
                                {
                                    //int number = int.Parse(cellData);
                                    int number; // = int.Parse(cellData);
                                    int.TryParse(cellData, out number);
                                    // number of minutes
                                    if (number >= 5 && number < 10)
                                    {
                                        sms.Rows[i].Cells[1].Style.BackColor = Color.Yellow;
                                    }
                                    else if (number >= 10)
                                    {
                                        sms.Rows[i].Cells[1].Style.BackColor = Color.Red;

                                    }
                                }
                            }
                        }
                    }//for 


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (DataGridViewColumn c in mutamer_fees.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);
            }


        }//jms_out
        public void fees()
        {

     

            mutamer_fees.DataSource = null;
            try
            {
                using (OracleConnection conn = new OracleConnection(Consts.connection))
                using (OracleCommand cmd = new OracleCommand(SQLQueries.fees_query, conn))
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    DataRow[] dr = dataTable.Select();
                    //*******************************
                    mutamer_fees.AutoGenerateColumns = false;
                    //Set Columns Count
                    mutamer_fees.ColumnCount = 2;
                    //Add Columns
                    mutamer_fees.Columns[0].Name = "COUNT"; mutamer_fees.Columns[0].HeaderText = "COUNT"; mutamer_fees.Columns[0].DataPropertyName = "COUNT";                    
                    mutamer_fees.Columns[1].Name = "DELAY"; mutamer_fees.Columns[1].HeaderText = "DELAY"; mutamer_fees.Columns[1].DataPropertyName = "DELAY";


                    mutamer_fees.DataSource = dataTable;

                    //    DataColumn dataColumn = dr[0].Table.ColumnName;
                    for (int i = 0; i < dataTable.Select().Length; i++) //loop rows
                    {
                        foreach (DataColumn c in dr[i].Table.Columns)  //loop through the columns. 
                        {
                            string cellData = dataTable.Rows[i][c.ColumnName].ToString();
                            if (c.ColumnName.Equals("MINUTES"))
                            {

                                if (int.Parse(cellData) >= 0)
                                {
                                    //int number = int.Parse(cellData);
                                    int number; // = int.Parse(cellData);
                                    int.TryParse(cellData, out number);
                                    // number of minutes
                                    if (number >= 10 && number < 30)
                                    {
                                        mutamer_fees.Rows[i].Cells[1].Style.BackColor = Color.Yellow;
                                    }
                                    else if (number >= 30)
                                    {
                                        mutamer_fees.Rows[i].Cells[1].Style.BackColor = Color.Red;
                                    }
                                }

                            }

                        }
                    }//for 


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (DataGridViewColumn c in mutamer_fees.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);
            }


        }//jms_out
        public void TOP10UOS()
        {

    
            topten.DataSource = DBHelper.connect_oracle(SQLQueries.top10_query);
            topten.Columns[0].Width = 80; topten.Columns[0].HeaderText = "التصريح";
            topten.Columns[1].Width = 270; topten.Columns[1].HeaderText = "الشركة";
            topten.Columns[2].Width = 80; topten.Columns[2].HeaderText = "الموفا";

            topten.Columns[0].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            topten.Columns[1].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            topten.Columns[2].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

            foreach (DataGridViewColumn c in topten.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 18F, GraphicsUnit.Pixel);
            }


        }
        public void UOS_left()
        {
         

            dg_canelled.DataSource = DBHelper.connect_oracle(SQLQueries.uos_left_query);
            dg_canelled.Columns[0].Width = 80; dg_canelled.Columns[0].HeaderText = "التصريح";
            dg_canelled.Columns[1].Width = 270; dg_canelled.Columns[1].HeaderText = "الشركة";
            dg_canelled.Columns[2].Width = 80; dg_canelled.Columns[2].HeaderText = "الحالة";

            dg_canelled.Columns[0].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            dg_canelled.Columns[1].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            dg_canelled.Columns[2].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

            foreach (DataGridViewColumn c in dg_canelled.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 18F, GraphicsUnit.Pixel);
            }



        }
        public string UOS_left_message()
        {
           

            string message = " error ";
            // send whatsapp
            try
            {

                using (OracleConnection conn = new OracleConnection(Consts.connection))
                using (OracleCommand cmd = new OracleCommand(SQLQueries.uos_left_message_query, conn))
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    DataRow[] dr = dataTable.Select();
                    //*******************************

                    if (dataTable.Rows.Count > 0)
                    {
                        message = "";


                        for (int i = 0; i < dataTable.Select().Length; i++) //loop rows
                        {
                            message += " تم فسخ عقد";
                            foreach (DataColumn c in dr[i].Table.Columns)  //loop through the columns. 
                            {
                                message += " " + dataTable.Rows[i][c.ColumnName].ToString() + "   ";
                            }
                            message += "\n";
                        }//for 

                    }


                }
                return message;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "error";
            }



        }
        public void checkGroups()
        {
            //  string connection = "Data Source=172.16.1.23:1521/UMR1; User Id = umra1439;Password=dev9341";
            dg_groups.DataSource = null;
            try
            {
                using (OracleConnection conn = new OracleConnection(Consts.connection))
                using (OracleCommand cmd = new OracleCommand(SQLQueries.check_groups_query, conn))
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    DataRow[] dr = dataTable.Select();
                    //*******************************
                    dg_groups.AutoGenerateColumns = false;
                    //Set Columns Count
                    dg_groups.ColumnCount = 7;
                    //Add Columns
                    dg_groups.Columns[0].Name = "GT_GR_MOH_STATE"; dg_groups.Columns[0].HeaderText = "GT_GR_MOH_STATE"; dg_groups.Columns[0].DataPropertyName = "GT_GR_MOH_STATE";
                    dg_groups.Columns[1].Name = "GT_GR_STATE"; dg_groups.Columns[1].HeaderText = "GT_GR_STATE"; dg_groups.Columns[1].DataPropertyName = "GT_GR_STATE";
                    dg_groups.Columns[2].Name = "DELAY_MINS"; dg_groups.Columns[2].HeaderText = "DELAY_MINS"; dg_groups.Columns[2].DataPropertyName = "DELAY_MINS";
                    dg_groups.Columns[3].Name = "group_count"; dg_groups.Columns[3].HeaderText = "group_count"; dg_groups.Columns[3].DataPropertyName = "group_count";
                    dg_groups.Columns[4].Name = "status"; dg_groups.Columns[4].HeaderText = "status"; dg_groups.Columns[4].DataPropertyName = "status";
                    dg_groups.Columns[5].Name = "MUTAMERS"; dg_groups.Columns[5].HeaderText = "MUTAMERS"; dg_groups.Columns[5].DataPropertyName = "MUTAMERS";

                    dg_groups.DataSource = dataTable;

                    //    DataColumn dataColumn = dr[0].Table.ColumnName;
                    for (int i = 0; i < dataTable.Select().Length; i++) //loop rows
                    {
                        foreach (DataColumn c in dr[i].Table.Columns)  //loop through the columns. 
                        {
                            if (c.ColumnName.Equals("DELAY_MINS"))
                            {
                                string cellData = dataTable.Rows[i][c.ColumnName].ToString();
                                if (cellData != null)
                                {
                                    //int number = int.Parse(cellData);
                                    int number; // = int.Parse(cellData);
                                    int.TryParse(cellData, out number);
                                    // number of minutes
                                    if (number >= 20 && number < 50)
                                    {
                                        dg_groups.Rows[i].Cells[2].Style.BackColor = Color.Yellow;
                                    }
                                    else if (number >= 50)
                                    {
                                        dg_groups.Rows[i].Cells[2].Style.BackColor = Color.Red;

                                    }
                                }

                            }


                        }
                    }//for 


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //   dg_groups.DataSource = connect_oracle(query,connection, dg_groups);

            dg_groups.Columns[0].Width = 50; dg_groups.Columns[0].HeaderText = "MOH_STATE";
            dg_groups.Columns[1].Width = 50; dg_groups.Columns[1].HeaderText = "STATE";
            dg_groups.Columns[2].Width = 100; dg_groups.Columns[2].HeaderText = " التأخير بالدقائق";
            dg_groups.Columns[3].Width = 50; dg_groups.Columns[3].HeaderText = "عدد المجموعات";
            dg_groups.Columns[4].Width = 150; dg_groups.Columns[4].HeaderText = "الحالة ";
            dg_groups.Columns[5].Width = 50; dg_groups.Columns[5].HeaderText = "المعتمرين  ";

            foreach (DataGridViewColumn c in dg_groups.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);
            }

        }
        public void JMS_OUT()
        {

           
            dg_out.DataSource = DBHelper.connect_oracle(SQLQueries.JMS_out_query);

            dg_out.Columns[0].Width = 50; dg_out.Columns[0].HeaderText = "CNT";
            dg_out.Columns[1].Width = 300; dg_out.Columns[1].HeaderText = "ERROR DESCRIPTION";
            dg_out.Columns[2].Width = 40; dg_out.Columns[2].HeaderText = "STATUS";


            foreach (DataGridViewColumn c in dg_out.Columns)
                c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);


        }//jms_out
        public void JMS_INC()
        {
           
            dg_inc.DataSource = DBHelper.connect_oracle(SQLQueries.JMS_in_query);

            dg_inc.Columns[0].Width = 50;  dg_inc.Columns[0].HeaderText = "CNT";
            dg_inc.Columns[1].Width = 300; dg_inc.Columns[1].HeaderText = "ERROR DESCRIPTION";
            dg_inc.Columns[2].Width = 40;  dg_inc.Columns[2].HeaderText = "STATUS";

            foreach (DataGridViewColumn c in dg_inc.Columns)
                c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);

        }//jms_out
        public void JMS_INCOMING_DELAY()
        {

            try
            {
                using (OracleConnection conn = new OracleConnection(Consts.connection))
                using (OracleCommand cmd = new OracleCommand(SQLQueries.JMS_in_delay_query, conn))
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    DataRow[] dr = dataTable.Select();
                    //*******************************
                    delay_inc.AutoGenerateColumns = false;
                    //Set Columns Count
                    delay_inc.ColumnCount = 4;
                    //Add Columns
                    delay_inc.Columns[0].Name = "QUEUE_ID"; delay_inc.Columns[0].HeaderText = "QUEUE_ID"; delay_inc.Columns[0].DataPropertyName = "QUEUE_ID";
                    delay_inc.Columns[1].Name = "Q_NAME"; delay_inc.Columns[1].HeaderText = "Q_NAME"; delay_inc.Columns[1].DataPropertyName = "Q_NAME";
                    delay_inc.Columns[2].Name = "LAST_PARSE_SINCE"; delay_inc.Columns[2].HeaderText = "LAST_PARSE_SINCE"; delay_inc.Columns[2].DataPropertyName = "LAST_PARSE_SINCE";
                    delay_inc.Columns[3].Name = "LAST_Arrive_SINCE"; delay_inc.Columns[3].HeaderText = "LAST_Arrive_SINCE"; delay_inc.Columns[3].DataPropertyName = "LAST_Arrive_SINCE";

                    delay_inc.DataSource = dataTable;
                    delay_inc.Columns[0].Width = 50; delay_inc.Columns[0].HeaderText = "Q_ID";
                    delay_inc.Columns[1].Width = 150; delay_inc.Columns[1].HeaderText = "QUEUE";
                    delay_inc.Columns[2].Width = 100; delay_inc.Columns[2].HeaderText = "LAST PARSED";
                    delay_inc.Columns[3].Width = 100; delay_inc.Columns[3].HeaderText = "LAST RECEIVED";

                    foreach (DataGridViewColumn c in delay_inc.Columns)
                    {
                        c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);
                    }

                    //    DataColumn dataColumn = dr[0].Table.ColumnName;
                    for (int i = 0; i < dataTable.Select().Length; i++) //loop rows
                    {
                        foreach (DataColumn c in dr[i].Table.Columns)  //loop through the columns. 
                        {
                            if (c.ColumnName.Equals("HRS"))
                            {
                                string cellData = dataTable.Rows[i][c.ColumnName].ToString();
                                //int number = int.Parse(cellData);
                                int number; // = int.Parse(cellData);
                                int.TryParse(cellData, out number);

                                // number of minutes
                                if (number >= 20 && number < 50)
                                {
                                    delay_inc.Rows[i].Cells[2].Style.BackColor = Color.Yellow;
                                }
                                else if (number >= 50)
                                {
                                    delay_inc.Rows[i].Cells[2].Style.BackColor = Color.Red;

                                }
                            }
                        }
                    }//for 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }//jms_out
        public void send_SMS(string mobile,int sms_status,DataTable dataTable, OracleConnection conn)
        {
            //   SMS SEND
            int minutes = 0;
            if(dataTable.Rows.Count != 0)
            {
                object field = dataTable.Rows[0][2];
                minutes = Convert.ToInt32(field);
                // int cellData1 =(int) dataTable.Rows[0].Field<int>(2);
                //    int.TryParse(cellData1, out minutes);
                if (minutes <= 3)
                {
                    OracleCommand sms = new OracleCommand(SQLQueries.moh_notifications_query, conn);
                    string MSG = dataTable.Rows[0].Field<string>(1);

                    sms = new OracleCommand("INSERT INTO " + ConfigurationManager.AppSettings["Schema_year"] +
                        ".SMS_JOB_MSG(SJM_ID, SJM_SJTL_ID, SJM_MOBILE, SJM_MSG, SJM_UNICODE ,SJM_SENT) "
                        + "VALUES ( " + ConfigurationManager.AppSettings["Schema_year"] + ".SEQ_SJM.NEXTVAL, 394, :pMobile,  :pMSG    , 1 ,:pSent)  ", conn);
                    sms.Parameters.Add("pMobile", mobile);
                    sms.Parameters.Add("pMSG", MSG);
                    sms.Parameters.Add("pSent", sms_status);
                    sms.ExecuteNonQuery();
                }
            }
  
        }
        public void MOH_NOTIFICATIONS()
        {

   
            //  dg_inc.DataSource = DBHelper.connect_oracle(query);
            try
            {
                using (OracleConnection conn = new OracleConnection(Consts.connection))
                using (OracleCommand cmd = new OracleCommand(SQLQueries.moh_notifications_query, conn))
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    DataRow[] dr = dataTable.Select();
                    //*******************************
                    if (dataTable.Rows.Count != 0)
                    {
                        dg_moh_notifications.AutoGenerateColumns = false;
                    //Set Columns Count
                    dg_moh_notifications.ColumnCount = 2;
                    //Add Columns
                    dg_moh_notifications.Columns[0].Name = "LAST_ARRIVE_SINCE"; dg_moh_notifications.Columns[0].HeaderText = "LAST_ARRIVE_SINCE"; dg_moh_notifications.Columns[0].DataPropertyName = "LAST_ARRIVE_SINCE";
                    dg_moh_notifications.Columns[1].Name = "MSG"; dg_moh_notifications.Columns[1].HeaderText = "MSG"; dg_moh_notifications.Columns[1].DataPropertyName = "MSG";

                 
                    dg_moh_notifications.DataSource = dataTable;
                    dg_moh_notifications.Columns[0].Width = 100; dg_moh_notifications.Columns[0].HeaderText = "وصلت منذ";
                    dg_moh_notifications.Columns[1].Width = 400; dg_moh_notifications.Columns[1].HeaderText = "المحتوى";


                    dg_moh_notifications.Rows[0].Cells[0].Selected = false;

                    // send sms
            
                        send_SMS("966583770664", 0, dataTable, conn);
                        send_SMS("966559191947", 0, dataTable, conn);
                        send_SMS("966555570171", 0, dataTable, conn);
                        send_SMS("966504302195", 0, dataTable, conn);
                        send_SMS("966501648941", 0, dataTable, conn);
                        send_SMS("966504678243", 0, dataTable, conn);
                        send_SMS("966531910792", 0, dataTable, conn);

                        // end SMS SEND
                        foreach (DataGridViewColumn c in dg_moh_notifications.Columns)
                    {
                        c.DefaultCellStyle.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);
                    }
                   
                    //    DataColumn dataColumn = dr[0].Table.ColumnName;
                    for (int i = 0; i < dataTable.Select().Length; i++) //loop rows
                    {
                        foreach (DataColumn c in dr[i].Table.Columns)  //loop through the columns. 
                        {
                            if (c.ColumnName.Equals("MINS"))
                            {
                                string cellData = dataTable.Rows[i][c.ColumnName].ToString();
                                //int number = int.Parse(cellData);
                                int number; // = int.Parse(cellData);
                                int.TryParse(cellData, out number);

                                // number of minutes
                                if (number >= 1 && number < 15)
                                {
                                    dg_moh_notifications.Rows[i].Cells[0].Style.BackColor = Color.Yellow;
                                }
                                else if (number >= 15 && number <=60)
                                {
                                    dg_moh_notifications.Rows[i].Cells[0].Style.BackColor = Color.Red;
                                }
                                else
                                {
                                    dg_moh_notifications.Rows[i].Cells[0].Style.BackColor = Color.Gray;
                                }
                            }
                        }
                    }//for 
                  }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void show_notification(string title, string content)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.TitleText = title;
            popup.ContentText = "تم التنفيذ " + content + DateTime.Now.ToString();
            popup.Popup();// show
        }
        private void InitializeTimerGroups()
        {
            
            Consts.timerJMS.Interval = 1000; // count down value every second 
            Consts.timerJMS.Enabled = true;
            // Hook up timer's tick event handler.  
            Consts.timerJMS.Tick += new EventHandler(timerGroups_Tick);
        }
    

    
        private void timerGroups_Tick(object sender, System.EventArgs e)
        {
            if (Consts.schedule_groups <= 0)
            {

                dg_groups.DataSource = null;
                dg_max.DataSource = null;
                dg_trx.DataSource = null;

                disable_functions();

                checkGroups();
                max_functions();
                check_trx();
                total_mofa_label();


                show_notification("GROUPS checks are done", "GROUPS");
                checkLog.Text = "تم  فحص المجموعات بتاريخ  " + DateTime.Now.ToString();

                write_log(login.username + " was logged ," + " Groups is done at " + DateTime.Now.ToString()
                   + " Was set at :" + lHour.Text + ":" + minute.Text + ":" + second.Text);

                //TimeSpan TodayTime = DateTime.Now.TimeOfDay;
                logGroups.Text = DateTime.Now.ToString();
                //  WindowState = FormWindowState.Normal; 
                //    timer1.Enabled = false;  // to stop timer
                Consts.schedule_groups = Consts.Groups_SET; //reset timer

                //enable
                enable_functions1();
             

            }
            else
            {

                if (checkBox_groups.Checked == true || checkBox_stopAll.Checked)
                {
                    Consts.schedule_groups -= 0;
                }
                else
                {
                    Consts.schedule_groups -= 1;
                }

                TimeSpan t = TimeSpan.FromSeconds(Consts.schedule_groups);
                string answer = "" + string.Format("{0:D2} : {1:D2} : {2:D2}  ", t.Hours, t.Minutes, t.Seconds); //,t.Milliseconds
                if (Consts.schedule_groups <= 15)
                {
                    groupsTimer.ForeColor = Color.Red;
                }
                else if (Consts.schedule_groups > 15)
                {
                    groupsTimer.ForeColor = Color.Green;
                }
                groupsTimer.Text = answer;
            }
        }
        private void InitializeTimerJMS()
        {
            Consts.timerJMS.Interval = 1000; // count down value every second 
            Consts.timerJMS.Enabled = true;
            // Hook up timer's tick event handler.  
            Consts.timerJMS.Tick += new System.EventHandler(this.timerJMS_Tick);
        }
        private void timerJMS_Tick(object sender, System.EventArgs e)
        {
            if (Consts.schedule <= 0)
            {

                //  to avoid error "column count propery"
                delay_inc.DataSource = null;
                dg_inc.DataSource = null;
                dg_out.DataSource = null;
                dg_moh_notifications.DataSource = null;

                //  disable
                disable_functions();
                delay_inc.Rows.Clear();
                JMS_OUT();
                JMS_INC();
                JMS_INCOMING_DELAY();
                MOH_NOTIFICATIONS();

                show_notification("JMS CHECKS ARE DONE", "JMS");
                checkLog.Text = "تم  فحص جي ام اس بتاريخ  " + DateTime.Now.ToString();
                write_log(login.username + " was logged ," + " JMS is done at " + DateTime.Now.ToString()
                    + " Was set at :" + lHour.Text + ":" + minute.Text + ":" + second.Text);
                logJMS.Text = DateTime.Now.ToString();

                Consts.schedule = Consts.JMS_SET; //reset timer


                //  enable
                enable_functions1();

            }
            else
            {
                if (checkBox_JMS.Checked == true || checkBox_stopAll.Checked)
                {
                    Consts.schedule -= 0;
                }
                //false
                else
                {
                    Consts.schedule -= 1;
                }

                TimeSpan t = TimeSpan.FromSeconds(Consts.schedule);
                string answer = "" + string.Format("{0:D2} : {1:D2} : {2:D2}  ", t.Hours, t.Minutes, t.Seconds); //,t.Milliseconds
                if (Consts.schedule <= 15)
                {
                    JMSTimer.ForeColor = Color.Red;
                }
                else if (Consts.schedule > 15)
                {
                    JMSTimer.ForeColor = Color.Green;
                }
                JMSTimer.Text = answer;

                int hour = DateTime.Now.Hour;
                //if (hour > 12)
                //{ hour -= 12; }
                string timenow = string.Format("{0:D2}:{1:D2}:{2:D2}  ", hour, DateTime.Now.Minute, DateTime.Now.Second); //,t.Milliseconds

                //get hijri date
                CultureInfo arSA = CultureInfo.CreateSpecificCulture("ar-SA");
                arSA.DateTimeFormat.Calendar = new UmAlQuraCalendar();
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                string hijri_month = dt.ToString("MM", arSA);

                DateTimeFormatInfo HDTF = new CultureInfo("ar-SA", false).DateTimeFormat;
                HDTF.Calendar = new UmAlQuraCalendar();
                //HDTF.Calendar = new HijriCalendar();
                var hijri_date = DateTime.Now.ToString("dddd/d/MMMM/yyyy هــ ", HDTF);

                systemDate.Text = "" + timenow + DateTime.Now.ToString("tt", CultureInfo.InvariantCulture) + " ( " + hijri_month + ")" + " هـ" + "\n" + hijri_date;

                tHour = Int32.Parse(lHour.Text);
                tMin = Int32.Parse(minute.Text);
                tSec = Int32.Parse(second.Text);
                // if the time matches then send mail
                STMPMail();



            }
        }
        public void InitializeTimerWebservice()
        {
            Consts.timerJMS.Interval = 1000; // count down value every second 
            Consts.timerJMS.Enabled = true;
            // Hook up timer's tick event handler.  
            Consts.timerJMS.Tick += new EventHandler(timerWebservice_Tick);
        }

        private bool CheckWSWorking;
         

        private void timerWebservice_Tick(object sender, System.EventArgs e)
        {
            if (Consts.schedule_webservice <= 0)
            {
               if (CheckWSWorking)
                    return;

               CheckWSWorking = true;
                //  disable
                disable_functions();
                Application.DoEvents();
                sms.DataSource = null;
                mutamer_fees.DataSource = null;

                sms_check();
                //fees();
                Application.DoEvents();
                elm_deleted();
                Application.DoEvents();
                elm_status();
                Application.DoEvents();
                TOP10UOS();
                Application.DoEvents();
                UOS_left();
                Application.DoEvents();

                show_notification("WEBSERVICE checks are done", "WEBSERVICE");
                checkLog.Text = "تم فحص الويب سرفس بتاريخ  " + DateTime.Now.ToString();

                write_log(login.username + " was logged ," + " webservice is done at " + DateTime.Now.ToString()
                    + " Was set at :" + lHour.Text + ":" + minute.Text + ":" + second.Text);

                logWebservice.Text = DateTime.Now.ToString();
                inProcess = false;
                //    WindowState = FormWindowState.Normal;
                //  timer1.Enabled = false;  // to stop timer
                Consts.schedule_webservice = Consts.Webservice_SET; //reset timer

                enable_functions1();
                CheckWSWorking = false;
            }
            else
            {
                //  count down.  condition to stop auto timer
                if (checkBox_webservice.Checked || checkBox_stopAll.Checked)
                {

                    Consts.schedule_webservice -= 0;
                }
                else
                {
                    Consts.schedule_webservice -= 1;
                }

                TimeSpan t = TimeSpan.FromSeconds(Consts.schedule_webservice);
                string answer = "" + string.Format("{0:D2} : {1:D2} : {2:D2}  ", t.Hours, t.Minutes, t.Seconds); //,t.Milliseconds
                if (Consts.schedule_webservice <= 15)
                {
                    webserviceTimer.ForeColor = Color.Red;
                }
                else if (Consts.schedule_webservice > 15)
                {
                    webserviceTimer.ForeColor = Color.Green;
                }
                webserviceTimer.Text = answer;
            }
        }

        private void b_10_Click(object sender, EventArgs e)
        {
            int x = 0;

            string query = "    SELECT    (TRUNC ((SYSDATE-( SUB_MAIN.LAST_REPLY))*24)||' : ')||(ROUND(MOD((SYSDATE-( SUB_MAIN.LAST_REPLY))*24*60,60))||' : ')||(ROUND(MOD((SYSDATE-( SUB_MAIN.LAST_REPLY))*24*60*60,60))||'  ')DELAY," +
                "GR_UO_CODE   ,  X.R_ID GROUP_REQUEST_1, GT_MOH_GROUP_CODE, GT_GROUP_CODE, GT_GR_STATE, GT_GR_MOH_STATE, X.MSG_TIME REQUEST1_SENT, SUB_MAIN.LAST_REPLY" +
                " FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS A, " + ConfigurationManager.AppSettings["Schema_year"] + ".DPU_GROUP_TRACKER B, " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST X" +
                " , (             SELECT C.R_ID AS RID, C.R_CVC AS CVC, C.MSG_TIME AS LAST_REPLY, ERR_DESC" +
                " FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST_REPLY  C, (" +
                "   SELECT  MAX(A.MSG_TIME) AS MAX_DATE, A.R_ID" +
                "  FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST_REPLY  A, " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST  B" +
                "   WHERE A.R_ID = B.R_ID    " +
                "   GROUP BY A.R_ID     ) SUB" +
                "  WHERE SUB.R_ID = C.R_ID AND SUB.MAX_DATE = C.MSG_TIME   ) SUB_MAIN WHERE B.GT_GROUP_CODE = A.GR_CODE" +
                "      AND(:GS = 1 OR GT_GR_STATE = :GS)" +
                "   AND GT_GROUP_CODE = X.R_ENITITY_PK    AND SUB_MAIN.RID = X.R_ID" +
                "   AND GR_UO_CODE != 888       AND(ERR_DESC IS NULL  AND   SUB_MAIN.CVC != 0)" +
                "   AND GT_GR_MOH_STATE != 514       AND GR_FROM_COUNTRY_ID != 967" +
                "  ORDER BY   DELAY desc"
                ;


            if (radioButton1.Checked == true)
            {
                //string msg_status = g_status.Text;// error_msg.Text;
                //int x = 0;
                //x = int.Parse(msg_status);
                // x = 8;
                //excute others
                disable_functions();
                groups_request0();
                enable_functions();
                // 

            }
            else if (radioButton2.Checked == true)
            {
                disable_functions();

                groups10_400();
                enable_functions();
                //   x = 10;
            }
            else if (radioButton3.Checked == true)
            {
                disable_functions();
                x = 8;
                try
                {
                    using (OracleConnection conn = new OracleConnection(Consts.connection))
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add(new OracleParameter("GS", x));
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dg_group10.DataSource = dataTable;

                            conn.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                enable_functions();
                //   x = 10;
            }
            else if (radioButton4.Checked == true)
            {
                disable_functions();
                x = 10;
                try
                {
                    using (OracleConnection conn = new OracleConnection(Consts.connection))
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add(new OracleParameter("GS", x));
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dg_group10.DataSource = dataTable;

                            conn.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                enable_functions();
                //   x = 10;
            }
            else if (radioButton5.Checked == true)
            {
                disable_functions();
                x = 12;
                try
                {
                    using (OracleConnection conn = new OracleConnection(Consts.connection))
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add(new OracleParameter("GS", x));
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dg_group10.DataSource = dataTable;

                            conn.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                enable_functions();
                //   x = 10;
            }





        }

        private void generate_ticket_R0(object sender, EventArgs e)
        {
            disable_functions();
            generate_sejel_ticket();
            enable_functions();
        }
        public void generate_sejel_ticket()
        {
            //  dg_inc.DataSource = DBHelper.connect_oracle(query);
            try
            {
                OracleConnection conn = new OracleConnection(Consts.connection);

                OracleCommand R0 = new OracleCommand(SQLQueries.queryR0, conn);
                OracleCommand R1 = new OracleCommand(SQLQueries.queryR1, conn);
                OracleCommand R1_400 = new OracleCommand(SQLQueries.queryR1_400, conn);
                conn.Open();

                OracleDataReader reader_R0 = R0.ExecuteReader();
                OracleDataReader reader_R1 = R1.ExecuteReader();
                OracleDataReader reader_R1_400 = R1_400.ExecuteReader();

                DataTable dataTable_R0 = new DataTable();
                DataTable dataTable_R1 = new DataTable();
                DataTable dataTable_R1_400 = new DataTable();

                dataTable_R0.Load(reader_R0);
                dataTable_R1.Load(reader_R1);
                dataTable_R1_400.Load(reader_R1_400);
                
                string rq_string = " ";
                int count = 0;
                richTextBox1.Text = " ";
                //if (Int32.Parse(comboBox2.Text) == 0)
                if (radioButton6.Checked)
                {

                    for (int i = 0; i < dataTable_R0.Rows.Count; i++)
                    {
                        if (Int32.Parse(dataTable_R0.Rows[i]["MINUTES"].ToString()) >= Int32.Parse(delay_minutes.Text))
                        {
                            count++;
                            //     richTextBox1.Text = richTextBox1.Text + dataTable_R0.Rows[i]["R_ID"].ToString() + "\n";
                            rq_string = rq_string + dataTable_R0.Rows[i]["R_ID"].ToString() + "\n";

                            //     r0_list = new List<int>();
                            r0_list.Add(Int32.Parse(dataTable_R0.Rows[i]["R_ID"].ToString()));
                        }

                    }
                    if (count > 0)
                    {
                        richTextBox1.Text = "السادة شركة سجل التقنية المحترمين \n" + "السلام عليكم ورحمه الله وبركاته \n \n";
                        richTextBox1.Text = richTextBox1.Text + "يوجد مجموعات request 11 بدون رد من طرفكم    \n" + "REQUEST_ID_UA  \n" + "============== \n ";
                        richTextBox1.Text = richTextBox1.Text + rq_string;
                        richTextBox1.Text = richTextBox1.Text + "\n" + "شاكرين تعاونكم الدائم ";

                    }
                    else if (count <= 0)
                    {
                        MessageBox.Show("  لايوجد مجموعات متأخرة أكثر من   " + delay_minutes.Text + " دقيقة");
                    }
                    count = 0;
                    rq_string = " ";
                }

                //   else if (Int32.Parse(comboBox2.Text) == 1)
                else if (radioButton7.Checked)
                {
                    //   richTextBox1.Text = richTextBox1.Text + "يوجد مجموعات request 1 بدون رد من طرفكم    \n" + "REQUEST_ID_UA  \n" + "============== \n ";
                    //     delay_minutes.Text = "30";
                    //     delay_minutes.Refresh();
                    for (int i = 0; i < dataTable_R1.Rows.Count; i++)
                    {

                        if (Int32.Parse(dataTable_R1.Rows[i]["MINUTES"].ToString()) > Int32.Parse(delay_minutes.Text))
                        {
                            count++;
                            rq_string = rq_string + dataTable_R1.Rows[i]["R_ID"].ToString() + "\n";

                            OracleCommand R11 = new OracleCommand(SQLQueries.queryR1, conn);


                            r1_list.Add(Int32.Parse(dataTable_R1.Rows[i]["R_ID"].ToString()));

                        }


                    }

                    // 100 , 400
                    for (int i = 0; i < dataTable_R1_400.Rows.Count; i++)
                    {

                        if (Int32.Parse(dataTable_R1_400.Rows[i]["MINUTES"].ToString()) > Int32.Parse(delay_minutes.Text))
                        {
                            count++;
                            // richTextBox1.Text = richTextBox1.Text + dataTable_R1_400.Rows[i]["R_ID"].ToString() + "\n";
                            rq_string = rq_string + dataTable_R1_400.Rows[i]["R_ID"].ToString() + "\n";

                            r1_400_list.Add(Int32.Parse(dataTable_R1_400.Rows[i]["R_ID"].ToString()));


                        }

                    }
                    if (count > 0)
                    {
                        richTextBox1.Text = "السادة شركة سجل التقنية المحترمين \n" + "السلام عليكم ورحمه الله وبركاته \n \n";
                        richTextBox1.Text = richTextBox1.Text + "يوجد مجموعات request 1 بدون رد من طرفكم    \n" + "REQUEST_ID_UA  \n" + "============== \n ";
                        richTextBox1.Text = richTextBox1.Text + rq_string;
                        richTextBox1.Text = richTextBox1.Text + "\n" + "شاكرين تعاونكم الدائم ";
                    }
                    else if (count <= 0)
                    {
                        MessageBox.Show("  لايوجد مجموعات ركوست 1 متأخرة أكثر من   " + delay_minutes.Text + " دقيقة");
                    }
                    //clear
                    count = 0;
                    rq_string = " ";
                    conn.Dispose();
                    conn.Close();
                }
                else //show all
                {
                    MessageBox.Show("الرجاء تحديد نوع الطلب ");
                }
                //         richTextBox1.Text = richTextBox1.Text + "\n" + "شاكرين تعاونكم الدائم ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void send_ticket_Click(object sender, EventArgs e)
        {

            if (richTextBox1.TextLength > 6) //richTextBox1.TextLength > 6
            {
                string fromAddress = ConfigurationManager.AppSettings["Email"];
                string mailPassword = ConfigurationManager.AppSettings["Password"];
                string Host = ConfigurationManager.AppSettings["SMTPEmail"];
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = Host;
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(fromAddress, mailPassword);

                var send_mail = new MailMessage();
                send_mail.IsBodyHtml = true;
                send_mail.From = new MailAddress(fromAddress);
                send_mail.To.Add(new MailAddress(ConfigurationManager.AppSettings["csMail"])); //
                if (radioButton6.Checked) //r0
                {
                    send_mail.Subject = "Request 0 no reply " + DateTime.Now.ToString();
                    send_mail.Body = "Automatic Mail <br/> <br/>" + richTextBox1.Text.Replace("\n", "<br/> ");
                }
                else if (radioButton7.Checked)
                {
                    send_mail.Subject = "Request 1 no reply " + DateTime.Now.ToString();
                    send_mail.Body = "Automatic Mail <br/> <br/>" + richTextBox1.Text.Replace("\n", "<br/> ");
                }

                OracleConnection conn = new OracleConnection(Consts.connection);
                OracleCommand R0 = new OracleCommand(SQLQueries.queryR0, conn);
                OracleCommand R1 = new OracleCommand(SQLQueries.queryR1, conn);
                OracleCommand R1_400 = new OracleCommand(SQLQueries.queryR1_400, conn);
                conn.Open();

                if (r0_list != null)
                {
                    //  MessageBox.Show("sdsds");
                    foreach (var request_id in r0_list)
                    {
                        //R0 = new OracleCommand("INSERT INTO " + ConfigurationManager.AppSettings["Schema_year"] + ".groups_history (GR_REQ_NO) " + "VALUES (:pDeptNo)", conn);
                        //R0.Parameters.Add("pDeptNo", request_id);
                        //R0.ExecuteNonQuery();
                    }
                    r0_list.Clear();

                }
                else if (r0_list == null)
                {
                    MessageBox.Show("تم المخاطبة مسبقا بأرقام المجموعات أعلاه");
                }
                if (r1_list != null)
                {
                    foreach (var request_id in r1_list)
                    {
                        //R1 = new OracleCommand("INSERT INTO " + ConfigurationManager.AppSettings["Schema_year"] + ".groups_history (GR_REQ_NO) " + "VALUES (:pDeptNo)", conn);
                        //R1.Parameters.Add("pDeptNo", request_id);
                        //R1.ExecuteNonQuery();
                    }
                    r1_list.Clear();

                }
                else if (r1_list == null)
                {
                    MessageBox.Show("تم المخاطبة مسبقا بأرقام المجموعات أعلاه");
                }
                if (r1_400_list != null)
                {
                    foreach (var request_id in r1_400_list)
                    {
                        //R1_400 = new OracleCommand("INSERT INTO " + ConfigurationManager.AppSettings["Schema_year"] + ".groups_history (GR_REQ_NO) " + "VALUES (:pDeptNo)", conn);
                        //R1_400.Parameters.Add("pDeptNo", request_id);
                        //R1_400.ExecuteNonQuery();
                    }
                    r1_400_list.Clear();

                }
                else if (r1_400_list == null)
                {
                    MessageBox.Show("تم المخاطبة مسبقا بأرقام المجموعات أعلاه");
                }

                conn.Dispose();
                conn.Close();

                client.Send(send_mail);
                Clipboard.SetText(richTextBox1.Text);
                richTextBox1.Text = " ";
                Process.Start("https://support.sejeltech.com/sejelsm/ess.do");
            }
            else
            {
                MessageBox.Show("no data");
            }

        }
        private void JMSTimer_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Set New Timer Value", "Input", "1".ToString(), -1, -1);
            if (input.Length > 0)
            {
                Consts.JMS_SET = int.Parse(input);
                Consts.JMS_SET = Consts.JMS_SET * 1; //60
                if (Consts.JMS_SET >= 2)
                {
                    Consts.schedule = Consts.JMS_SET;
                }
                else if (Consts.JMS_SET < 120)
                {
                    string s1 = "لا يمكن أن يكون وقت التشغيل أقل من ";
                    string s2 = " ثانية";
                    MessageBox.Show(s1 + "120" + s2);
                }

            }
        }
        private void groupsTimer_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Set New Timer Value", "Input", "1".ToString(), -1, -1);
            if (input.Length > 0)
            {

                Consts.Groups_SET = int.Parse(input);
                Consts.Groups_SET = Consts.Groups_SET * 60;
                if (Consts.Groups_SET >= 1)
                {
                    Consts.schedule_groups = Consts.Groups_SET;
                }
                else if (Consts.Groups_SET < 1)
                {
                    string s1 = "لا يمكن أن يكون وقت التشغيل أقل من ";
                    string s2 = " دقيقة";
                    MessageBox.Show(s1 + "1" + s2);
                }

            }
        }
        private void webserviceCheck_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Set New Timer Value", "Input", "1".ToString(), -1, -1);
            if (input.Length > 0)
            {

                Consts.Webservice_SET = int.Parse(input);
                Consts.Webservice_SET = Consts.Webservice_SET * 60
                      ;
                if (Consts.Webservice_SET >= 3) //15*60
                {
                    Consts.schedule_webservice = Consts.Webservice_SET;
                }
                else if (Consts.Webservice_SET < 3)
                {
                    string s1 = "لا يمكن أن يكون وقت التشغيل أقل من ";
                    string s2 = " دقيقة";
                    MessageBox.Show(s1 + "3" + s2);
                }

            }
        }
        public void output_DR_tables()
        {
            try
            {
                dg_uos.DataSource = null; dg_eas.DataSource = null; dg_c_uos.DataSource = null; dg_countries.DataSource = null;
                dg_country_performance.DataSource = null;
                dg_uo_performance.DataSource = null;

                dg_uos.DataSource = DBHelper.connect_oracle(SQLQueries.Q_UOS);
                dg_eas.DataSource = DBHelper.connect_oracle(SQLQueries.Q_EAS);
                dg_c_uos.DataSource = DBHelper.connect_oracle(SQLQueries.Q_C_UOS);
                dg_countries.DataSource = DBHelper.connect_oracle(SQLQueries.Q_C);
                //Application.DoEvents();
                // dailymofa

                dg_dm.DataSource = DBHelper.connect_oracle(SQLQueries.Q_DR);
                //Application.DoEvents();
                // dailymofa C
                dg_dmc.DataSource = DBHelper.connect_oracle(SQLQueries.Q_DRC);
                //Application.DoEvents();

                //performance
                dg_uo_performance.DataSource = DBHelper.connect_oracle(SQLQueries.Q_UOS_PERFORMANCE);
                //Application.DoEvents();
                dg_country_performance.DataSource = DBHelper.connect_oracle(SQLQueries.Q_C_PERFORMACE);
                //Application.DoEvents();
                // this is only to fix column width on datagrid
                dg_dm.Columns[0].Width = 38;
                dg_dm.Columns[1].Width = 250;

                dg_dmc.Columns[0].Width = 38;
                dg_dmc.Columns[1].Width = 250;
                for (int i = 2; i <= 31; i++)
                {
                    DataGridViewColumn column = dg_dm.Columns[i];
                    DataGridViewColumn column2 = dg_dmc.Columns[i];
                    column.Width = 38;
                    column2.Width = 38;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void daily_report()
        {
            int numberOfSheets = 7;
            Excel.Application xlexcel;
            Excel.Workbook xlWorkBook;

            Excel.Worksheet xlWorkSheet1;// defines sheet1
            Excel.Worksheet xlWorkSheet2;// defines sheet2
            Excel.Worksheet xlWorkSheet3;// defines sheet3
            Excel.Worksheet xlWorkSheet4;// defines sheet3
            Excel.Worksheet xlWorkSheet5;// defines sheet5
            Excel.Worksheet xlWorkSheet6;// defines sheet6
            Excel.Worksheet xlWorkSheet7;// defines sheet7 uos performace
            Excel.Worksheet xlWorkSheet8;// defines sheet7 countries performace

            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application();
            // xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            for (int i = 0; i < numberOfSheets; i++) // here we add the rest of sheet into the excel
            {
                xlexcel.Sheets.Add(After: xlexcel.Sheets[xlexcel.Sheets.Count]);
            }
            xlWorkSheet1 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1); //setting the first sheet equal to first sheet in excel
            xlWorkSheet2 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);//setting the 2nd sheet equal to first sheet in excel
            xlWorkSheet3 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(3);
            xlWorkSheet4 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(4);
            xlWorkSheet5 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(5);
            xlWorkSheet6 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(6);
            xlWorkSheet7 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(7);
            xlWorkSheet8 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(8);

            export_sheet(xlWorkSheet1, dg_uos, "شركات");
            export_sheet(xlWorkSheet2, dg_eas, "وكالات");
            export_sheet(xlWorkSheet3, dg_c_uos, "دول- شركات");
            export_sheet(xlWorkSheet4, dg_countries, "دول");
            export_sheet(xlWorkSheet5, dg_dm, "شركات - موفا يومية");
            export_sheet(xlWorkSheet6, dg_dmc, " دول - موفا يومية");
            export_sheet(xlWorkSheet7, dg_uo_performance, " أداء الشركات");
            export_sheet(xlWorkSheet8, dg_country_performance, " أداء الدول");
            //get hijri date
            CultureInfo arSA = CultureInfo.CreateSpecificCulture("ar-SA");
            arSA.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string hijri_now = dt.ToString("dd-MM-yyyy", arSA);

            Excel.Worksheet sheet = (Excel.Worksheet)xlexcel.Worksheets[1];
            sheet.Select(Type.Missing);
            //outpot to excel
            string file_name = " إجمالي موفا ودخول وخروج والمتواجدون" + " " + hijri_now + " هـ";
            xlWorkBook.SaveAs("C:\\DC\\" + file_name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //Change all cells' alignment to center
            xlWorkBook.Application.ActiveWorkbook.Sheets[1].Select(Type.Missing); //activates first 
            xlWorkBook.Close(true, misValue, misValue);
            xlexcel.Quit();
            show_notification("توليد اكسل", "تم تصدير التقرير بنجاح على المسار C:\\DR");
            


        }
        public void export_sheet(Excel.Worksheet sheet, DataGridView dg, string name)
        {
            for (int j = 0; j <= dg.ColumnCount - 1; j++)
            {
                sheet.Cells[1, j + 1] = dg.Columns[j].HeaderText;
            }
            for (int i = 0; i <= dg.RowCount - 1; i++)
            {
                for (int j = 0; j <= dg.ColumnCount - 1; j++)
                {
                    DataGridViewCell cell = dg[j, i];
                    sheet.Cells[i + 2, j + 1] = cell.Value;
                }
            }
            if (align_center.Checked)
            {
                sheet.Range["A1:A2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; sheet.Range["B1:B2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                sheet.Range["C1:C2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; sheet.Range["D1:D2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                sheet.Range["E1:E2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; sheet.Range["F1:F2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                sheet.Range["G1:G2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; sheet.Range["H1:H2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                sheet.Range["I1:I2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; sheet.Range["J1:J2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                sheet.Range["K1:K2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; sheet.Range["L1:L2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                sheet.Range["M1:M2500"].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }



            sheet.Name = name;
            sheet.Columns.AutoFit();
        }
        private void export_excel_Click(object sender, EventArgs e)
        {
            disable_functions();
            output_DR_tables();
            enable_functions();

            //   daily_report();
        }
        private void generate_excelReport_Click(object sender, EventArgs e)
        {
            //STOP AUTO CHECKS
            export_excel.Enabled = false;
            delete_tables.Enabled = false;
            checkBox_stopAll.Checked = true;
            daily_report();
            //RESUME CHECKS
            checkBox_stopAll.Checked = false;
            export_excel.Enabled = true;
            delete_tables.Enabled = true;
        }
        private void delete_tables_Click(object sender, EventArgs e)
        {
            generate_excelReport.Enabled = false;
            dg_uos.DataSource = null; dg_eas.DataSource = null; dg_c_uos.DataSource = null; dg_countries.DataSource = null;
        }

        #region auto_daily_check_timer
        static int tHour = 9;
        static int tMin = 0;
        static int tSec = 0;
        #endregion
        private void STMPMail()
        {
            try
            {

           
            string fromAddress = ConfigurationManager.AppSettings["Email"];
            string mailPassword = ConfigurationManager.AppSettings["Password"];
            bool isCheckedAlready = false;
            //  string mailTo = sendTo.Text;
            //string mailCC = sendCC.Text;
            if (autoSend.Checked)
            {
                if (DateTime.Now.Hour == tHour && DateTime.Now.Minute == tMin && DateTime.Now.Second == tSec)
                {
                    checkBox_stopAll.Checked = true;
                    //string picPath = @"C:\DC\Screenshots\printscreen.jpg";
                 //   PrintScreen(picPath);

                    CultureInfo arSA = CultureInfo.CreateSpecificCulture("ar-SA");
                    arSA.DateTimeFormat.Calendar = new UmAlQuraCalendar();
                    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    string hijri_now = dt.ToString("dd-MM-yyyy", arSA);

                    string sy = hijri_now.Substring(5, 5);
                    string sm = hijri_now.Substring(3, 2);
                    string ss = hijri_now.Substring(0, 2);
                    string file_name = " إجمالي موفا ودخول وخروج والمتواجدون" + " " + hijri_now + " هـ";
                    string mail_title = " إجمالي موفا ودخول وخروج والمتواجدون" + " " + ss + " - " + sm + " - " + sy + " هـ";
                    string path = @"C:\DC\" + file_name + ".xls";

                    if (File.Exists(path))
                    {

                        string Host = ConfigurationManager.AppSettings["SMTPEmail"];

                        SmtpClient client = new SmtpClient();
                        client.Port = 587;
                        client.Host = Host;
                        client.EnableSsl = true;
                        client.Timeout = 1000000;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential(fromAddress, mailPassword);

                        var send_mail = new MailMessage();
                        send_mail.IsBodyHtml = true;
                        send_mail.From = new MailAddress(fromAddress);

                        if (checkBoxTest.Checked)
                        {

                            //send_mail.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToZahrani"]));
                            send_mail.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToKayali"]));
                            //send_mail.CC.Add(new MailAddress(ConfigurationManager.AppSettings["ToAli"]));
                            //send_mail.CC.Add(new MailAddress(ConfigurationManager.AppSettings["ToOmar"]));

                        }

                        else if (checkBoxTest.Checked == false)
                        {
                            send_mail.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToSupport"]));
                            send_mail.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToFadel"]));
                            send_mail.CC.Add(new MailAddress(ConfigurationManager.AppSettings["CCKhalid"]));
                        }

                        send_mail.Subject = mail_title;
                        send_mail.Body = "مرفق لكم إجمالي موفا دخول وخروج ومتواجدون";

                        string attachmentFilename = @"C:\DC\" + file_name + ".xls";

                        if (attachmentFilename != null)
                            send_mail.Attachments.Add(new Attachment(attachmentFilename));

                        client.Send(send_mail);

                        show_notification("إشعار إيميل", "تم ارسال ايميل التقرير اليومي");

                    }
                    else  // no file
                    {

                        if (checkBox_stopAll.Checked)
                        {
                            isCheckedAlready = true;
                        }

                        disable_functions();
                        output_DR_tables();

                        //STOP AUTO CHECKS
                        export_excel.Enabled = false;
                        delete_tables.Enabled = false;
                        checkBox_stopAll.Checked = true;
                        //generate report and write to disk
                        daily_report();
                        //RESUME CHECKS
                        export_excel.Enabled = true;
                        delete_tables.Enabled = true;
                        //if all tunred off
                        if (isCheckedAlready == false)
                        {
                            enable_functions();
                            checkBox_stopAll.Checked = false;

                        }
                        else if (isCheckedAlready == true)
                        {
                            enable_functions();
                            checkBox_stopAll.Checked = true;
                        }
                    

                            string Host = ConfigurationManager.AppSettings["SMTPEmail"];

                        SmtpClient client = new SmtpClient();
                        client.Port = 587;
                        client.Host = Host;
                        client.EnableSsl = true;
                        client.Timeout = 1000000;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential(fromAddress, mailPassword);

                        var send_mail = new MailMessage();
                        send_mail.IsBodyHtml = true;
                        send_mail.From = new MailAddress(fromAddress);

                        if (checkBoxTest.Checked)
                        {

                            //send_mail.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToZahrani"]));
                            send_mail.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToKayali"]));
                            //send_mail.CC.Add(new MailAddress(ConfigurationManager.AppSettings["ToAli"]));

                        }
                        else if (checkBoxTest.Checked == false)
                        {
                            send_mail.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToSupport"]));
                            send_mail.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToFadel"]));
                            send_mail.CC.Add(new MailAddress(ConfigurationManager.AppSettings["CCKhalid"]));
                        }


                        send_mail.Subject = mail_title;
                        send_mail.Body = "مرفق لكم إجمالي موفا دخول وخروج ومتواجدون";


                        string attachmentFilename = @"C:\DC\" + file_name + ".xls";

                        if (attachmentFilename != null && File.Exists(attachmentFilename))
                        {

                       
                            send_mail.Attachments.Add(new Attachment(attachmentFilename));

                        client.Send(send_mail);

                        show_notification("إشعار إيميل", "تم ارسال ايميل التقرير اليومي");
                        }
                        else
                        {
                            write_log("CANNOT SEND EMAIL FILE DOESNT EXIST" + DateTime.Now.ToString()
                          + " Was set at :" + lHour.Text + ":" + minute.Text + ":" + second.Text);
                        }
                    }//end else


                }// end time confitions
                checkBox_stopAll.Checked = false;

            }
            }
            catch (Exception ex)
            {

                write_log(ex.Message+"-"+ex.ToString());
            }

        }
        public void write_log(string log)
        {
            ///
            // Specify a name for your top-level folder.
            string folderName = @"C:\DC";
            System.IO.Directory.CreateDirectory(folderName);
            // string dateNow = DateTime.Now.TimeOfDay.ToString();
            string fileName = "log.txt";
            string pathString;
            // Use Combine again to add the file name to the path.
            pathString = System.IO.Path.Combine(folderName, fileName);

            if (!System.IO.File.Exists(pathString))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(pathString)) { }
                File.AppendAllText(pathString, log + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(pathString, log + Environment.NewLine);
            }


        }
        private void PrintScreen(string path)
        {

            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(printscreen as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            string directory = System.IO.Path.Combine(@"C:\DC\", "Screenshots");
            Directory.CreateDirectory(directory); // no need to check if it exists
            printscreen.Save(path, ImageFormat.Jpeg);

        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            lHour.Enabled = false;
            minute.Enabled = false;
            second.Enabled = false;
            autoSend.Enabled = false;
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Password Required", "Enter Password", "Enter Password ...");
            if (input.Equals("Dash"))
            {
                lHour.Enabled = true;
                minute.Enabled = true;
                second.Enabled = true;
                autoSend.Enabled = true;
            }
        }

        public void disable_functions()
        {
            checkBox_stopAll.Checked = true;
            //checkBox_JMS.Checked = true;
            //checkBox_webservice.Checked = true;
            updateButton.Enabled = false;
            generate_ticket.Enabled = false;
            b_10.Enabled = false;
            delete_tables.Enabled = false;
            generate_excelReport.Enabled = false;
            export_excel.Enabled = false;

            indicatorLevel.Text = "WORKING ...".ToString();
            indicatorLevel.ForeColor = Color.Blue;
            statusStrip1.Update();
        }
        public void enable_functions1()
        {
            checkBox_stopAll.Checked = false;
            updateButton.Enabled = true;
            generate_ticket.Enabled = true;
            b_10.Enabled = true;
            delete_tables.Enabled = true;
            if (dg_uos.Rows.Count != 0 && dg_eas.Rows.Count != 0 && dg_c_uos.Rows.Count != 0 && dg_countries.Rows.Count != 0)
            {
                generate_excelReport.Enabled = true;
            }
            export_excel.Enabled = true;
            indicatorLevel.Text = "READY";
            indicatorLevel.ForeColor = Color.Green;
        }
        public void enable_functions()
        {
            checkBox_stopAll.Checked = false;
            checkBox_JMS.Checked = false;
            checkBox_webservice.Checked = false;
            updateButton.Enabled = true;
            generate_ticket.Enabled = true;
            b_10.Enabled = true;
            delete_tables.Enabled = true;
            if (dg_uos.Rows.Count != 0 && dg_eas.Rows.Count != 0 && dg_c_uos.Rows.Count != 0 && dg_countries.Rows.Count != 0)
            {
                generate_excelReport.Enabled = true;
            }
            export_excel.Enabled = true;
            indicatorLevel.Text = "READY";
            indicatorLevel.ForeColor = Color.Green;
        }

        private void wa_btn_Click(object sender, EventArgs e)
        {

        }
    }


}

