namespace SDP_assignment
{
    public abstract class User : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual bool Login(string email, string password)
        {
            return Email.Equals(email, StringComparison.OrdinalIgnoreCase) && Password == password;
        }
    }
}