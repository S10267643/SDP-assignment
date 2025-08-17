using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public sealed class IndexedSearchStrategy : ISearchStrategy
    {
        public List<MenuItem> Search(List<MenuItem> items, SearchCriteria c)
        {
            var index = items
                .GroupBy(i => i.Name.ToLowerInvariant())
                .ToDictionary(g => g.Key, g => g.ToList());

            string q = (c?.Query ?? "").Trim().ToLowerInvariant();
            string key = (c?.Category ?? "").Trim().ToLowerInvariant(); // description keyword
            decimal? max = c?.MaxPrice;

            IEnumerable<MenuItem> baseSet = q.Length == 0
                ? items
                : (index.TryGetValue(q, out var exact) ? exact
                   : items.Where(i => i.Name.ToLowerInvariant().Contains(q)));

            return baseSet.Where(i =>
                (key.Length == 0 || (i.Description ?? string.Empty).ToLowerInvariant().Contains(key)) &&
                (!max.HasValue || i.Price <= max.Value)
            ).ToList();
        }
    }
}
