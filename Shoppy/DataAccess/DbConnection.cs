using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.DataAccess
{
    public class DbConnection
    {
        public SqlConnection getConnection(IConfiguration configuration)
        {
            //string conString = "Data Source=DESKTOP-K6NC9N0\\SQLEXPRESS;Initial Catalog=Shoppy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string conString = configuration.GetSection("ConnectionString").GetSection("ShoppyDb").Value;
            SqlConnection con = new SqlConnection(conString);
            return con;
        }
    }
}
