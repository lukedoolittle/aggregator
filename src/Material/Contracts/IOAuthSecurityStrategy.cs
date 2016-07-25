namespace Material.Contracts
{
    public interface IOAuthSecurityStrategy
    {
        string CreateOrGetSecureParameter(
            string userId,
            string parameterName);

        bool IsSecureParameterValid(
            string userId,
            string parameterName,
            string parameterValue);
    }
}
