using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace AlamiahDailycheckDashboard.Helpers
{
    public static class DBHelper
    {
        public static DataTable connect_oracle(string query)
        {
            try
            {
                //dfghfgh
                string usernameOracle = login.username;
                string passwordOracle = login.password;
                string connection = "Data Source=172.16.1.34:1521/ALAM1; User Id = " + usernameOracle + ";Password=" + passwordOracle;

                using (OracleConnection conn = new OracleConnection(connection))
                {
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            conn.Dispose();
                            conn.Close();
                            return dataTable;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
