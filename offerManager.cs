using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class OfferManager
    {
        private readonly List<SpecialOffer> _offers = new List<SpecialOffer>();
        private int _offerIdCounter = 1;

        public SpecialOffer CreateStorewideOffer(int restaurantId, string name, decimal discountPercent)
        {
            DeactivateExistingOffer(restaurantId);
            var strategy = new StorewideDiscountStrategy(discountPercent);
            var offer = new SpecialOffer(strategy)
            {
                Id = _offerIdCounter++,
                Name = name,
                Type = OfferType.StorewideDiscount,
                RestaurantId = restaurantId
            };
            _offers.Add(offer);
            return offer;
        }

        public SpecialOffer CreateBundleOffer(int restaurantId, string name, int minItems, decimal discountPercent)
        {
            DeactivateExistingOffer(restaurantId);
            var strategy = new BundleDiscountStrategy(minItems, discountPercent);
            var offer = new SpecialOffer(strategy)
            {
                Id = _offerIdCounter++,
                Name = name,
                Type = OfferType.BundleDiscount,
                RestaurantId = restaurantId
            };
            _offers.Add(offer);
            return offer;
        }

        public bool DeactivateOffer(int restaurantId)
        {
            var offer = GetActiveOfferForRestaurant(restaurantId);
            if (offer == null) return false;

            offer.DeactivateOffer();
            return true;
        }

        public SpecialOffer GetActiveOfferForRestaurant(int restaurantId)
        {
            return _offers.FirstOrDefault(o => o.RestaurantId == restaurantId && o.IsActive);
        }

        public List<SpecialOffer> GetAllOffersForRestaurant(int restaurantId)
        {
            return _offers.Where(o => o.RestaurantId == restaurantId).ToList();
        }

        public List<SpecialOffer> GetAllActiveOffers()
        {
            return _offers.Where(o => o.IsActive).ToList();
        }

        private void DeactivateExistingOffer(int restaurantId)
        {
            var existing = GetActiveOfferForRestaurant(restaurantId);
            existing?.DeactivateOffer();
        }
    }
}    

