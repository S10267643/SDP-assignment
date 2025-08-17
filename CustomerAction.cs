using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SDP_assignment
{
    public static class CustomerActions
    {
        public static void RunSearch(Customer customer, List<User> users, List<MenuItem> localMenuItems)
        {
            var (catalog, ownerMap) = BuildCatalogAcrossRestaurants(users);

            // fallback to local list if no restaurant data yet
            if (catalog.Count == 0 && localMenuItems != null && localMenuItems.Count > 0)
            {
                catalog = new List<MenuItem>(localMenuItems);
                ownerMap = catalog.ToDictionary(i => i, _ => "(local)");
            }

            if (catalog.Count == 0)
            {
                Console.WriteLine("No menu items found. Ask restaurants to add categories/items first.");
                return;
            }

            Console.WriteLine("\nSelect search strategy (1=Linear, 2=Indexed, 3=Fuzzy): ");
            string s = Console.ReadLine() ?? "";
            ISearchStrategy strategy = s switch
            {
                "2" => new IndexedSearchStrategy(),
                "3" => new FuzzySearchStrategy(),
                _ => new LinearSearchStrategy()
            };

            var criteria = new SearchCriteria
            {
                Query = Prompt("Keyword (matches name; blank = all): "),
                Category = Prompt("Description keyword (blank = any): "),
                MaxPrice = ParseNullableDecimal(Prompt("Max price (blank = any): "))
            };

            var service = new SearchService();
            service.SetStrategy(strategy);
            var results = service.Search(catalog, criteria);

            if (results.Count == 0)
            {
                Console.WriteLine("No results.");
                return;
            }

            Console.WriteLine("\n=== RESULTS ===");
            for (int i = 0; i < results.Count; i++)
            {
                var owner = ownerMap.TryGetValue(results[i], out var rname) ? rname : "(unknown)";
                Console.WriteLine($"{i + 1}. {results[i].Name} — {owner} — {results[i].Price:C}");
            }
        }

        private static (List<MenuItem> Catalog, Dictionary<MenuItem, string> OwnerMap) BuildCatalogAcrossRestaurants(List<User> users)
        {
            var catalog = new List<MenuItem>();
            var ownerMap = new Dictionary<MenuItem, string>();

            foreach (var r in users.OfType<Restaurant>())
            {
                var root = GetRootMenuViaReflection(r);
                if (root == null) continue;

                foreach (var item in TraverseMenu(root))
                {
                    catalog.Add(item);
                    if (!ownerMap.ContainsKey(item))
                        ownerMap[item] = r.Name;
                }
            }
            return (catalog, ownerMap);
        }

        // Looks for rootMenu/RootMenu field or property, or getRootMenu/GetRootMenu method.
        private static object? GetRootMenuViaReflection(Restaurant r)
        {
            var t = r.GetType();

            var f = t.GetField("rootMenu", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null) return f.GetValue(r);

            var p = t.GetProperty("rootMenu", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 ?? t.GetProperty("RootMenu", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null) return p.GetValue(r);

            var m = t.GetMethod("getRootMenu", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 ?? t.GetMethod("GetRootMenu", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (m != null) return m.Invoke(r, null);

            return null;
        }

        // DFS over MenuCategory.Children/children, yielding MenuItem leaves
        private static IEnumerable<MenuItem> TraverseMenu(object node)
        {
            if (node == null) yield break;

            if (node is MenuItem mi)
            {
                yield return mi;
                yield break;
            }

            var type = node.GetType();
            if (type.Name.Contains("MenuCategory"))
            {
                var p = type.GetProperty("children", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                     ?? type.GetProperty("Children", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                var f = type.GetField("children", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                     ?? type.GetField("Children", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                var kidsObj = p?.GetValue(node) ?? f?.GetValue(node);
                if (kidsObj is IEnumerable kids)
                {
                    foreach (var child in kids)
                    {
                        foreach (var leaf in TraverseMenu(child))
                            yield return leaf;
                    }
                }
            }
        }

        private static string Prompt(string label)
        {
            Console.Write(label);
            return Console.ReadLine() ?? "";
        }

        private static decimal? ParseNullableDecimal(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var d)) return d;
            if (decimal.TryParse(s, out d)) return d;
            return null;
        }
    }
}