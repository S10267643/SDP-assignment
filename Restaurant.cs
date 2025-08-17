

namespace SDP_assignment
{
    public class Restaurant : User, Subject
    {

        public string CuisineType { get; set; }
        private MenuCategory rootMenu = new MenuCategory("Main Menu");
        private List<Observer> observers = new List<Observer>();
        public List<MenuItem> MyMenuItems { get; private set; } = new List<MenuItem>();

        private readonly MenuComponent _allMenus; // Root of composite (MAIN MENU)


        public void AddMenuCategory(string categoryName)
        {
            rootMenu.Add(new MenuCategory(categoryName));
            Console.WriteLine($"Category '{categoryName}' added successfully.");
        }


        public void AddMenuItemToCategory(string categoryName, MenuItem item)
        {
            var category = FindCategory(categoryName);
            if (category != null)
            {
                category.Add(item);
            }
            else
            {
                Console.WriteLine($"Category '{categoryName}' not found.");
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

        public void CreateMenuItem(List<MenuItem> globalMenuItems, ref int menuItemIdCounter)
        {
            Console.WriteLine("\n=== CREATE NEW MENU ITEM ===");

            Console.Write("Enter item name: ");
            string name = Console.ReadLine();

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            Console.Write("Enter price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            var item = new MenuItem(name, description, price, this.UserId)
            {

                MenuItemId = menuItemIdCounter++
            };

            globalMenuItems.Add(item);
            MyMenuItems.Add(item);
            Console.WriteLine($"Menu item '{item.Name}' created successfully!");
        }

        // Stubs
        public void UpdateMenuItem() => Console.WriteLine("Updating menu item...");
        public void DeleteMenuItem() => Console.WriteLine("Deleting menu item...");
        public void CreateSpecialOffer() => Console.WriteLine("Creating special offer...");
        public void AcceptOrder() => Console.WriteLine("Accepting order...");
        public void RejectOrder() => Console.WriteLine("Rejecting order...");
        public void StoreDeliveredOrder() => Console.WriteLine("Storing delivered order...");

        public void AddMenuCategory(MenuCategory category)
        {
            rootMenu.Add(category);
        }

        public MenuItem FindMenuItem(string itemName)
        {
            return FindMenuItem(rootMenu, itemName);
        }

        private MenuItem FindMenuItem(MenuCategory category, string itemName)
        {
            foreach (var component in category.GetChildren())
            {
                if (component is MenuItem item && item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    return item;

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
                else if (component is MenuCategory subCategory)
                {
                    var foundItem = FindMenuItem(subCategory, itemName);
                    if (foundItem != null) return foundItem;
                }
            }
            return null;
        }

        private MenuCategory FindCategory(string categoryName)
        {
            return FindCategory(rootMenu, categoryName);
        }

        private MenuCategory FindCategory(MenuCategory current, string categoryName)
        {
            if (current.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
            {
                return current;
            }


            foreach (var component in current.GetChildren())
            {
                if (component is MenuCategory category)
                {
                    var found = FindCategory(category, categoryName);
                    if (found != null) return found;
                }
            }

            return null;
        }

        public void PrintFullMenu()
        {
            rootMenu.Print();
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


        public void PrintSubscribers()
        {
            Console.WriteLine("\nSubscribers:");
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
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        public void Detach(Observer observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            // Not used in this implementation
        }
    }
}