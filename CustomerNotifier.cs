using System;
using SDP_assignment;

public class CustomerNotifier : OrderObserver
{
    public void Update(Order order)
    {
        if (order.State is PaidState)
        {
            Console.WriteLine($"[NOTIFICATION] Customer: Your order #{order.Id} has been paid");
        }
        else if (order.State is RejectedState)
        {
            Console.WriteLine($"[NOTIFICATION] Customer: Your order #{order.Id} was rejected, refund issued");
        }
    }
}