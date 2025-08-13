
using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    class Program
    {
        static List<User> users = new List<User>();
        static List<MenuItem> menuItems = new List<MenuItem>();
        static int userIdCounter = 1;
        static int menuItemIdCounter = 1;

        static void Main(string[] args)
        {
            Console.WriteLine("=== WELCOME TO GRABBEROO ===");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CreateAccount();
                        break;
                    case "2":
                        User user = Login();
                        if (user != null)
                        {
                            if (user is Customer customer)
                            {
                                Console.WriteLine($"Logged in as customer {customer.Name}");
                                CustomerMenu(customer);
                            }
                            else if (user is Restaurant restaurant)
                            {
                                ShowRestaurantMenu(restaurant);
                            }
                        }
                        break;
                    case "3":
                        exit = true;
                        Console.WriteLine("Thank you for using Grabberoo!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void CreateAccount()
        {
            Console.WriteLine("\n=== CREATE ACCOUNT ===");
            UserFactory factory = null;

            while (factory == null)
            {
                Console.Write("Enter user type (customer/restaurant): ");
                string input = Console.ReadLine()?.ToLower();

                if (input == "customer")
                    factory = new CustomerCreator();
                else if (input == "restaurant")
                    factory = new RestaurantCreator();
                else
                    Console.WriteLine("Invalid type. Try again.");
            }

            User newUser = factory.RegisterUser();
            newUser.UserId = userIdCounter++;
            users.Add(newUser);
            Console.WriteLine($"Account created successfully! Your ID is: {newUser.UserId}");

            if (newUser is Customer customer)
            {
                Console.WriteLine($"Logged in as customer {customer.Name}");
                CustomerMenu(customer);
            }
            else if (newUser is Restaurant restaurant)
            {
                ShowRestaurantMenu(restaurant);
            }
        }

        static User Login()
        {
            Console.WriteLine("\n=== LOGIN ===");
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            User user = users.Find(u => u.Login(email, password));
            if (user == null)
            {
                Console.WriteLine("Login failed.");
                return null;
            }

            Console.WriteLine($"Welcome back, {user.Name}!");
            return user;
        }

        static void ShowRestaurantMenu(Restaurant restaurant)
        {
            bool logout = false;
            while (!logout)
            {
                Console.WriteLine($"\n=== RESTAURANT MENU - {restaurant.Name} ===");
                Console.WriteLine("1. Add Category");
                Console.WriteLine("2. Add Item to Category");
                Console.WriteLine("3. View Full Menu");
                Console.WriteLine("4. Logout");
                Console.Write("Enter choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter category name: ");
                        restaurant.AddMenuCategory(Console.ReadLine());
                        break;

                    case "2":
                        Console.Write("Enter target category name: ");
                        restaurant.AddMenuItemToCategory(Console.ReadLine());
                        break;

                    case "3":
                        restaurant.PrintFullMenu();
                        break;

                    case "4":
                        logout = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
        static void CustomerMenu(Customer customer)
        {
            bool logout = false;
            while (!logout)
            {
                Console.WriteLine("\n=== CUSTOMER MENU ===");
                Console.WriteLine("1. Place Order");
                Console.WriteLine("2. View Order History");
                Console.WriteLine("3. Logout");
                Console.WriteLine("4. Try Pattern Demo (Strategy + Decorator)");
                Console.Write("Enter choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        // Simplified order placement - in real app you'd select restaurant and items
                        var sampleItems = new List<MenuItem>
                {
                  //  new MenuItem { Name = "Burger", Price = 9.99m },
                   // new MenuItem { Name = "Fries", Price = 3.99m }
                };
                     //   customer.PlaceOrder(null, sampleItems); // Pass null restaurant for demo
                        break;
                    case "2":
                     //   customer.ViewOrderHistory();
                        break;
                    case "3":
                        logout = true;
                        break;
                    case "4": // ADD — interactive Strategy + Decorator
                        PatternDemo();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            static void PatternDemo()
            {
                if (menuItems == null || menuItems.Count == 0)
                {
                    Console.WriteLine("\nNo menu items available. Ask a restaurant to add some first.");
                    return;
                }

                Console.WriteLine("\n=== STRATEGY — Choose search method ===");
                Console.WriteLine("1. Linear");
                Console.WriteLine("2. Indexed");
                Console.WriteLine("3. Fuzzy");
                Console.Write("Choice: ");
                ISearchStrategy strategy = (Console.ReadLine() ?? "") switch
                {
                    "2" => new IndexedSearchStrategy(),
                    "3" => new FuzzySearchStrategy(),
                    _ => new LinearSearchStrategy()
                };

                var criteria = new SearchCriteria
                {
                    QueryText = Prompt("Query text (blank = all): "),
                    Category = Prompt("Category filter (blank = any): "),
                    MaxPrice = ParseNullableDecimal(Prompt("Max price (blank = any): "))
                };

                var service = new SearchService();
                service.SetStrategy(strategy);

                var results = service.Search(menuItems, criteria);
                if (results.Count == 0)
                {
                    Console.WriteLine("No results. Try different criteria.");
                    return;
                }

                Console.WriteLine("\n=== RESULTS ===");
                for (int i = 0; i < results.Count; i++)
                {
                    var priceVal = results[i].Price;
                    Console.WriteLine($"{i + 1}. {results[i].Name} - {priceVal:C}");
                }

                int idx = ReadIntInRange($"Select an item (1..{results.Count}): ", 1, results.Count) - 1;

                // Wrap selected MenuItem as IFoodItem for decorators
                IFoodItem order = results[idx];

                bool done = false;
                while (!done)
                {
                    Console.WriteLine("\n=== DECORATOR — Customise ===");
                    Console.WriteLine("1. Add-on");
                    Console.WriteLine("2. Swap side");
                    Console.WriteLine("3. Size upgrade");
                    Console.WriteLine("4. Finish");
                    Console.Write("Choice: ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            var add = Prompt("Add-on name: ");
                            var addDelta = ReadDecimal("Price delta: ");
                            order = new AddOnDecorator(order, add, addDelta);
                            Console.WriteLine($"→ {order.GetDescription()}  |  {order.GetPrice():C}");
                            break;

                        case "2":
                            var from = Prompt("From side: ");
                            var to = Prompt("To side: ");
                            var fee = ReadDecimal("Swap fee: ");
                            order = new SideSwapDecorator(order, from, to, fee);
                            Console.WriteLine($"→ {order.GetDescription()}  |  {order.GetPrice():C}");
                            break;

                        case "3":
                            var size = Prompt("Size label: ");
                            var delta = ReadDecimal("Price delta: ");
                            order = new SizeUpgradeDecorator(order, size, delta);
                            Console.WriteLine($"→ {order.GetDescription()}  |  {order.GetPrice():C}");
                            break;

                        case "4":
                            done = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }

                Console.WriteLine("\n=== CHECKOUT SUMMARY ===");
                Console.WriteLine(order.GetDescription());
                Console.WriteLine($"Total: {order.GetPrice():C}");
            }

            static string Prompt(string label)
            {
                Console.Write(label);
                return Console.ReadLine() ?? "";
            }

            static int ReadIntInRange(string label, int min, int max)
            {
                while (true)
                {
                    Console.Write(label);
                    var s = Console.ReadLine();
                    if (int.TryParse(s, out var v) && v >= min && v <= max) return v;
                    Console.WriteLine($"Enter an integer between {min} and {max}.");
                }
            }

            static decimal ReadDecimal(string label)
            {
                while (true)
                {
                    Console.Write(label);
                    var s = Console.ReadLine();
                    if (decimal.TryParse(s, out var d)) return d;
                    Console.WriteLine("Enter a valid number.");
                }
            }

            static decimal? ParseNullableDecimal(string s)
            {
                if (string.IsNullOrWhiteSpace(s)) return null;
                if (decimal.TryParse(s, out var d)) return d;
                return null;
            }
        }
    }

}

