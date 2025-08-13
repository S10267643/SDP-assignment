using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class FuzzySearchStrategy : ISearchStrategy
    {
        public List<MenuItem> Search(List<MenuItem> items, SearchCriteria criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.QueryText))
                return Filter(items, criteria.Category, criteria.MaxPrice);

            int threshold = Math.Max(1, criteria.QueryText.Length / 4);

            return Filter(items.Where(item =>
                Levenshtein(item.Name.ToLower(), criteria.QueryText.ToLower()) <= threshold ||
                item.Name.Contains(criteria.QueryText, StringComparison.OrdinalIgnoreCase)
            ), criteria.Category, criteria.MaxPrice);
        }

        private static List<MenuItem> Filter(IEnumerable<MenuItem> items, string category, decimal? maxPrice)
        {
            return items.Where(item =>
                (string.IsNullOrWhiteSpace(category) || item.Category.Equals(category, StringComparison.OrdinalIgnoreCase)) &&
                (!maxPrice.HasValue || item.Price <= maxPrice.Value)
            ).ToList();
        }

        private static int Levenshtein(string a, string b)
        {
            var dp = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost
                    );
                }
            }
            return dp[a.Length, b.Length];
        }
    }
}
