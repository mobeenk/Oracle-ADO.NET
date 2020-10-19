using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace AlamiahDailycheckDashboard
{
    public partial class login : Form
    {
        public static string username = "";
        public static string password = "";
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            validate_connection();
            /*
            int connection_result = 0;
            connection_result = f1.validate_connection();
            if(connection_result == 1)
            {
             
            }
      */

        }
        public void  validate_connection()
        {
            string connectionJMS = "";
            username = textBox1.Text.ToString();
            password = textBox2.Text.ToString();

     connectionJMS = "Data Source=172.16.1.34:1521/ALAM1; User Id = " + username + ";Password=" + password;
       //   connectionJMS = "Data Source=172.16.1.23:1521/UMR1; User Id = " + username + ";Password=" + password;
            try
            {
                OracleConnection conn = new OracleConnection(connectionJMS);
                conn.Open();
                if (conn.State == ConnectionState.Open)  //
                {
                    this.Hide();
                    //username = "jmsumra";
                    //password = "devjms34";
                    FrmMain f1 = new FrmMain();
               //     f1.MaximizeBox = false;
                    f1.ShowDialog();
                    conn.Dispose();
                    conn.Close();
                    this.Close();

                }
           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
