using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class Restaurant : User
    {
        public void CreateMenuItem(List<MenuItem> menuItems, ref int idCounter)
        {
            Console.WriteLine("\n=== CREATE NEW MENU ITEM ===");
            var builder = new MenuItemBuilder();

            try
            {
                string name;
                do
                {
                    Console.Write("Enter item name (min 2 chars): ");
                    name = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(name) || name.Length < 2);

                Console.Write("Enter description: ");
                string description = Console.ReadLine();

                decimal price;
                do
                {
                    Console.Write("Enter price: $");
                } while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0);

                Console.Write("Enter category: ");
                string category = Console.ReadLine();

                var item = builder
                    .SetId(idCounter++)
                    .SetName(name)
                    .SetDescription(description)
                    .SetPrice(price)
                    .SetCategory(category)
                    .SetRestaurantId(this.Id)
                    .Build();

                menuItems.Add(item);
                Console.WriteLine($"Menu item '{name}' created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ViewMyMenuItems(List<MenuItem> menuItems)
        {
            var myItems = menuItems.Where(m => m.RestaurantId == Id).ToList();
            if (!myItems.Any())
            {
                Console.WriteLine("You have no menu items yet.");
                return;
            }

            Console.WriteLine($"\n=== YOUR MENU ITEMS ===");
            foreach (var item in myItems)
            {
                Console.WriteLine($"- {item}");
            }
        }

        public void ManageSpecialOffers(OfferManager offerManager)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== MANAGE SPECIAL OFFERS ===");
                Console.WriteLine("1. Create Store-wide Discount");
                Console.WriteLine("2. Create Bundle Deal");
                Console.WriteLine("3. View All Offers");
                Console.WriteLine("4. Stop Current Offer");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CreateStorewideOffer(offerManager);
                        break;
                    case "2":
                        CreateBundleOffer(offerManager);
                        break;
                    case "3":
                        ViewAllOffers(offerManager);
                        break;
                    case "4":
                        StopCurrentOffer(offerManager);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void CreateStorewideOffer(OfferManager offerManager)
        {
            Console.WriteLine("\n=== CREATE STORE-WIDE DISCOUNT ===");
            Console.Write("Enter offer name: ");
            string name = Console.ReadLine();

            decimal discount;
            do
            {
                Console.Write("Enter discount percentage (1-99): ");
            } while (!decimal.TryParse(Console.ReadLine(), out discount) || discount < 1 || discount > 99);

            var offer = offerManager.CreateStorewideOffer(Id, name, discount);
            Console.WriteLine($"Offer created: {offer.GetOfferDescription()}");
        }

        private void CreateBundleOffer(OfferManager offerManager)
        {
            Console.WriteLine("\n=== CREATE BUNDLE DEAL ===");
            Console.Write("Enter offer name: ");
            string name = Console.ReadLine();

            int minItems;
            do
            {
                Console.Write("Enter minimum items for discount (2+): ");
            } while (!int.TryParse(Console.ReadLine(), out minItems) || minItems < 2);

            decimal discount;
            do
            {
                Console.Write("Enter discount percentage (1-99): ");
            } while (!decimal.TryParse(Console.ReadLine(), out discount) || discount < 1 || discount > 99);

            var offer = offerManager.CreateBundleOffer(Id, name, minItems, discount);
            Console.WriteLine($"Offer created: {offer.GetOfferDescription()}");
        }

        private void ViewAllOffers(OfferManager offerManager)
        {
            var offers = offerManager.GetAllOffersForRestaurant(Id);
            if (!offers.Any())
            {
                Console.WriteLine("You have no offers yet.");
                return;
            }

            Console.WriteLine("\n=== YOUR OFFERS ===");
            foreach (var offer in offers)
            {
                Console.WriteLine($"- {offer}");
            }
        }

        private void StopCurrentOffer(OfferManager offerManager)
        {
            var activeOffer = offerManager.GetActiveOfferForRestaurant(Id);
            if (activeOffer == null)
            {
                Console.WriteLine("You don't have any active offers.");
                return;
            }

            Console.WriteLine($"Current active offer: {activeOffer.GetOfferDescription()}");
            Console.Write("Are you sure you want to stop this offer? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                offerManager.DeactivateOffer(Id);
                Console.WriteLine("Offer has been stopped.");
            }
        }
    }
}