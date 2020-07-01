using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Tech_Admin.Connection_String;
using Tech_Admin.Models;

namespace Tech_Admin.Controllers
{
    public class DAL
    {
        public List<Employee_Master> GetData()
        {
            SqlConnection conn = new SqlConnection(AppSettingConnectionString.GetConnectionString("entity"));
            SqlCommand cmd = new SqlCommand("spSelectEmployee", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            conn.Open();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            List<Employee_Master> emp = new List<Employee_Master>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Employee_Master emp2 = new Employee_Master();
                    emp2.EmpId = Convert.ToInt32(row["EmpId"].ToString());
                    emp2.Name = row["Name"].ToString();
                    emp2.Phone = row["Phone"].ToString();
                    emp2.Date_Of_Birth = Convert.ToDateTime(row["Date_Of_Birth"].ToString());
                    emp2.Address = row["Address"].ToString();
                    emp2.EmailId = row["EmailId"].ToString();
                    emp2.Country = Convert.ToInt32(row["Country"].ToString());
                    emp2.State = Convert.ToInt32(row["State"].ToString());
                    emp2.City = Convert.ToInt32(row["City"].ToString());
                    emp.Add(emp2);
                }
            }
            return emp;
        }


        public List<Country_Info> GetCountry()
        {
            SqlConnection conn = new SqlConnection(AppSettingConnectionString.GetConnectionString("entity"));
            SqlCommand cmd = new SqlCommand("spGetCountry", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            conn.Open();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            List<Country_Info> ct = new List<Country_Info>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Country_Info cnt = new Country_Info();
                    cnt.Country_Id = Convert.ToInt32(row["Country_Id"].ToString());
                    cnt.Country_name = row["Country_name"].ToString();
                    ct.Add(cnt);
                }
            }
            return ct;
        }

        public List<State_info> GetState()
        {
            SqlConnection conn = new SqlConnection(AppSettingConnectionString.GetConnectionString("entity"));
            SqlCommand cmd = new SqlCommand("spGetState", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            conn.Open();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            List<State_info> ct = new List<State_info>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    State_info cnt = new State_info();
                    cnt.State_Id = Convert.ToInt32(row["State_Id"].ToString());
                    cnt.State_Name = row["State_name"].ToString();
                    
                    ct.Add(cnt);
                }
            }
            return ct;
        }

        public List<City_Info> GetCity()
        {
            SqlConnection conn = new SqlConnection(AppSettingConnectionString.GetConnectionString("entity"));
            SqlCommand cmd = new SqlCommand("spGetCity", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            conn.Open();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            List<City_Info> ct = new List<City_Info>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    City_Info  cnt = new City_Info();
                    cnt.City_Id = Convert.ToInt32(row["City_Id"].ToString());
                    cnt.City_Name = row["City_Name"].ToString();
                   
                    ct.Add(cnt);
                }
            }
            return ct;
        }


    }
}