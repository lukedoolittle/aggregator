namespace Quantfabric.Test.Helpers
{
    public class UsernameAndPassword
    {
        public UsernameAndPassword(
            string username, 
            string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }
        public string Password { get; }

    }
}
