namespace SDP_assignment
{
    public abstract class User
    {
        public int UserId { get; set; }  // Specific ID name
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual bool Login(string email, string password)
        {
            return Email.Equals(email, StringComparison.OrdinalIgnoreCase) && Password == password;
        }
    }

}