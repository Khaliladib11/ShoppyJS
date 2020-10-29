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
    public class ShoppingCartDbHandler
    {
        IConfiguration _configuration;

        public ShoppingCartDbHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int addToCart(int pid,int uid, int quantity, int cid, int sid)
        {
            DbConnection db = new DbConnection();
            SqlConnection con = db.getConnection(_configuration);
            SqlCommand cmd = new SqlCommand("insertShoppingItem", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid", pid);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.AddWithValue("@sid", sid);
            cmd.Parameters.AddWithValue("@coid", cid);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            int retval = (int)cmd.Parameters["@retValue"].Value;

            if (retval == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public List<ShoppingCartItem> getShoppingItems(int uid) 
        {
            List<ShoppingCartItem> shoppingCartItem = new List<ShoppingCartItem>();
            try
            {
                DbConnection db = new DbConnection();
                using (SqlConnection con = db.getConnection(_configuration))
                {
                    SqlCommand cmd = new SqlCommand("getShoppingItems", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@uid", uid);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Open();
                    sd.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        shoppingCartItem.Add(new ShoppingCartItem
                        {
                            pid = Int32.Parse(dr["pid"].ToString()),
                            productName = dr["productName"].ToString(),
                            productPrice = float.Parse(dr["price"].ToString()),
                            productImg = dr["coverImg"].ToString(),
                            uid = Int32.Parse(dr["uid"].ToString()),
                            quantity = Int32.Parse(dr["quantity"].ToString()),
                            size = dr["size"].ToString(),
                            color = dr["color"].ToString(),
                        });
                    }

                }
                return shoppingCartItem;
            }
            catch
            {
                return shoppingCartItem;
            }
        }

        public int deleteShoppingItems(int uid, int pid)
        {
            DbConnection db = new DbConnection();
            SqlConnection con = db.getConnection(_configuration);
            SqlCommand cmd = new SqlCommand("deleteShoppingItem", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid", pid);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            int retval = (int)cmd.Parameters["@retValue"].Value;

            if (retval == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
