using System;
using SDP_assignment;

public class RestaurantNotifier : OrderObserver
{
    public void Update(Order order)
    {
        if (order.State is PaidState)
        {
            Console.WriteLine($"[NOTIFICATION] Restaurant: New order #{order.Id} from {order.CustomerName}");
        }
        else if (order.State is RejectedState)
        {
            Console.WriteLine($"[NOTIFICATION] Restaurant: Order #{order.Id} was rejected");
        }
    }
}

