using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.Models
{
    public class ShoppingCartItem
    {
        public int pid { get; set; }
        public string productName { get; set; }
        public float productPrice { get; set; }
        public string productImg { get; set; }
        public int uid { get; set; }
        public int quantity { get; set; }
        public string color { get; set; }
        public string size { get; set; }
    }
}
