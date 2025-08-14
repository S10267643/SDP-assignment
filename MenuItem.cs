using System;

public class MenuItem : MenuComponent
{

    private string _name;
    private string _description;
    private decimal _price;
    private int _userId;


    public MenuItem(string name, string description, decimal price, int userId)
    {
        _name = name;
        _description = description;
        _price = price;
        _userId = userId;
    }

    public override string Name => _name;
    public override string Description => _description;
    public override decimal Price => _price;

    public override void Print()
    {
        Console.Write($"  {_name}");
        Console.WriteLine($": ${_price:N2}");
        Console.WriteLine($"  -- {_description}");
    }
}