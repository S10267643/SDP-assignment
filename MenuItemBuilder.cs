namespace SDP_assignment
{
    public class MenuItemBuilder
    {
        private MenuItem _menuItem = new MenuItem();

        public MenuItemBuilder SetId(int id)
        {
            _menuItem.Id = id;
            return this;
        }

        public MenuItemBuilder SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
                throw new ArgumentException("Name must be at least 2 characters");
            _menuItem.Name = name;
            return this;
        }

        public MenuItemBuilder SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty");
            _menuItem.Description = description;
            return this;
        }

        public MenuItemBuilder SetPrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be positive");
            _menuItem.Price = price;
            return this;
        }

        public MenuItemBuilder SetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be empty");
            _menuItem.Category = category;
            return this;
        }

        public MenuItemBuilder SetRestaurantId(int restaurantId)
        {
            if (restaurantId <= 0)
                throw new ArgumentException("Invalid restaurant ID");
            _menuItem.RestaurantId = restaurantId;
            return this;
        }

        public MenuItem Build()
        {
            return _menuItem;
        }
    }
}