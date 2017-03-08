namespace Material.Contracts
{
    public interface IOAuthSecurityStrategy
    {
        /// <summary>
        /// Gets a current parameter or throws an exception if it does not exist
        /// </summary>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="requestId">The unique identifier of the request being made</param>
        /// <returns>The existing created parameter</returns>
        string GetSecureParameter(
            string requestId,
            string parameterName);

        /// <summary>
        /// Creates a new parameter
        /// </summary>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="requestId">The unique identifier of the request being made</param>
        /// <returns>The existing or newly created parameter</returns>
        string CreateSecureParameter(
            string requestId,
            string parameterName);

        /// <summary>
        /// Sets a secure parameter to a specific value
        /// </summary>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="requestId">The unique identifier of the request being made</param>
        /// <param name="parameterValue">The secure value to set</param>
        void SetSecureParameter(
            string requestId,
            string parameterName,
            string parameterValue);

        /// <summary>
        /// Checks given crypto list and determines if given secure parameter is valid
        /// </summary>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="parameterValue">Returned value of the cryptographic parameter</param>
        /// <param name="requestId">The unique identifier of the request being made</param>
        /// <returns>False if the parameter is expired, incorrect, or expected and missing, True otherwise</returns>
        bool IsSecureParameterValid(
            string requestId,
            string parameterName,
            string parameterValue);

        /// <summary>
        /// Removes all the security parameters for the given request
        /// </summary>
        /// <param name="requestId">The unique identifier of the request being made</param>
        void ClearSecureParameters(string requestId);
    }
}
