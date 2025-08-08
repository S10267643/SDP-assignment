using System;

namespace SDP_assignment
{

    public class PaidState : OrderState
    {
        public void Handle(Order order)
        {
            Console.WriteLine("Order has been paid");
        }
    }
}