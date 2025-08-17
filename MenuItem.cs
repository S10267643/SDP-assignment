
namespace SDP_assignment

    // Composite leaf + ConcreteComponent for Decorator
    public class MenuItem : MenuComponent, IFoodItem

    {
        private string _name;
        private string _description;
        private decimal _price;
        private int _userId;
        public bool IsNew { get; private set; }


        // Optional: used for search/display (safe default = "")
        private string _category = string.Empty;


        // === Original constructor (kept for backward compatibility) ===
        public MenuItem(string name, string description, decimal price, int userId)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _description = description ?? string.Empty;
            _price = price;
            _userId = userId;
        }

        // === Overload with Category (non‑breaking) ===
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

        public override void Print()
        {
            Console.Write($"  {_name}: ${_price:N2}");
            if (IsNew) Console.Write("  (NEW!)");
            Console.WriteLine($"\n  -- {_description}");

        // Exposed for Strategy/search UI (optional)
        public string Category => _category;

        // === IFoodItem (Decorator expects these) ===
        public decimal GetPrice() => _price;
        public string GetDescription() => _name;

        public override void Print()
        {
            var cat = string.IsNullOrWhiteSpace(_category) ? "" : $" [{_category}]";
            Console.Write($"  {_name}{cat}");
            Console.WriteLine($": ${_price:N2}");
            if (!string.IsNullOrWhiteSpace(_description))
                Console.WriteLine($"  -- {_description}");

        }

        public void MarkAsNotNew() => IsNew = false;

    }
}