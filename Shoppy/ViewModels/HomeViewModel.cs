using Shoppy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> newProducts { get; set; }

        public List<Category> categories { get; set; }

        public List<Product> products { get; set; }
    }
}
