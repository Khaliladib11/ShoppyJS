using Microsoft.Extensions.Configuration;
using Shoppy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.DataAccess
{
    public class CategoryDbHandler
    {
        IConfiguration _configuration;

        public CategoryDbHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Category> getAllCategories()
        {
            List<Category> Categories = new List<Category>();
            try
            {
                DbConnection db = new DbConnection();
                using (SqlConnection con = db.getConnection(_configuration))
                {
                    SqlCommand cmd = new SqlCommand("getAllCategories", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Open();
                    sd.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        Categories.Add(new Category
                        {
                            Cid = Int32.Parse(dr["cid"].ToString()),
                            CategoryName = dr["categoryName"].ToString(),
                            CategoryImage = dr["categoryImage"].ToString()
                        });
                    }
                }
                return Categories;
            }
            catch
            {
                return Categories;
            }
        }
        public List<Category> getCategorysList(int pid)
        {
            List<Category> Categories = new List<Category>();
            try
            {
                DbConnection db = new DbConnection();
                using (SqlConnection con = db.getConnection(_configuration))
                {
                    SqlCommand cmd = new SqlCommand("getCategoryList", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pid", pid);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Open();
                    sd.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        Categories.Add(new Category
                        {
                            Cid = Int32.Parse(dr["cid"].ToString()),
                            CategoryName = dr["categoryName"].ToString()
                        });
                    }
                }
                return Categories;
            }
            catch
            {
                return Categories;
            }
        }
    }
}
