namespace LaptopCart.Services
{
    public class RazorPay : IPaymentService
    {
        public string pay(decimal amount)
        {
            return $" The rupiees {amount} paid using RazorPay";
        }
    }
}
