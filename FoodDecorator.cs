using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public abstract class FoodDecorator : IFoodItem
    {
        protected readonly IFoodItem Inner;

        protected FoodDecorator(IFoodItem inner)
        {
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public abstract decimal GetPrice();
        public abstract string GetDescription();
    }
}
