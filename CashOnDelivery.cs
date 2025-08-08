using System;

namespace SDP_assignment
{
    public class CashOnDeliveryPayment : PaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Order will be paid in cash on delivery ({amount:C})");
            return true;
        }

        public bool ProcessRefund(decimal amount)
        {
            Console.WriteLine($"No refund needed for cash on delivery");
            return true;
        }
    }
}