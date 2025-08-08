using System;

namespace SDP_assignment
{
    public class CreditCardPayment : PaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of {amount:C}");
            return true;
        }

        public bool ProcessRefund(decimal amount)
        {
            Console.WriteLine($"Refunding {amount:C} to credit card");
            return true;
        }
    }
}