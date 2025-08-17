using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public sealed class SizeUpgradeDecorator : FoodDecorator
    {
        private readonly string _size;
        private readonly decimal _delta;

        public SizeUpgradeDecorator(IFoodItem inner, string size, decimal priceDelta)
            : base(inner)
        {
            _size = size;
            _delta = priceDelta;
        }

        public override decimal GetPrice() => Inner.GetPrice() + _delta;
        public override string GetDescription() => $"{Inner.GetDescription()} [{_size}]";
    }
}
