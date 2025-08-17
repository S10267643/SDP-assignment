using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public sealed class LinearSearchStrategy : ISearchStrategy
    {
        public List<MenuItem> Search(List<MenuItem> items, SearchCriteria c)
        {
            string q = (c?.Query ?? "").Trim().ToLowerInvariant();
            string key = (c?.Category ?? "").Trim().ToLowerInvariant(); // description keyword
            decimal? max = c?.MaxPrice;

            return items.Where(i =>
                (q.Length == 0 || i.Name.ToLowerInvariant().Contains(q)) &&
                (key.Length == 0 || (i.Description ?? string.Empty).ToLowerInvariant().Contains(key)) &&
                (!max.HasValue || i.Price <= max.Value)
            ).ToList();
        }
    }
}
