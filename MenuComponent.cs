public abstract class MenuComponent
{
    public abstract string Name { get; }
    public virtual string Description => "";
    public virtual decimal Price => 0;
    public virtual void Add(MenuComponent component) => throw new NotImplementedException();
    public virtual void Remove(MenuComponent component) => throw new NotImplementedException();
    public virtual MenuComponent GetChild(int i) => throw new NotImplementedException();
    public abstract void Print();
}