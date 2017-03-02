using System;

namespace Material.Contracts
{
    public interface ICryptographicParameterRepository
    {
        /// <summary>
        /// Adds a crypto parameter to the repository by the composite key of [userId, parameterName]
        /// </summary>
        /// <param name="userId">The userId portion of the composite key</param>
        /// <param name="parameterName">The parameter name portion of the composite key</param>
        /// <param name="parameterValue">The value of the parameter to insert</param>
        /// <param name="timestamp">The timestamp associated with the entry</param>
        /// <returns>True if the insert suceeded, false otherwise</returns>
        bool TryInsertCryptographicParameterValue(
            string userId, 
            string parameterName, 
            string parameterValue,
            DateTimeOffset timestamp);

        /// <summary>
        /// Gets a crypto entry from the repository by the composite key of [userId, parameterName]
        /// </summary>
        /// <param name="userId">The userId portion of the composite key</param>
        /// <param name="parameterName">The parameter name portion of the composite key</param>
        /// <returns>The value if it exists in the repository, otherwise null</returns>
        Tuple<string, DateTimeOffset> GetCryptographicParameterValue(
            string userId,
            string parameterName);

        /// <summary>
        /// Removes a crypto parameter from the repository by the composite key of [userId, parameterName]
        /// </summary>
        /// <param name="userId">The userId portion of the composite key</param>
        /// <param name="parameterName">The parameter name portion of the composite key</param>
        void DeleteCryptographicParameterValue(
            string userId, 
            string parameterName);

        /// <summary>
        /// Removes all crypto parameters from the repository by the primary key of userId
        /// </summary>
        /// <param name="userId">The primary key</param>
        void DeleteCryptographicParameterValues(string userId);
    }
}
