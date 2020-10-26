
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
        public ViewResult Categories(string category, string searchProduct, int?  min_range, int? max_range, string sortBy, int? page)
        {
            page = page.HasValue ? page : 1;
            min_range = min_range.HasValue ? min_range : 0;
            max_range = max_range.HasValue ? max_range : 800;
            searchProduct = string.IsNullOrEmpty(searchProduct) ? "" : searchProduct;
            sortBy = string.IsNullOrEmpty(sortBy) ? "Latest" : sortBy;

            int rowNumber = 2;
            int start = (int)((page * rowNumber) - rowNumber);
            int totalProducts = _productDb.getProductsByCategoryTotal(category, searchProduct, (int)min_range, (int)max_range);
            double totalPages = Math.Round( ((double)totalProducts / rowNumber), MidpointRounding.ToPositiveInfinity);
            

            List<Product> pro = _productDb.getProductsByCategory(category, searchProduct, (int)min_range, (int)max_range, sortBy, start, rowNumber);
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
                Page = (int)page,
                TotalPages = (int)totalPages
            };

            return View(categoryViewModel);
        }

        [HttpGet]
        public IActionResult SearchProduct(string search)
        {
            int page = 1;
            int rowNumber = 2;
            int start = (int)((page * rowNumber) - rowNumber);
            int totalProducts = _productDb.getProductsByCategoryTotal("all", search, 0, 800);
            double totalPages = Math.Round(((double)totalProducts / rowNumber), MidpointRounding.ToPositiveInfinity);

            List<Product> products = _productDb.searchProduct(search);
            CategoryViewModel categoryViewModel = new CategoryViewModel
            {
                Products = products,
                Category = "All",
                Categories = _categoryDbHandler.getAllCategories(),
                Search = search,
                MinPrice = 0,
                MaxPrice = 800,
                SortByList = new List<string>()
                {
                    "Latest",
                    "Rate",
                    "Price High To Low",
                    "Price Low To High",
                },
                SortBy = "Latest",
                Page = (int)page,
                TotalPages = (int)totalPages
            };
            return View("Categories", categoryViewModel);
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
