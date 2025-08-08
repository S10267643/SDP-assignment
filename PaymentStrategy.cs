namespace SDP_assignment
{
    public interface PaymentStrategy
    {
        bool ProcessPayment(decimal amount);
        bool ProcessRefund(decimal amount);
    }
}