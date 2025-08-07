using System.Collections.Generic;

namespace SDP_assignment
{
    public interface IOfferStrategy
    {
        decimal CalculateDiscount(List<MenuItem> items);
        string GetDescription();
    }
}