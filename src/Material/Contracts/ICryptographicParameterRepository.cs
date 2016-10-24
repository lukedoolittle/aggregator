using System;

namespace Material.Contracts
{
    public interface ICryptographicParameterRepository
    {
        void SetCryptographicParameterValue(
            string userId, 
            string parameterName, 
            string parameterValue,
            DateTimeOffset timestamp);

        Tuple<string, DateTimeOffset> GetCryptographicParameterValue(
            string userId,
            string parameterName);

        void DeleteCryptographicParameterValue(
            string userId, 
            string parameterName);
    }
}
