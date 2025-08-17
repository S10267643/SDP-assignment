using SDP_assignment;
using System;

public class MenuCategory : MenuComponent
{
    private List<MenuComponent> _children = new List<MenuComponent>();
    private List<Observer> observers = new List<Observer>();
    private string _name;

    public MenuCategory(string name) => _name = name;

    public override string Name => _name;

    public override void Add(MenuComponent component)
    {
        _children.Add(component);
        if (component is MenuItem item)
        {
            NotifyNewMenuItem(item);
        }
    }

    public override void Remove(MenuComponent component) => _children.Remove(component);

    public override MenuComponent GetChild(int i) => _children[i];

    public List<MenuComponent> GetChildren() => _children;

    public override void Print()
    {
        Console.WriteLine($"\n{_name.ToUpper()}");
        foreach (var component in _children)
        {
            component.Print();
            if (component is MenuItem item)
                item.MarkAsNotNew();  // Item is no longer "new" after being displayed
        }
    }

    public void Attach(Observer observer) => observers.Add(observer);
    public void Detach(Observer observer) => observers.Remove(observer);

    public void NotifyNewMenuItem(MenuItem newItem)
    {
        foreach (var observer in observers)
        {
            observer.Update(newItem);
        }
    }
}
