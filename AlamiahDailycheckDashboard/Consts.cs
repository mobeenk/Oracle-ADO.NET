using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlamiahDailycheckDashboard
{
    public static class Consts
    {

       public  static Timer timerJMS= new Timer();


        //usernameOracle = "jmsumra";
        //passwordOracle = "devjms34";
        //  connection = "Data Source=172.16.1.23:1521/UMR1; User Id = " + usernameOracle + ";Password=" + passwordOracle;
        //   connection = "Data Source=172.16.1.68:1521/ALAM1; User Id = " + usernameOracle + ";Password=" + passwordOracle;
        public static int schedule = 165;
        public static int schedule_groups = 900;
        public static int schedule_webservice = 2000;
        //set  new timers 
        public static int JMS_SET = 165;
        public static int Groups_SET = 900;
        public static int Webservice_SET = 2000;

        public static  string connection =  "Data Source=172.16.1.34:1521/ALAM1; User Id = " + login.username + ";Password=" + login.password;

      
      static Consts()
      {
     
      }
        
    }
 
}
