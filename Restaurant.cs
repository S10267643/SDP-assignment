namespace SDP_assignment
{
    public class Restaurant : User
    {
        private readonly MenuComponent _allMenus;

        public Restaurant()
        {
            _allMenus = new MenuCategory("MAIN MENU");
        }

        public void AddMenuCategory(string name)
        {
            _allMenus.Add(new MenuCategory(name));
            Console.WriteLine($"Category '{name}' created successfully!");
        }

        public void AddMenuItemToCategory(string categoryName)
        {
            try
            {
                // Get the MAIN MENU category
                var mainMenu = _allMenus as MenuCategory;

                // Find the target category
                MenuCategory targetCategory = null;
                foreach (MenuComponent component in mainMenu.GetChildren())
                {
                    if (component is MenuCategory category && category.Name.Equals(categoryName))
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
                string name = Console.ReadLine();

                Console.Write("Description: ");
                string desc = Console.ReadLine();

                Console.Write("Price: $");
                decimal price = decimal.Parse(Console.ReadLine());

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