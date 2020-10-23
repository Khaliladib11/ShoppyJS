using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.ViewModels
{
    public class CategoryViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public string Category { get; set; }
        public string Search { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public string SortBy { get; set; }
        public List<string> SortByList { get; set; }
    }
}
