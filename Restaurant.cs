using System;
using System.Globalization;

namespace SDP_assignment
{
    public class Restaurant : User
    {
        private readonly MenuComponent _allMenus; // Root of composite (MAIN MENU)

        public Restaurant()
        {
            _allMenus = new MenuCategory("MAIN MENU");
        }

        
        public MenuCategory RootMenu => (MenuCategory)_allMenus;

        
        public MenuCategory GetRootMenu() => (MenuCategory)_allMenus;

        public void AddMenuCategory(string name)
        {
            name ??= string.Empty;
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Category name cannot be empty.");
                return;
            }

            RootMenu.Add(new MenuCategory(name));
            Console.WriteLine($"Category '{name}' created successfully!");
        }

        public void AddMenuItemToCategory(string categoryName)
        {
            try
            {
                categoryName ??= string.Empty;
                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    Console.WriteLine("Category name cannot be empty.");
                    return;
                }

                // Find the target category (case-insensitive)
                MenuCategory? targetCategory = null;
                foreach (MenuComponent component in RootMenu.GetChildren())
                {
                    if (component is MenuCategory category &&
                        category.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetCategory = category;
                        break;
                    }
                }

                if (targetCategory == null)
                {
                    Console.WriteLine($"Category '{categoryName}' not found!");
                    return;
                }

                Console.Write("Item name: ");
                string name = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Item name cannot be empty.");
                    return;
                }

                Console.Write("Description: ");
                string desc = Console.ReadLine() ?? string.Empty;

                Console.Write("Price: $");
                var priceInput = Console.ReadLine() ?? string.Empty;
                if (!decimal.TryParse(priceInput, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price) &&
                    !decimal.TryParse(priceInput, out price))
                {
                    Console.WriteLine("Invalid price.");
                    return;
                }

                targetCategory.Add(new MenuItem(name, desc, price, this.UserId));
                Console.WriteLine($"Item '{name}' added to '{categoryName}'!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void PrintFullMenu()
        {
            Console.WriteLine($"\n=== {Name}'s MENU ===");
            _allMenus.Print();
        }
    }
}