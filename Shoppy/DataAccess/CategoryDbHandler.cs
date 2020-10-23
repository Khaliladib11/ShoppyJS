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
                    string sql = "select c.categoryName, c.cid, c.categoryImage from category c";
                    SqlCommand cmd = new SqlCommand(sql, con);
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
                    string sql = "select c.categoryName, c.cid from category c, product p, product_category pc where c.cid = pc.cid and p.pid = pc.pid and p.pid = " + pid;
                    SqlCommand cmd = new SqlCommand(sql, con);
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
