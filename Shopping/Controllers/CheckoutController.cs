using Microsoft.AspNetCore.Identity;
using Shopping.Areas.Admin.Repository;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shopping.Models;
using Shopping.Models.ViewModels;
using Shopping.Rpository;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;

        public CheckoutController(IEmailSender emailSender, DataContext context)
        {
            _dataContext = context;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if(userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var ordercode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = ordercode;
                orderItem.UserName = userEmail;
                orderItem.Status = 1;
                orderItem.CreatedDate = DateTime.Now;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
                //tạo order details
                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach (var cart in cartItems)
                {
                    var orderdetails = new OrderDetail();
                    orderdetails.UserName = userEmail;
                    orderdetails.OrderCode = ordercode;
                    orderdetails.ProductId = cart.ProductId;
                    orderdetails.Price = cart.Price;
                    orderdetails.Quantity = cart.Quantity;
                    //Update product sold
                    var product = await _dataContext.Products.Where(p => p.Id == cart.ProductId).FirstAsync();
                    product.Quantity -= cart.Quantity;
                    product.Sold += cart.Quantity;
                    _dataContext.Products.Update(product);
                    //Save order details
                    _dataContext.Add(orderdetails);
                    _dataContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                //Send mail order when success
                var receiver = userEmail; //email của nguoi đặt hàng
                var subject = "Đặt hàng thành công.";
                var message = "Đặt hàng thành công, trải nghiệm dịch vụ nhé.";

                await _emailSender.SendEmailAsync(receiver, subject, message);
                TempData["success"] = "Checkout thành công, vui lòng chờ duyệt đơn hàng.";
                return RedirectToAction("Index","Cart");
            }
            return View();
        }
    }
}
