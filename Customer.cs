using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class Customer : User
    {
        public void OrderFood()
        {
            Console.WriteLine("Ordering food...");
        }

        public void CustomizeOrderItem()
        {
            Console.WriteLine("Customizing order item...");
        }

        public void SearchFoodsAndRestaurants()
        {
            Console.WriteLine("Searching for foods and restaurants...");
        }

        public void SelectPaymentMethod()
        {
            Console.WriteLine("Selecting payment method...");
        }
    }

}