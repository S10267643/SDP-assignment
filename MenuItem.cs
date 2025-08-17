
namespace SDP_assignment
{

    public class MenuItem : MenuComponent
    {
        private string _name;
        private string _description;
        private decimal _price;
        private int _userId;
        public bool IsNew { get; private set; }

        public MenuItem(string name, string description, decimal price, int userId)
        {
            _name = name;
            _description = description;
            _price = price;
            _userId = userId;
            IsNew = true;
        }

        // Implement required abstract members from MenuComponent
        public override string Name => _name;
        public override string Description => _description;
        public override decimal Price => _price;

        public int MenuItemId { get; internal set; }

        public override void Print()
        {
            Console.Write($"  {_name}: ${_price:N2}");
            if (IsNew) Console.Write("  (NEW!)");
            Console.WriteLine($"\n  -- {_description}");
        }

        public void MarkAsNotNew() => IsNew = false;

    }
}