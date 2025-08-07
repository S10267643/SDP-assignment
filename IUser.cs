namespace SDP_assignment
{
    public interface IUser
    {
        int Id { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool Login(string email, string password);
    }
}