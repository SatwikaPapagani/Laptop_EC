using LaptopCart.Services;
using Microsoft.AspNetCore.Mvc;

namespace LaptopCart.Controllers
{
    public class OrderController : Controller
    {
        private readonly IPaymentService _paymentSerive;
        public OrderController(IPaymentService paymentService)
        {
            _paymentSerive = paymentService;
        }

        public IActionResult Index()
        {
            /* IPaymentService payment = new StripeService();
             string result = payment.pay(1000);
            */
            //Old way beacuse we will be having multiple controllers(like orders, subscription, Donation etc) to call this serive or method and if in sometime the service is getting changed from on other(like Stripe to Paypall)
            //then you must changes in all the controllers where this service(Stripe) is being used. Instead you can use Dependency injection concept here.

            string result = _paymentSerive.pay(1000);
            
            return View();
        }
    }
}
