using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shoppy.DataAccess;
using Shoppy.ViewModels;

namespace Shoppy.Controllers
{
    public class ShoppingCartController : Controller
    {
        ShoppingCartDbHandler _shoppingCartDbHandler;
        public ShoppingCartController(IConfiguration configuration)
        {
            _shoppingCartDbHandler = new ShoppingCartDbHandler(configuration);
        }
        public JsonResult AddToCart(int pid, int? quantity, int cid, int sid)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                //return RedirectToAction("login", "account");
                return Json(new { result = "Redirect", url = Url.Action("Login", "account") });
            }
            int uid = (int)HttpContext.Session.GetInt32("uid");
            quantity = quantity.HasValue ? quantity : 1;
           
            int result = _shoppingCartDbHandler.addToCart(pid, uid, (int)quantity, cid, sid);
            if(result == 1)
            {
                return Json(new { result = "Success" });
            }
            return Json(new { result = "unsuccess" });
        }

        public IActionResult ViewCart(int uid)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("login", "account");
            }
            ViewCartModelView viewCartModelView = new ViewCartModelView()
            {
                ShoppingCartItem = _shoppingCartDbHandler.getShoppingItems(uid)
            };
            return View(viewCartModelView);
        }

        public JsonResult DeleteItem(int pid, int uid)
        {
            int result = _shoppingCartDbHandler.deleteShoppingItems(uid, pid);
            if (result == 1)
            {
                return Json(new { result = "Success" });
            }
            else{
                return Json(new { result = "Faild" });
            }
        }

        public IActionResult updateCart()
        {
            return ViewComponent("ShoppingItems");
        }
        public IActionResult updateViewCart()
        {
            return ViewComponent("Cart");
        }
    }
}
