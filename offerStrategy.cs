using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class StorewideDiscountStrategy : IOfferStrategy
    {
        private readonly decimal _discountPercent;

        public StorewideDiscountStrategy(decimal discountPercent)
        {
            if (discountPercent < 1 || discountPercent > 99)
                throw new ArgumentOutOfRangeException("Discount must be between 1-99%");
            _discountPercent = discountPercent;
        }

        public decimal CalculateDiscount(List<MenuItem> items)
        {
            return items.Sum(i => i.Price) * (_discountPercent / 100);
        }

        public string GetDescription()
        {
            return $"Store-wide {_discountPercent}% discount";
        }
    }

    public class BundleDiscountStrategy : IOfferStrategy
    {
        private readonly int _minItems;
        private readonly decimal _discountPercent;

        public BundleDiscountStrategy(int minItems, decimal discountPercent)
        {
            if (minItems < 2) throw new ArgumentException("Minimum items must be at least 2");
            if (discountPercent < 1 || discountPercent > 99)
                throw new ArgumentOutOfRangeException("Discount must be between 1-99%");

            _minItems = minItems;
            _discountPercent = discountPercent;
        }

        public decimal CalculateDiscount(List<MenuItem> items)
        {
            return items.Count >= _minItems ? items.Sum(i => i.Price) * (_discountPercent / 100) : 0;
        }

        public string GetDescription()
        {
            return $"{_discountPercent}% off on {_minItems}+ items";
        }
    }
}