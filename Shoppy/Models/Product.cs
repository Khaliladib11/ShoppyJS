using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.Models
{
    public class Product
    {
        public int Pid { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public List<string> Categories { get; set; }
        public string CoverImage { get; set; }
        public float ProductPrice { get; set; }
        public float ProductRate { get; set; }
        public int ProductDiscount { get; set; }
    }
}
