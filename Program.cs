
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
                Console.Write("Enter choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        // Simplified order placement - in real app you'd select restaurant and items
                        var sampleItems = new List<MenuItem>
                        {
                        new MenuItem("Burger", "Beef burger", 9.99m, 0),
                        new MenuItem("Fries",  "Crispy fries", 3.99m, 0)
                        };
                        //   customer.PlaceOrder(null, sampleItems); // Pass null restaurant for demo
                        break;
                    case "2":
                     //   customer.ViewOrderHistory();
                        break;
                    case "3":
                        logout = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }

}

