using Microsoft.Extensions.Configuration;
using Shoppy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.DataAccess
{


    public class ProductDbHandler
    {
        IConfiguration _configuration;
        CategoryDbHandler _categoryDbHandler;

        public ProductDbHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Product> getAllProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                DbConnection db = new DbConnection();
                using (SqlConnection con = db.getConnection(_configuration))
                {
                    string sql = "select * from product";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Open();
                    sd.Fill(dt);
                    con.Close();
                    _categoryDbHandler = new CategoryDbHandler(_configuration);
                    foreach (DataRow dr in dt.Rows)
                    {
                        products.Add(new Product { 
                            
                            Pid = Convert.ToInt32(dr["pid"]),
                            ProductName = Convert.ToString(dr["productName"]),
                            ProductDesc = Convert.ToString(dr["productDesc"]),
                            CoverImage = Convert.ToString(dr["coverImg"]),
                            ProductPrice = float.Parse(dr["price"].ToString()),
                            ProductRate = float.Parse(dr["rate"].ToString()),
                            Categories = _categoryDbHandler.getCategorysList(Convert.ToInt32(dr["pid"])),

                        });
                    }
                }
                    return products;
            }
            catch
            {
                return products;
            }
        }

        public Product getProductById(int id)
        {
            Product product = new Product();
            try
            {
                DbConnection db = new DbConnection();
                using (SqlConnection con = db.getConnection(_configuration))
                {

                    string sql = "select pid, productname, productDesc, coverImg, price, discount, rate, catgorys = (SELECT  STUFF((SELECT ',' + c.categoryName FROM category as c, product_category as pc WHERE  c.cid = pc.cid and pc.pid = pcc.pid FOR XML PATH('')), 1, 1, '') FROM category as cc, product_category as pcc WHERE cc.cid = pcc.cid and pcc.pid = p.pid GROUP BY pid) from product as p where p.pid = "+ id;
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    con.Open();
                    sd.Fill(dt);
                    con.Close();
                    _categoryDbHandler = new CategoryDbHandler(_configuration);
                    foreach (DataRow dr in dt.Rows)
                    {
                        product.Pid = Int32.Parse(dr["pid"].ToString());
                        product.ProductName = dr["productName"].ToString();
                        product.ProductDesc = dr["productDesc"].ToString();
                        product.ProductPrice = float.Parse(dr["price"].ToString());
                        product.CoverImage = dr["coverImg"].ToString();
                        product.Categories = _categoryDbHandler.getCategorysList(Convert.ToInt32(dr["pid"]));
                        product.ProductRate = float.Parse(dr["rate"].ToString());
                    }
                    return product;
                }
            }
            catch
            {
                return product;
            }
        }

        public List<Product> getRelatedProduct(String category)
        {
            List<Product> relatedProducts = new List<Product>();
            try
            {
                DbConnection db = new DbConnection();
                using (SqlConnection con = db.getConnection(_configuration))
                {

                    string sql = "select pid, productName, productDesc, coverImg, price, categoryName, discount, rate from product, category where category.categoryName = '"+ category +"' and product.cid = category.cid;";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    con.Open();
                    sd.Fill(dt);
                    con.Close();
                    _categoryDbHandler = new CategoryDbHandler(_configuration);
                    foreach (DataRow dr in dt.Rows)
                    {
                        relatedProducts.Add(
                            new Product
                            {
                                Pid = Int32.Parse(dr["pid"].ToString()),
                                ProductName = dr["productName"].ToString(),
                                ProductDesc = dr["productDesc"].ToString(),
                                ProductPrice = float.Parse(dr["price"].ToString()),
                                CoverImage = dr["coverImg"].ToString(),
                                Categories = _categoryDbHandler.getCategorysList(Convert.ToInt32(dr["pid"])),
                                ProductRate = float.Parse(dr["rate"].ToString()),
                            });
                    }
                    return relatedProducts;
                }
            }
            catch
            {
                return relatedProducts;
            }
        }

        public List<Product> getProductsByCategory(string category, string searchProduct, int min, int max, string sortBy)
        {
            List<Product> products = new List<Product>();

            try
            {
                DbConnection db = new DbConnection();
                using (SqlConnection con = db.getConnection(_configuration))
                {

                    //string sql = "select p.* from product p, product_category pc, category c where p.pid = pc.pid and pc.cid = c.cid and c.categoryName = '"+ category + "' and p.productName LIKE '%"+ searchProduct + "%' and p.price >= "+ min +" and p.price <= " + max;
                    SqlCommand cmd = new SqlCommand("getFilterProducts", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@search", searchProduct);
                    cmd.Parameters.AddWithValue("@minPrice", min);
                    cmd.Parameters.AddWithValue("@maxPrice", max);
                    cmd.Parameters.AddWithValue("@sortBy", sortBy);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    con.Open();
                    sd.Fill(dt);
                    con.Close();
                    _categoryDbHandler = new CategoryDbHandler(_configuration);
                    foreach (DataRow dr in dt.Rows)
                    {
                        products.Add(
                            new Product
                            {
                                Pid = Convert.ToInt32(dr["pid"]),
                                ProductName = Convert.ToString(dr["productName"]),
                                ProductDesc = Convert.ToString(dr["productDesc"]),
                                CoverImage = Convert.ToString(dr["coverImg"]),
                                ProductPrice = float.Parse(dr["price"].ToString()),
                                ProductRate = float.Parse(dr["rate"].ToString()),
                                Categories = _categoryDbHandler.getCategorysList(Convert.ToInt32(dr["pid"])),
                            });
                    }
                    return products;
                }
            }
            catch
            {
                return products;
            }

        }

        public bool checkProduct(int pid)
        {
            DbConnection db = new DbConnection();
            using (SqlConnection con = db.getConnection(_configuration))
            {

                //string sql = "select p.* from product p, product_category pc, category c where p.pid = pc.pid and pc.cid = c.cid and c.categoryName = '"+ category + "' and p.productName LIKE '%"+ searchProduct + "%' and p.price >= "+ min +" and p.price <= " + max;
                SqlCommand cmd = new SqlCommand("checkProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pid", pid);
                cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                int retval = (int)cmd.Parameters["@retValue"].Value;

                if (retval == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
    }
}
