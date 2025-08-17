using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class Restaurant : User, Subject
    {
        public string CuisineType { get; set; }

        // Composite root
        private readonly MenuCategory rootMenu = new MenuCategory("Main Menu");

        // Observer list
        private readonly List<Observer> observers = new List<Observer>();

        // Optional flat list (if you still want it)
        public List<MenuItem> MyMenuItems { get; private set; } = new List<MenuItem>();

        // ---- Composite API ----

        public void AddMenuCategory(string categoryName)
        {
            categoryName ??= string.Empty;
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Console.WriteLine("Category name cannot be empty.");
                return;
            }

            rootMenu.Add(new MenuCategory(categoryName));
            Console.WriteLine($"Category '{categoryName}' added successfully.");
        }

        public void AddMenuItemToCategory(string categoryName, MenuItem item)
        {
            if (item == null)
            {
                Console.WriteLine("Invalid item.");
                return;
            }

            var category = FindCategory(categoryName);
            if (category != null)
            {
                category.Add(item);
                MyMenuItems.Add(item);
            }
            else
            {
                Console.WriteLine($"Category '{categoryName}' not found.");
            }
        }

        public MenuItem FindMenuItem(string itemName)
        {
            return FindMenuItemRecursive(rootMenu, itemName);
        }

        private MenuItem FindMenuItemRecursive(MenuCategory category, string itemName)
        {
            foreach (var component in category.GetChildren())
            {
                if (component is MenuItem item &&
                    item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }

                if (component is MenuCategory subCategory)
                {
                    var found = FindMenuItemRecursive(subCategory, itemName);
                    if (found != null) return found;
                }
            }
            return null;
        }

        private MenuCategory FindCategory(string categoryName)
        {
            return FindCategoryRecursive(rootMenu, categoryName);
        }

        private MenuCategory FindCategoryRecursive(MenuCategory current, string categoryName)
        {
            if (current.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
            {
                return current;
            }

            foreach (var component in current.GetChildren())
            {
                if (component is MenuCategory child)
                {
                    var found = FindCategoryRecursive(child, categoryName);
                    if (found != null) return found;
                }
            }

            return null;
        }

        public void PrintFullMenu()
        {
            rootMenu.Print();
        }

        // ---- Observer API ----

        public void PrintSubscribers()
        {
            Console.WriteLine("\nSubscribers:");
            if (observers.Count == 0)
            {
                Console.WriteLine("(none)");
                return;
            }

            foreach (var observer in observers)
            {
                if (observer is Customer customer)
                {
                    Console.WriteLine($"- {customer.Name} ({customer.Email})");
                }
            }
        }

        public void Attach(Observer observer)
        {
            if (observer == null) return;
            if (!observers.Contains(observer))
                observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            observers.Remove(observer);
        }

        // Not used in current design, but required by Subject
        public void Notify()
        {
            // Intentionally left blank; notifications are done by MenuCategory when items are added.
        }
    }
}