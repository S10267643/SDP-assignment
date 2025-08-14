namespace SDP_assignment
{
    public class MenuCategory : MenuComponent
    {
        private  List<MenuComponent> _children = new List<MenuComponent>();
        private  string _name;

        public MenuCategory(string name) => _name = name;

        public override string Name => _name;

        public override void Add(MenuComponent component) => _children.Add(component);

        public override void Remove(MenuComponent component) => _children.Remove(component);

        public override MenuComponent GetChild(int i) => _children[i];

        public List<MenuComponent> GetChildren() => _children;

        public override void Print()
        {
            Console.WriteLine($"\n{_name.ToUpper()}");
            foreach (var component in _children)
                component.Print();
        }
    }
}