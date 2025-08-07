namespace SDP_assignment
{
    public class UserFactory : IUserFactory
    {
        public User CreateUser(string userType)
        {
            return userType.ToLower() switch
            {
                "customer" => new Customer(),
                "restaurant" => new Restaurant(),
                _ => throw new ArgumentException("Invalid user type. Must be 'customer' or 'restaurant'.")
            };
        }
    }
}