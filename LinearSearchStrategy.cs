using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class LinearSearchStrategy : ISearchStrategy
    {
        public List<MenuItem> Search(List<MenuItem> items, SearchCriteria criteria)
        {
            return items.Where(item =>
                (string.IsNullOrWhiteSpace(criteria.QueryText) || item.Name.Contains(criteria.QueryText, System.StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(criteria.Category) || item.Category.Equals(criteria.Category, System.StringComparison.OrdinalIgnoreCase)) &&
                (!criteria.MaxPrice.HasValue || item.Price <= criteria.MaxPrice.Value)
            ).ToList();
        }
    }
}
