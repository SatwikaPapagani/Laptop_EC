using LaptopCart.Services;
using Microsoft.AspNetCore.Mvc;

namespace LaptopCart.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly IPaymentService _paymentService;
        public SubscriptionController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        public IActionResult Index()
        {
            string result = _paymentService.pay(500);
            return View();
        }
    }
}
