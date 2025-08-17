using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class Customer : User, Observer
    {
        private List<Order> orderHistory = new List<Order>();

        public void PlaceOrder(Restaurant restaurant, List<MenuItem> items)
        {
            if (restaurant == null)
            {
                Console.WriteLine("Error: No restaurant selected");
                return;
            }

            if (items == null || items.Count == 0)
            {
                Console.WriteLine("Error: No items selected for order");
                return;
            }

            var order = new Order
            {
                CustomerName = this.Name,
                Items = new List<MenuItem>(items) // Create a copy to prevent external modification
            };

            Console.WriteLine("Select payment method:");
            Console.WriteLine("1. Credit Card");
            Console.WriteLine("2. PayPal");
            Console.WriteLine("3. Cash on Delivery");

            bool validChoice = false;
            while (!validChoice)
            {
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        order.SetPaymentStrategy(new CreditCardPayment());
                        validChoice = true;
                        break;
                    case "2":
                        order.SetPaymentStrategy(new PayPalPayment());
                        validChoice = true;
                        break;
                    case "3":
                        order.SetPaymentStrategy(new CashOnDeliveryPayment());
                        validChoice = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select 1, 2, or 3");
                        break;
                }
            }

            try
            {
                order.ProcessPayment();
                orderHistory.Add(order);
                Console.WriteLine("Order placed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing payment: {ex.Message}");
            }
        }

        public void ViewOrderHistory()
        {
            if (!orderHistory.Any())
            {
                Console.WriteLine("No order history found.");
                return;
            }

            Console.WriteLine($"Order History for {Name}:");
            var oneYearAgo = DateTime.Now.AddYears(-1);

            var recentOrders = orderHistory
                .Where(o => o.OrderDate >= oneYearAgo)
                .OrderByDescending(o => o.OrderDate);

            foreach (var order in recentOrders)
            {
                Console.WriteLine($"\nOrder #{order.Id} - {order.OrderDate}");
                Console.WriteLine($"Total: {order.TotalAmount:C}");
                Console.WriteLine("Items:");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"- {item.Name} ({item.Price:C})");
                }
            }
        }

        public void Update(MenuItem newItem)
        {
            Console.WriteLine($"[LATEST NOTIFICATION] {Name}, {newItem.Name} was just added to the menu!");
            Console.WriteLine($"- Price: {newItem.Price:C}");
            Console.WriteLine($"- Description: {newItem.Description}\n");
        }

        // Helper method to add order to history (used by Program.cs)
        public void AddOrderToHistory(Order order)
        {
            if (order != null)
            {
                orderHistory.Add(order);
            }
        }
    }
}