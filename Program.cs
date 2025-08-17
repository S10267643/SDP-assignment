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
                string input = (Console.ReadLine() ?? "").ToLowerInvariant();

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
            string email = Console.ReadLine() ?? "";
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

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
                Console.WriteLine("4. View Subscribers");
                Console.WriteLine("5. Logout");
                Console.Write("Enter choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter category name: ");
                        string categoryName = Console.ReadLine() ?? "";
                        restaurant.AddMenuCategory(categoryName);
                        break;

                    case "2":
                        Console.Write("Enter target category name: ");
                        string targetCategory = Console.ReadLine() ?? "";

                        Console.Write("Enter item name: ");
                        string itemName = Console.ReadLine() ?? "";

                        Console.Write("Enter item description: ");
                        string description = Console.ReadLine() ?? "";

                        Console.Write("Enter item price: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                        {
                            Console.WriteLine("Invalid price.");
                            break;
                        }

                        MenuItem newItem = new MenuItem(itemName, description, price, restaurant.UserId);
                        restaurant.AddMenuItemToCategory(targetCategory, newItem);
                        break;

                    case "3":
                        restaurant.PrintFullMenu();
                        break;

                    case "4":
                        restaurant.PrintSubscribers();
                        break;

                    case "5":
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
                Console.WriteLine("1. Search Menu Items");
                Console.WriteLine("2. Place Order");
                Console.WriteLine("3. View Order History");
                Console.WriteLine("4. Subscribe to Restaurant");
                Console.WriteLine("5. Logout");
                Console.Write("Enter choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CustomerActions.RunSearch(customer, users, menuItems);
                        break;

                    case "2":
                        PlaceOrder(customer);
                        break;

                    case "3":
                        customer.ViewOrderHistory();
                        break;

                    case "4":
                        SubscribeToRestaurant(customer);
                        break;

                    case "5":
                        logout = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void PlaceOrder(Customer customer)
        {
            var restaurants = users.OfType<Restaurant>().ToList();
            if (restaurants.Count == 0)
            {
                Console.WriteLine("No restaurants available.");
                return;
            }

            Console.WriteLine("\nAvailable Restaurants:");
            foreach (var r in restaurants)
            {
                Console.WriteLine($"{r.UserId}. {r.Name}");
            }

            Console.Write("Select restaurant ID: ");
            if (!int.TryParse(Console.ReadLine(), out int restaurantId))
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            var selectedRestaurant = restaurants.FirstOrDefault(r => r.UserId == restaurantId);
            if (selectedRestaurant == null)
            {
                Console.WriteLine("Invalid restaurant selection.");
                return;
            }

            selectedRestaurant.PrintFullMenu();
            var orderItems = new List<MenuItem>();

            bool addingItems = true;
            while (addingItems)
            {
                Console.Write("Enter item name to add to order (or 'done' to finish): ");
                string itemName = (Console.ReadLine() ?? "").Trim();

                if (itemName.Equals("done", StringComparison.OrdinalIgnoreCase))
                {
                    addingItems = false;
                    continue;
                }

                var menuItem = selectedRestaurant.FindMenuItem(itemName);
                if (menuItem != null)
                {
                    orderItems.Add(menuItem);
                    Console.WriteLine($"{menuItem.Name} added to order.");
                }
                else
                {
                    Console.WriteLine("Item not found.");
                }
            }

            if (!orderItems.Any())
            {
                Console.WriteLine("No items selected.");
                return;
            }

            var order = new Order
            {
                CustomerName = customer.Name,
                Items = orderItems
            };

            Console.WriteLine("\nSelect payment method:");
            Console.WriteLine("1. Credit Card");
            Console.WriteLine("2. PayPal");
            Console.WriteLine("3. Cash on Delivery");
            Console.Write("Enter choice: ");

            switch (Console.ReadLine())
            {
                case "1":
                    order.SetPaymentStrategy(new CreditCardPayment());
                    break;
                case "2":
                    order.SetPaymentStrategy(new PayPalPayment());
                    break;
                case "3":
                    order.SetPaymentStrategy(new CashOnDeliveryPayment());
                    break;
                default:
                    Console.WriteLine("Invalid choice, defaulting to Credit Card.");
                    order.SetPaymentStrategy(new CreditCardPayment());
                    break;
            }

            order.ProcessPayment();
            customer.AddOrderToHistory(order);
            Console.WriteLine($"Order placed successfully! Total: {order.TotalAmount:C}");
        }

        static void SubscribeToRestaurant(Customer customer)
        {
            var restaurants = users.OfType<Restaurant>().ToList();
            if (restaurants.Count == 0)
            {
                Console.WriteLine("No restaurants available.");
                return;
            }

            Console.WriteLine("\nAvailable Restaurants:");
            foreach (var r in restaurants)
            {
                Console.WriteLine($"{r.UserId}. {r.Name}");
            }

            Console.Write("Select restaurant ID to subscribe to: ");
            if (!int.TryParse(Console.ReadLine(), out int restaurantId))
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            var selectedRestaurant = restaurants.FirstOrDefault(r => r.UserId == restaurantId);
            if (selectedRestaurant != null)
            {
                selectedRestaurant.Attach(customer);
                Console.WriteLine($"Subscribed to {selectedRestaurant.Name} successfully!");
            }
            else
            {
                Console.WriteLine("Invalid restaurant selection.");
            }
        }
    }
}