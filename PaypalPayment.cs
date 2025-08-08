using System;

namespace SDP_assignment
{
    public class PayPalPayment : PaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount:C}");
            return true;
        }

        public bool ProcessRefund(decimal amount)
        {
            Console.WriteLine($"Refunding {amount:C} via PayPal");
            return true;
        }
    }
}