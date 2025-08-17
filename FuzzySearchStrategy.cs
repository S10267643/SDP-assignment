using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public sealed class FuzzySearchStrategy : ISearchStrategy
    {
        public List<MenuItem> Search(List<MenuItem> items, SearchCriteria c)
        {
            string q = (c?.Query ?? "").Trim().ToLowerInvariant();
            string key = (c?.Category ?? "").Trim().ToLowerInvariant(); // description keyword
            decimal? max = c?.MaxPrice;

            if (q.Length == 0)
                return Filter(items, key, max);

            int threshold = Math.Max(1, q.Length / 4);

            return Filter(items.Where(i =>
            {
                string name = i.Name.ToLowerInvariant();
                return name.Contains(q) || Distance(name, q) <= threshold;
            }), key, max);
        }

        private static List<MenuItem> Filter(IEnumerable<MenuItem> src, string key, decimal? max)
        {
            return src.Where(i =>
                (key.Length == 0 || (i.Description ?? string.Empty).ToLowerInvariant().Contains(key)) &&
                (!max.HasValue || i.Price <= max.Value)
            ).ToList();
        }

        // Levenshtein distance
        private static int Distance(string a, string b)
        {
            var dp = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost
                    );
                }
            return dp[a.Length, b.Length];
        }
    }
}
