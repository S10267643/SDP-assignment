namespace SDP_assignment
{
    public class SideSwapDecorator : FoodDecorator
    {
        private readonly string _fromSide;
        private readonly string _toSide;
        private readonly decimal _swapFee;

        public SideSwapDecorator(IFoodItem inner, string fromSide, string toSide, decimal swapFee)
            : base(inner)
        {
            _fromSide = fromSide;
            _toSide = toSide;
            _swapFee = swapFee;
        }

        public override decimal GetPrice() => Inner.GetPrice() + _swapFee;

        public override string GetDescription() => $"{Inner.GetDescription()} (swap {_fromSide} → {_toSide})";
    }
}
