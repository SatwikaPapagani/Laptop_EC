using LaptopCart.Services;
using System.Reflection.Metadata.Ecma335;

namespace LaptopCart.Services
{
    public class StripeService : IPaymentService
    {
        public string pay(decimal amount)
        {
            return $"Paid {amount} using Stripe";
        }
    }
}
