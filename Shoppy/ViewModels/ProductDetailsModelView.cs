using Shoppy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Shoppy.ViewModels
{
    public class ProductDetailsModelView
    {
        public Product product { get; set; }
        public List<Color> Colors { get; set; }
        public List<Size> Sizes { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Size { get; set; }
        [Required(ErrorMessage = "You have to choose a color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int Quantity { get; set; }
    }
}
