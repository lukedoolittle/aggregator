namespace Pacman.Infrastructure.Credentials
{
    public abstract class TokenCredentials
    {
        public abstract bool AreCredentialsExpired { get; }
    }
}
