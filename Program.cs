
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
                                Console.WriteLine($"Logged in as customer {customer.Name}");
                            else if (user is Restaurant restaurant)
                                ShowRestaurantMenu(restaurant);
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
                Console.WriteLine("1. Create Menu Item");
                Console.WriteLine("2. Logout");
                Console.Write("Enter choice: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        restaurant.CreateMenuItem(menuItems, ref menuItemIdCounter);
                        break;
                    case "2":
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

