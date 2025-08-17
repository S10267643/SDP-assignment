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
        public static void PlaceOrderWithCustomisations(Customer customer, List<User> users, List<MenuItem> localMenuItems)
        {
            // 1) Build a flat catalog (across all restaurants) – same traversal we use for search
            var (catalog, ownerMap) = BuildCatalogAcrossRestaurants(users);

            // Fallback to local list if no restaurant menu data yet
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

            // 2) Let the user choose a search strategy to find an item (Strategy pattern)
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
            var matches = service.Search(catalog, criteria);

            if (matches.Count == 0)
            {
                Console.WriteLine("No matching items.");
                return;
            }

            Console.WriteLine("\n=== MATCHES ===");
            for (int i = 0; i < matches.Count; i++)
            {
                var owner = ownerMap.TryGetValue(matches[i], out var rname) ? rname : "(unknown)";
                Console.WriteLine($"{i + 1}. {matches[i].Name} — {owner} — {matches[i].Price:C}");
            }

            int pick = ReadIntInRange($"Select an item (1..{matches.Count}): ", 1, matches.Count) - 1;

            // 3) Decorator pattern: wrap the base item with customisations
            IFoodItem orderItem = matches[pick]; // MenuItem implements IFoodItem

            bool done = false;
            while (!done)
            {
                Console.WriteLine("\nCustomisation:");
                Console.WriteLine("1. Add-on");
                Console.WriteLine("2. Swap side");
                Console.WriteLine("3. Size upgrade");
                Console.WriteLine("4. Proceed to checkout");
                Console.Write("Enter choice: ");

                switch ((Console.ReadLine() ?? "").Trim())
                {
                    case "1":
                        var add = Prompt("Add-on name: ");
                        var addDelta = ReadDecimal("Price delta (e.g., 0.50): ");
                        orderItem = new AddOnDecorator(orderItem, add, addDelta);
                        Console.WriteLine($"Added {add}. Current total: {orderItem.GetPrice():C}");
                        break;

                    case "2":
                        var from = Prompt("Replace side (from): ");
                        var to = Prompt("Replace with (to): ");
                        var fee = ReadDecimal("Swap fee (can be negative): ");
                        orderItem = new SideSwapDecorator(orderItem, from, to, fee);
                        Console.WriteLine($"Swapped {from}→{to}. Current total: {orderItem.GetPrice():C}");
                        break;

                    case "3":
                        var size = Prompt("Size label: ");
                        var delta = ReadDecimal("Price delta: ");
                        orderItem = new SizeUpgradeDecorator(orderItem, size, delta);
                        Console.WriteLine($"Upgraded to {size}. Current total: {orderItem.GetPrice():C}");
                        break;

                    case "4":
                        done = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }

            // 4) Checkout summary (hook into your Order aggregate later)
            Console.WriteLine("\n=== CHECKOUT SUMMARY ===");
            Console.WriteLine(orderItem.GetDescription());
            Console.WriteLine($"Total: {orderItem.GetPrice():C}");
        }

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
        private static int ReadIntInRange(string label, int min, int max)
        {
            while (true)
            {
                Console.Write(label);
                var s = Console.ReadLine();
                if (int.TryParse(s, out var v) && v >= min && v <= max) return v;
                Console.WriteLine($"Enter an integer between {min} and {max}.");
            }
        }

        private static decimal ReadDecimal(string label)
        {
            while (true)
            {
                Console.Write(label);
                var s = Console.ReadLine();
                if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var d)) return d;
                if (decimal.TryParse(s, out d)) return d; // fallback to current culture
                Console.WriteLine("Enter a valid number, e.g., 4.50");
            }
        }
    }
}