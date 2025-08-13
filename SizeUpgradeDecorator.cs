namespace SDP_assignment
{
    public class SizeUpgradeDecorator : FoodDecorator
    {
        private readonly string _size;
        private readonly decimal _priceDelta;

        public SizeUpgradeDecorator(IFoodItem inner, string size, decimal priceDelta)
            : base(inner)
        {
            _size = size;
            _priceDelta = priceDelta;
        }

        public override decimal GetPrice() => Inner.GetPrice() + _priceDelta;

        public override string GetDescription() => $"{Inner.GetDescription()} [{_size}]";
    }
}
