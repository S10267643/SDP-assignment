using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public sealed class AddOnDecorator : FoodDecorator
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
