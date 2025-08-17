using SDP_assignment;

public class Order
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string CustomerName { get; set; }
    public DateTime OrderDate { get; } = DateTime.Now;
    public List<MenuItem> Items { get; set; } = new List<MenuItem>();
    public decimal TotalAmount => Items.Sum(item => item.Price);
    private PaymentStrategy paymentStrategy;

    public void SetPaymentStrategy(PaymentStrategy strategy)
    {
        paymentStrategy = strategy;
    }

    public void ProcessPayment()
    {
        paymentStrategy.ProcessPayment(TotalAmount);
    }
}
