using System;

namespace SDP_assignment
{
    // Composite leaf + ConcreteComponent for Decorator
    public class MenuItem : MenuComponent, IFoodItem
    {
        private string _name;
        private string _description;
        private decimal _price;
        private int _userId;
        public bool IsNew { get; private set; }

        // Optional: used for search/display
        private string _category = string.Empty;

        // === Original constructor (backward compatibility) ===
        public MenuItem(string name, string description, decimal price, int userId)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _description = description ?? string.Empty;
            _price = price;
            _userId = userId;
        }

        // === Overload with Category ===
        public MenuItem(string name, string description, string category, decimal price, int userId)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _description = description ?? string.Empty;
            _category = category ?? string.Empty;
            _price = price;
            _userId = userId;
            IsNew = true;
        }

        public override string Name => _name;
        public override string Description => _description;
        public override decimal Price => _price;

        public int MenuItemId { get; internal set; }

        // === Print (Composite leaf override) ===
        public override void Print()
        {
            var cat = string.IsNullOrWhiteSpace(_category) ? "" : $" [{_category}]";
            Console.Write($"  {_name}{cat}: ${_price:N2}");
            if (IsNew) Console.Write("  (NEW!)");
            Console.WriteLine();
            if (!string.IsNullOrWhiteSpace(_description))
                Console.WriteLine($"  -- {_description}");
        }

        // Exposed for Strategy/search UI
        public string Category => _category;

        // === IFoodItem (Decorator expects these) ===
        public decimal GetPrice() => _price;
        public string GetDescription() => _name;

        public void MarkAsNotNew() => IsNew = false;
    }
}