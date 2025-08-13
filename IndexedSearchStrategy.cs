using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class IndexedSearchStrategy : ISearchStrategy
    {
        public List<MenuItem> Search(List<MenuItem> items, SearchCriteria criteria)
        {
            var index = items.GroupBy(i => i.Name.ToLower())
                             .ToDictionary(g => g.Key, g => g.ToList());

            IEnumerable<MenuItem> baseSet = string.IsNullOrWhiteSpace(criteria.QueryText)
                ? items
                : (index.ContainsKey(criteria.QueryText.ToLower())
                    ? index[criteria.QueryText.ToLower()]
                    : items.Where(i => i.Name.Contains(criteria.QueryText, System.StringComparison.OrdinalIgnoreCase)));

            return baseSet.Where(item =>
                (string.IsNullOrWhiteSpace(criteria.Category) || item.Category.Equals(criteria.Category, System.StringComparison.OrdinalIgnoreCase)) &&
                (!criteria.MaxPrice.HasValue || item.Price <= criteria.MaxPrice.Value)
            ).ToList();
        }
    }
}
