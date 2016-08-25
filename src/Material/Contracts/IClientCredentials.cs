using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public enum CallbackTypeEnum
    {
        Localhost,
        Protocol
    }

    public interface IClientCredentials
    {
        TCredentials GetClientCredentials<TService, TCredentials>(
            CallbackTypeEnum callbackType = CallbackTypeEnum.Localhost)
            where TService : ResourceProvider
            where TCredentials : TokenCredentials;
    }
}
