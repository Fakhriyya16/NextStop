
namespace Service.DTOs.Payments
{
    public class PaymentIntentRequest
    {
        public long Amount { get; set; }  
        public string Currency { get; set; }  
        public string Description { get; set; }
    }
}
