namespace SDP_assignment
{
    public abstract class MenuComponent
    {
        public virtual void Add(MenuComponent component) =>
            throw new NotSupportedException();

        public virtual void Remove(MenuComponent component) =>
            throw new NotSupportedException();

        public virtual MenuComponent GetChild(int i) =>
            throw new NotSupportedException();

        public virtual string Name =>
            throw new NotSupportedException();

        public virtual string Description =>
            throw new NotSupportedException();

        public virtual decimal Price =>
            throw new NotSupportedException();

        public virtual void Print() =>
            throw new NotSupportedException();
    }
}