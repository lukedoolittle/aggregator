namespace Material.Contracts
{
    public interface IOAuthSecurityStrategy
    {
        /// <summary>
        /// Gets a current parameter or throws an exception if it does not exist
        /// </summary>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="userId">The identifier of the user submitting the request</param>
        /// <returns>The existing created parameter</returns>
        string GetSecureParameter(
            string userId,
            string parameterName);

        /// <summary>
        /// Creates a new parameter
        /// </summary>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="userId">The identifier of the user submitting the request</param>
        /// <returns>The existing or newly created parameter</returns>
        string CreateSecureParameter(
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

        /// <summary>
        /// Removes all the security parameters for the given user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        void ClearSecureParameters(string userId);
    }
}
