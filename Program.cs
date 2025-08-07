
﻿using System;
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
            Console.WriteLine("Your favorite food delivery service!\n");

            IUserFactory factory = new UserFactory();
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
                        CreateAccount(factory);
                        break;
                    case "2":
                        User user = Login();
                        if (user != null)
                        {
                            if (user is Customer customer)
                                ShowCustomerMenu(customer);
                            else if (user is Restaurant restaurant)
                                ShowRestaurantMenu(restaurant);
                        }
                        break;
                    case "3":
                        exit = true;
                        Console.WriteLine("Thank you for using Grabberoo! Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void CreateAccount(IUserFactory factory)
        {
            Console.WriteLine("\n=== CREATE ACCOUNT ===");

            string userType;
            do
            {
                Console.Write("Enter user type (customer/restaurant): ");
                userType = Console.ReadLine().ToLower();
            } while (userType != "customer" && userType != "restaurant");

            try
            {
                User newUser = factory.CreateUser(userType);
                newUser.Id = userIdCounter++;

                Console.Write("Enter your name: ");
                newUser.Name = Console.ReadLine();

                string email;
                do
                {
                    Console.Write("Enter email: ");
                    email = Console.ReadLine();
                    if (users.Exists(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                        Console.WriteLine("Email already exists. Please try another.");
                } while (string.IsNullOrWhiteSpace(email) || users.Exists(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
                newUser.Email = email;

                string password;
                do
                {
                    Console.Write("Enter password (min 6 characters): ");
                    password = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(password) || password.Length < 6);
                newUser.Password = password;

                users.Add(newUser);
                Console.WriteLine($"\nAccount created successfully! Your ID is: {newUser.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating account: {ex.Message}");
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
                Console.WriteLine("Login failed. Invalid email or password.");
                return null;
            }

            Console.WriteLine($"\nWelcome back, {user.Name}!");
            return user;
        }

        static void ShowCustomerMenu(Customer customer)
        {
            bool logout = false;
            while (!logout)
            {
                Console.WriteLine("\n=== CUSTOMER MENU ===");
                Console.WriteLine("1. View Restaurants");
                Console.WriteLine("2. View All Menu Items");
                Console.WriteLine("3. View Menu by Restaurant");
                Console.WriteLine("4. Logout");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        customer.ViewRestaurants(users);
                        break;
                    case "2":
                        customer.ViewMenuItems(menuItems);
                        break;
                    case "3":
                        ViewMenuByRestaurant(customer);
                        break;
                    case "4":
                        logout = true;
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void ShowRestaurantMenu(Restaurant restaurant)
        {
            bool logout = false;
            while (!logout)
            {
                Console.WriteLine($"\n=== RESTAURANT MENU - {restaurant.Name} ===");
                Console.WriteLine("1. View My Menu Items");
                Console.WriteLine("2. Add New Menu Item");
                Console.WriteLine("3. Manage Special Offers");
                Console.WriteLine("4. Logout");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        restaurant.ViewMyMenuItems(menuItems);
                        break;
                    case "2":
                        restaurant.CreateMenuItem(menuItems, ref menuItemIdCounter);
                        break;
                    case "3":
                      //  call wtv special order does here 
                        break;
                    case "4":
                        logout = true;
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void ViewMenuByRestaurant(Customer customer)
        {
            customer.ViewRestaurants(users);
            var restaurants = users.OfType<Restaurant>().ToList();

            if (!restaurants.Any())
            {
                Console.WriteLine("No restaurants available.");
                return;
            }

            Console.Write("\nEnter restaurant ID to view menu: ");
            if (int.TryParse(Console.ReadLine(), out int restaurantId))
            {
                var restaurant = restaurants.FirstOrDefault(r => r.Id == restaurantId);
                if (restaurant != null)
                {
                    Console.WriteLine($"\n=== {restaurant.Name}'s MENU ===");
                    customer.ViewMenuItems(menuItems.Where(m => m.RestaurantId == restaurantId).ToList());
                }
                else
                {
                    Console.WriteLine("Restaurant not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }
}

