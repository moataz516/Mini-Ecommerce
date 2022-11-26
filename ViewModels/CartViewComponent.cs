using ECommerceProject.Helpers;
using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Interfaces.Components
{

    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            SamllCartViewModel cartViewModel;
            if (cart == null || cart.Count == 0)
            {
                cartViewModel = null;
            }
            else
            {
                cartViewModel = new SamllCartViewModel()
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    TotalAmount = cart.Sum(x => x.Quantity * x.Price),
                };
            }
            return View(cartViewModel);
        }
    }
}
