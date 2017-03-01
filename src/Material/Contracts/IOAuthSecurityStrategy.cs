namespace Material.Contracts
{
    public interface IOAuthSecurityStrategy
    {
        /// <summary>
        /// Gets a current parameter or creates one if one does not exist
        /// </summary>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="userId">The identifier of the user submitting the request</param>
        /// <returns>The existing or newly created parameter</returns>
        string CreateOrGetSecureParameter(
            string userId,
            string parameterName);

        /// <summary>
        /// Sets a secure parameter to a specific value
        /// </summary>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="userId">The identifier of the user submitting the request</param>
        /// <param name="parameterValue">The secure value to set</param>
        void SetSecureParameter(
            string userId,
            string parameterName,
            string parameterValue);

        /// <summary>
        /// Checks given crypto list and determines if given secure parameter is valid
        /// </summary>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="parameterValue">Returned value of the cryptographic parameter</param>
        /// <param name="userId">The identifier of the user submitting the request</param>
        /// <returns>False if the parameter is expired, incorrect, or expected and missing, True otherwise</returns>
        bool IsSecureParameterValid(
            string userId,
            string parameterName,
            string parameterValue);
    }
}
