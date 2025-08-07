using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class Restaurant : User
    {
        public void CreateMenuItem(List<MenuItem> menuItems, ref int idCounter)
        {
            Console.WriteLine("\n=== CREATE NEW MENU ITEM ===");
            var builder = new MenuItemBuilder();

            try
            {
                string name;
                do
                {
                    Console.Write("Enter item name (min 2 chars): ");
                    name = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(name) || name.Length < 2);

                Console.Write("Enter description: ");
                string description = Console.ReadLine();

                decimal price;
                do
                {
                    Console.Write("Enter price: $");
                } while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0);

                Console.Write("Enter category: ");
                string category = Console.ReadLine();

                var item = builder
                    .SetId(idCounter++)
                    .SetName(name)
                    .SetDescription(description)
                    .SetPrice(price)
                    .SetCategory(category)
                    .SetRestaurantId(this.Id)
                    .Build();

                menuItems.Add(item);
                Console.WriteLine($"Menu item '{name}' created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ViewMyMenuItems(List<MenuItem> menuItems)
        {
            var myItems = menuItems.Where(m => m.RestaurantId == Id).ToList();
            if (!myItems.Any())
            {
                Console.WriteLine("You have no menu items yet.");
                return;
            }

            Console.WriteLine($"\n=== YOUR MENU ITEMS ===");
            foreach (var item in myItems)
            {
                Console.WriteLine($"- {item}");
            }
        }
    }
}