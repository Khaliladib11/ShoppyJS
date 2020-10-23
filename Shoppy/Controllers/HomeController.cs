using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shoppy.DataAccess;
using Shoppy.Models;
using Shoppy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;


namespace Shoppy.Controllers
{
    public class HomeController : Controller
    {
        ProductDbHandler _productDb;
        CategoryDbHandler _categoryDbHandler;
        UserDbHandler _userDbHandler;
        ShoppingCartDbHandler _shoppingCartDbHandler;
        public HomeController(IConfiguration configuration)
        {
            _productDb = new ProductDbHandler(configuration);
            _categoryDbHandler = new CategoryDbHandler(configuration);
            _userDbHandler = new UserDbHandler(configuration);
            _shoppingCartDbHandler = new ShoppingCartDbHandler(configuration);
        }
        public IActionResult Index()
        {

            var email = Request.Cookies["email"];
            var password = Request.Cookies["password"];
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                User user = _userDbHandler.login(email, password);
                if (user.Username != null)
                {
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("username", user.Username);
                    HttpContext.Session.SetString("role", user.Role);
                    HttpContext.Session.SetInt32("uid", user.Uid);
                }

            }
            ViewBag.Title = "Shoppy | E-Commerce Webiste";
            HomeViewModel home = new HomeViewModel()
            {
                newProducts = _productDb.getAllProducts(),
                categories = _categoryDbHandler.getAllCategories(),
            };
            List<Product> products = _productDb.getAllProducts();
            return View(home);
        }

     
        [HttpGet]
        public ViewResult ProductDetails(int? pid)
        {

            bool checkProduct = _productDb.checkProduct(pid ?? 1);
            if (checkProduct == true)
            {
                ProductDetailsModelView productDetailsModelView = new ProductDetailsModelView
                {
                    product = _productDb.getProductById(pid ?? 1)
                };

                return View(productDetailsModelView);
            }
            else
            {
                Response.StatusCode = 404;
                return View("ProductNotFound");
            }
            
        }

        [HttpGet][HttpPost]
        public ViewResult Categories(string category, string searchProduct, int?  min_range, int? max_range, string sortBy)
        {
            min_range = min_range.HasValue ? min_range : 0;
            max_range = max_range.HasValue ? max_range : 800;
            searchProduct = string.IsNullOrEmpty(searchProduct) ? "" : searchProduct;
            sortBy = string.IsNullOrEmpty(sortBy) ? "Latest" : sortBy;
            List<Product> pro = _productDb.getProductsByCategory(category, searchProduct, (int)min_range, (int)max_range, sortBy);
            CategoryViewModel categoryViewModel = new CategoryViewModel
            {
                Products = pro,
                Category = category,
                Categories = _categoryDbHandler.getAllCategories(),
                Search = searchProduct,
                MinPrice = (int)min_range,
                MaxPrice = (int)max_range,
                SortByList = new List<string>()
                {
                    "Latest",
                    "Rate",
                    "Price High To Low",
                    "Price Low To High",
                },
                SortBy = sortBy,
            };

            return View(categoryViewModel);
        }

        public IActionResult AddToCart()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("login", "account");
            }
            return RedirectToAction("index", "home");
        }
    }
}
