
using Microsoft.Extensions.Configuration;
using Shoppy.Models;
using Shoppy.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Shoppy.DataAccess
{
    public class UserDbHandler
    {
        IConfiguration _configuration;

        public UserDbHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int checkUsername(string username)
        {
            DbConnection db = new DbConnection();
            SqlConnection con = db.getConnection(_configuration);
            SqlCommand cmd = new SqlCommand("checkUsername", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
           
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            int retval = (int)cmd.Parameters["@retValue"].Value;

            if (retval == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int checkEmail(string email)
        {
            DbConnection db = new DbConnection();
            SqlConnection con = db.getConnection(_configuration);
            SqlCommand cmd = new SqlCommand("checkEmail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
       
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            int retval = (int)cmd.Parameters["@retValue"].Value;

            if (retval == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int insertUser(RegisterViewModel registerViewModel)
        {
            DbConnection db = new DbConnection();
            SqlConnection con = db.getConnection(_configuration);
            SqlCommand cmd = new SqlCommand("insertUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username", registerViewModel.Username);
            cmd.Parameters.AddWithValue("@email", registerViewModel.Email);
            cmd.Parameters.AddWithValue("@password", registerViewModel.EncryptedPass);
            cmd.Parameters.AddWithValue("@phone", registerViewModel.Phone.ToString());
            cmd.Parameters.AddWithValue("@role", registerViewModel.Role);
            cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            int retval = (int)cmd.Parameters["@retValue"].Value;

            if (retval == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public User login(string email, string password)
        {
            User user = new User();
            DbConnection db = new DbConnection();
            SqlConnection con = db.getConnection(_configuration);
            SqlCommand cmd = new SqlCommand("login", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                user.Uid = Int32.Parse(dr["uid"].ToString());
                user.Username = dr["username"].ToString();
                user.Role = dr["role"].ToString();
            }
            return user;
        }


        public int getUid(string username)
        {
            DbConnection db = new DbConnection();
            SqlConnection con = db.getConnection(_configuration);
            SqlCommand cmd = new SqlCommand("getUid", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username", username);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            int uid = 0;
            foreach (DataRow dr in dt.Rows)
            {
                uid = Int32.Parse(dr["uid"].ToString());
            }
            return uid;
        }
    }
}
