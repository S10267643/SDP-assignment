namespace SDP_assignment
{
    public class AddOnDecorator : FoodDecorator
    {
        private readonly string _addOnName;
        private readonly decimal _priceDelta;

        public AddOnDecorator(IFoodItem inner, string addOnName, decimal priceDelta)
            : base(inner)
        {
            _addOnName = addOnName;
            _priceDelta = priceDelta;
        }

        public override decimal GetPrice() => Inner.GetPrice() + _priceDelta;

        public override string GetDescription() => $"{Inner.GetDescription()} + {_addOnName}";
    }
}
