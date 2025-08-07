using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace SDP_assignment
{
    public enum OfferType { StorewideDiscount, BundleDiscount }

    public class SpecialOffer
    {
        private readonly IOfferStrategy _strategy;

        public int Id { get; set; }
        public string Name { get; set; }
        public OfferType Type { get; set; }
        public bool IsActive { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreatedDate { get; set; }

        public SpecialOffer(IOfferStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
            CreatedDate = DateTime.Now;
            IsActive = true;
        }

        public decimal CalculateDiscount(List<MenuItem> items)
        {
            return IsActive ? _strategy.CalculateDiscount(items) : 0;
        }

        public string GetOfferDescription()
        {
            return _strategy.GetDescription();
        }

        public void DeactivateOffer()
        {
            IsActive = false;
        }

        public override string ToString()
        {
            return $"{Name}: {GetOfferDescription()} ({(IsActive ? "Active" : "Inactive")})";
        }
    }
}