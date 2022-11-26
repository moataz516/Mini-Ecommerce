using ECommerceProject.Helpers;
using ECommerceProject.Interfaces;
using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Components
{
    public class SmallCartViewComponent : ViewComponent, ISmallCartViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            SamllCartViewModel cartViewModel;
            if (cart == null)
            {
                //cartViewModel = null;
                cartViewModel = new SamllCartViewModel()
                {
                    NumberOfItems = 0,
                    TotalAmount = 0,
                };
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



        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
        //    SamllCartViewModel cartViewModel;
        //    if (cart == null || cart.Count == 0)
        //    {
        //        cartViewModel = null;
        //    }
        //    else
        //    {
        //        cartViewModel = new SamllCartViewModel()
        //        {
        //            NumberOfItems = cart.Sum(x => x.Quantity),
        //            TotalAmount = cart.Sum(x => x.Quantity * x.Price),
        //        };
        //    }
        //    return View(cartViewModel);
        //}



    }
}
