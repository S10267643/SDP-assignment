using System;

namespace SDP_assignment
{

    public class PendingPaymentState : OrderState
    {
        public void Handle(Order order)
        {
            Console.WriteLine("Order is pending payment");
        }
    }
}