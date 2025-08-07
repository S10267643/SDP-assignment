using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP_assignment
{
    public class Restaurant : User
    {
        public List<MenuItem> MyMenuItems { get; private set; }

        public Restaurant()
        {
            MyMenuItems = new List<MenuItem>();
        }

        public void CreateMenuItem(List<MenuItem> globalMenuItems, ref int menuItemIdCounter)
        {
            var builder = new MenuItemBuilder().SetRestaurantId(this.UserId);
            var item = builder.BuildFromInput(menuItemIdCounter);

            if (item != null)
            {
                menuItemIdCounter++;
                globalMenuItems.Add(item);
                MyMenuItems.Add(item);
                Console.WriteLine($"Menu item '{item.Name}' created successfully!");
            }
        }

        // Stubs
        public void UpdateMenuItem() => Console.WriteLine("Updating menu item...");
        public void DeleteMenuItem() => Console.WriteLine("Deleting menu item...");
        public void CreateSpecialOffer() => Console.WriteLine("Creating special offer...");
        public void AcceptOrder() => Console.WriteLine("Accepting order...");
        public void RejectOrder() => Console.WriteLine("Rejecting order...");
        public void StoreDeliveredOrder() => Console.WriteLine("Storing delivered order...");
    }

}