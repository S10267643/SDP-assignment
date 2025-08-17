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
                Console.WriteLine("4. View Subscribers");
                Console.WriteLine("5. Logout");

                Console.Write("Enter choice: ");
                switch (Console.ReadLine())
                {
                    case "1":

                        restaurant.CreateMenuItem(menuItems, ref menuItemIdCounter);

                        Console.Write("Enter category name: ");
                        string categoryName = Console.ReadLine();
                        restaurant.AddMenuCategory(categoryName);

                        break;
                    case "2":


                        Console.Write("Enter target category name: ");
                        string targetCategory = Console.ReadLine();

                        Console.Write("Enter item name: ");
                        string itemName = Console.ReadLine();

                        Console.Write("Enter item description: ");
                        string description = Console.ReadLine();

                        Console.Write("Enter item price: ");
                        decimal price = decimal.Parse(Console.ReadLine());

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
                Console.WriteLine("1. Place Order");
                Console.WriteLine("2. View Order History");
                Console.WriteLine("3. Subscribe to Restaurant");
                Console.WriteLine("4. Logout");
                Console.Write("Enter choice: ");

                switch (Console.ReadLine())
                {
                    case "1":

                        // Simplified order placement - in real app you'd select restaurant and items
                        var sampleItems = new List<MenuItem>
                {
               //     new MenuItem { Name = "Burger", Price = 9.99m },
               //     new MenuItem { Name = "Fries", Price = 3.99m }
                };
                        customer.PlaceOrder(null, sampleItems); // Pass null restaurant for demo

                        PlaceOrder(customer);

                        break;
                    case "2":
                        customer.ViewOrderHistory();
                        break;
                    case "3":
                        SubscribeToRestaurant(customer);
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

        static void PlaceOrder(Customer customer)
        {
            if (users.Count(u => u is Restaurant) == 0)
            {
                Console.WriteLine("No restaurants available.");
                return;
            }

            Console.WriteLine("\nAvailable Restaurants:");
            foreach (var user in users.Where(u => u is Restaurant))
            {
                if (user is Restaurant restaurant)
                {
                    Console.WriteLine($"{restaurant.UserId}. {restaurant.Name}");
                }
            }

            Console.Write("Select restaurant ID: ");
            int restaurantId = int.Parse(Console.ReadLine());
            var selectedRestaurant = users.FirstOrDefault(u => u.UserId == restaurantId && u is Restaurant) as Restaurant;

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
                string itemName = Console.ReadLine();

                if (itemName.ToLower() == "done")
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

            if (orderItems.Any())
            {
                var order = new Order
                {
                    CustomerName = customer.Name,
                    Items = orderItems
                };

                Console.WriteLine("Select payment method:");
                Console.WriteLine("1. Credit Card");
                Console.WriteLine("2. PayPal");
                Console.WriteLine("3. Cash on Delivery");

                var choice = Console.ReadLine();
                switch (choice)
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
                        Console.WriteLine("Invalid choice, defaulting to Credit Card");
                        order.SetPaymentStrategy(new CreditCardPayment());
                        break;
                }

                order.ProcessPayment();
                customer.AddOrderToHistory(order);
                Console.WriteLine("Order placed successfully!");
            }
            else
            {
                Console.WriteLine("No items selected.");
            }
        }

        static void SubscribeToRestaurant(Customer customer)
        {
            if (users.Count(u => u is Restaurant) == 0)
            {
                Console.WriteLine("No restaurants available.");
                return;
            }

            Console.WriteLine("\nAvailable Restaurants:");
            foreach (var user in users.Where(u => u is Restaurant))
            {
                if (user is Restaurant restaurant)
                {
                    Console.WriteLine($"{restaurant.UserId}. {restaurant.Name}");
                }
            }

            Console.Write("Select restaurant ID to subscribe to: ");
            int restaurantId = int.Parse(Console.ReadLine());
            var selectedRestaurant = users.FirstOrDefault(u => u.UserId == restaurantId && u is Restaurant) as Restaurant;

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