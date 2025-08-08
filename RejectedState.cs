using System;

namespace SDP_assignment
{

    public class RejectedState : OrderState
    {
        public void Handle(Order order)
        {
            Console.WriteLine("Order has been rejected");
        }
    }
}