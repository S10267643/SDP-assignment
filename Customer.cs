using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class Customer : User
    {
        private List<Order> orderHistory = new List<Order>();
        public void PlaceOrder(Restaurant restaurant, List<MenuItem> items)
        {
            var order = new Order
            {
                CustomerName = this.Name,
                Items = items
            };

            // Set payment strategy based on user choice
            Console.WriteLine("Select payment method:");
            Console.WriteLine("1. Credit Card");
            Console.WriteLine("2. PayPal");
            Console.WriteLine("3. Cash on Delivery");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    order.SetPaymentStrategy(new CreditCardPayment());
                    break;
                case "2":
                    order.SetPaymentStrategy(new PayPalPayment());
                    break;
                case "3":
                    order.SetPaymentStrategy(new CashOnDeliveryPayment());
                    break;
                default:
                    Console.WriteLine("Invalid choice, defaulting to Credit Card");
                    order.SetPaymentStrategy(new CreditCardPayment());
                    break;
            }

            // Attach observers
            order.AttachObserver(new RestaurantNotifier());
            order.AttachObserver(new CustomerNotifier());

            // Process payment
            order.ProcessPayment();

            // Add to history if successful
            if (order.State is PaidState)
            {
                orderHistory.Add(order);
                Console.WriteLine("Order placed successfully!");
            }
        }

        public void ViewOrderHistory()
        {
            Console.WriteLine($"Order History for {Name}:");
            var oneYearAgo = DateTime.Now.AddYears(-1);

            foreach (var order in orderHistory.Where(o => o.OrderDate >= oneYearAgo))
            {
                Console.WriteLine($"\nOrder #{order.Id} - {order.OrderDate}");
                Console.WriteLine($"Status: {order.State.GetType().Name}");
                Console.WriteLine($"Total: {order.TotalAmount:C}");
                Console.WriteLine("Items:");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"- {item.Name} ({item.Price:C})");
                }
            }
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