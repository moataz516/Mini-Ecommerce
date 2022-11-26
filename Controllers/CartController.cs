using ECommerceProject.Helpers;
using ECommerceProject.Models;
using ECommerceProject.Models.Data;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Controllers
{
    [Route("cart")]

    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        [Route("Index")]
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart != null )
            {
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(i => i.Product.Price * i.Quantity);            
            }else
            {
                ViewBag.cart = null;

                ViewBag.total = 0;
            }

            return View();
        }


        [Route("buy/{id}")]
        public ActionResult Buy(int id)
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                var cart = new List<Item>();
                cart.Add(new Item() { Product = _context.Products.Find(id), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = Exists(cart, id);
                if (index == -1)
                {
                    cart.Add(new Item() { Product = _context.Products.Find(id), Quantity = 1 });
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            }
            TempData["Success"] = "The product has been added!";

            return RedirectToAction("Index");
        }


        [Route("Decrease")]

        public IActionResult Decrease(int id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

            Item cartItem =  cart.Where(c => c.Product.Id == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.Product.Id == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            TempData["Success"] = $"The product {cartItem.Product.Name} has been removed!";

            return RedirectToAction("Index");
        }


        [Route("remove/{id}")]
        public ActionResult Remove(int id)
        {

            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = Exists(cart, id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);


            return RedirectToAction("Index");
        }

        [Route("clear")]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("cart");

            return RedirectToAction("Index");
        }


        private int Exists(List<Item> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id == id)
                {
                    return i;
                }
            }
            return -1;
        }



        [TempData]
        //public string TotalAmount { get; set; }
        public int TotalAmount { get; set; }
        [HttpGet]
        public IActionResult CheckOut()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.DollarAmount = cart.Sum(i => i.Product.Price * i.Quantity);
            ViewBag.total = ViewBag.DollarAmount * 100;
            ViewBag.total = Convert.ToInt64(ViewBag.total);
            long total = ViewBag.total;
            //TotalAmount = total.ToString();
            return View();
        }


        [HttpPost]
        public IActionResult Processing(Order order)
        {
            var curUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = _context.ApplicationUsers.Find(curUser);
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

                if(user.UserWallet > 0 && user.UserWallet >= cart.Sum(i => i.Product.Price * i.Quantity))
                {
                if(cart.Sum(i => i.Product.Price * i.Quantity) == 0)
                {
                    TempData["Success"] = $"there is no item";
                    return View();
                }
                foreach(var item in cart)
                {
                    var product = _context.Products.Find(item.Product.Id);
                    if (product != null)
                    {
                        product.Qty -= item.Quantity;
                        _context.Update(product);
                        _context.SaveChangesAsync();

                    }

                    order.UserId = curUser;

                order.UserName = user.UserName;
                order.DeliveryDate = DateTime.Now.AddDays(+14);
                //order.DeliveryStaus = "Restitute";
                order.Email = user.Email;
                order.total = cart.Sum(i => i.Product.Price * i.Quantity);
                _context.Add(order);
                }
                _context.SaveChangesAsync();

                user.UserWallet -= order.total;
                _context.Update(user);
                _context.SaveChangesAsync();


                HttpContext.Session.Remove("cart");

                return RedirectToAction("Index","home");
            }
            else
            {
                TempData["Error"] = "You don't have enough money";
            }

            return RedirectToAction("Index", "Home");
        }


    }
}
