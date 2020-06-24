using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


namespace Tech_Admin.Connection_String
{
    public class AppSettingConnectionString
    {
        public static string GetConnectionString(string keyname)
        {
            string connection = string.Empty;
            switch (keyname)
            {
                case "entity":
                    connection = ConfigurationManager.ConnectionStrings["entity"].ConnectionString;
                    break;
                case "EmployeeEntities":
                    connection = ConfigurationManager.ConnectionStrings["EmployeeEntities"].ConnectionString;
                    break;
                default:
                    break;
            }
            return connection;
        }
        
    }
}