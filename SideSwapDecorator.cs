using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public sealed class SideSwapDecorator : FoodDecorator
    {
        private readonly string _from;
        private readonly string _to;
        private readonly decimal _fee;

        public SideSwapDecorator(IFoodItem inner, string fromSide, string toSide, decimal swapFee)
            : base(inner)
        {
            _from = fromSide;
            _to = toSide;
            _fee = swapFee;
        }

        public override decimal GetPrice() => Inner.GetPrice() + _fee;
        public override string GetDescription() => $"{Inner.GetDescription()} (swap {_from}→{_to})";
    }
}
