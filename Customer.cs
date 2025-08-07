using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class Customer : User
    {
        public void ViewRestaurants(List<User> users)
        {
            Console.WriteLine("\n=== AVAILABLE RESTAURANTS ===");
            var restaurants = users.OfType<Restaurant>();

            if (!restaurants.Any())
            {
                Console.WriteLine("No restaurants available.");
                return;
            }

            foreach (var r in restaurants)
            {
                Console.WriteLine($"{r.Id}. {r.Name}");
            }
        }

        public void ViewMenuItems(List<MenuItem> items)
        {
            if (!items.Any())
            {
                Console.WriteLine("No menu items available.");
                return;
            }

            var grouped = items.GroupBy(i => i.Category);
            foreach (var group in grouped)
            {
                Console.WriteLine($"\n{group.Key.ToUpper()}");
                foreach (var item in group)
                {
                    Console.WriteLine($"- {item.Name} (${item.Price})");
                }
            }
        }
    }
}