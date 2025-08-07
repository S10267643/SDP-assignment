namespace SDP_assignment
{
    public class MenuItemBuilder
    {
        private MenuItem _menuItem = new MenuItem();
        private int _restaurantId;

        public MenuItemBuilder SetRestaurantId(int restaurantId)
        {
            if (restaurantId <= 0)
                throw new ArgumentException("Invalid restaurant ID");
            _restaurantId = restaurantId;
            return this;
        }

        public MenuItem BuildFromInput(int id)
        {
            Console.WriteLine("\n=== CREATE NEW MENU ITEM ===");

            try
            {
                string name;
                do
                {
                    Console.Write("Enter item name (min 2 chars): ");
                    name = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(name) || name.Length < 2);
                _menuItem.Name = name;

                Console.Write("Enter description: ");
                _menuItem.Description = Console.ReadLine();

                decimal price;
                do
                {
                    Console.Write("Enter price: $");
                } while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0);
                _menuItem.Price = price;

                Console.Write("Enter category: ");
                string category = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(category))
                    throw new ArgumentException("Category cannot be empty");
                _menuItem.Category = category;

                _menuItem.MenuItemId = id;
                _menuItem.RestaurantId = _restaurantId;

                return _menuItem;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating menu item: {ex.Message}");
                return null;
            }
        }
    }

}