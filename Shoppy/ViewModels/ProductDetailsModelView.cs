using Shoppy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shoppy.ViewModels
{
    public class ProductDetailsModelView
    {
        public Product product { get; set; }
        public List<Product> relatedProdcut { get; set; }
    }
}
