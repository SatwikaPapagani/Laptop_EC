namespace LaptopCart.Services
{
    public class Paypal : IPaymentService
    {
        public string pay(decimal amount)
        {
            return $"Paid {amount} using Paypal";
        }


    }
}
