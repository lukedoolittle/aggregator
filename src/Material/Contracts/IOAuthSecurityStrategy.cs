namespace Material.Contracts
{
    public interface IOAuthSecurityStrategy
    {
        string CreateOrGetSecureParameter(
            string userId,
            string parameterName);

        void SetSecureParameter(
            string userId,
            string parameterName,
            string parameterValue);

        bool IsSecureParameterValid(
            string userId,
            string parameterName,
            string parameterValue);
    }
}
