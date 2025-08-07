namespace SDP_assignment
{
    public abstract class UserFactory
    {
        public User RegisterUser()
        {
            User user = CreateUser();

            Console.Write("Enter your name: ");
            user.Name = Console.ReadLine();

            string email;
            do
            {
                Console.Write("Enter email: ");
                email = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(email));
            user.Email = email;

            string password;
            do
            {
                Console.Write("Enter password (min 6 characters): ");
                password = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(password) || password.Length < 6);
            user.Password = password;

            return user;
        }

        protected abstract User CreateUser();
    }

}