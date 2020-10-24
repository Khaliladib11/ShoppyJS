using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shoppy.DataAccess;
using Shoppy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ShoppingCartDbHandler _shoppingCartDbHandler;

        public CartViewComponent(IConfiguration configuration)
        {
            _shoppingCartDbHandler = new ShoppingCartDbHandler(configuration);
        }
        public Task<IViewComponentResult> InvokeAsync()
        {
            ViewCartModelView viewCartModelView = new ViewCartModelView();
            if (HttpContext.Session.GetString("username") != null)
            {
                int uid = (int)HttpContext.Session.GetInt32("uid");
                viewCartModelView.ShoppingCartItem = _shoppingCartDbHandler.getShoppingItems(uid);
            }

            return Task.FromResult((IViewComponentResult)View(viewCartModelView));
        }
    }
}
