namespace SDP_assignment
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int RestaurantId { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Description} (${Price:F2}) [{Category}]";
        }
    }

}