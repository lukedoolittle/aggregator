using Material.Infrastructure;

namespace Material.Contracts
{
    public interface IClientFactory
    {
        IRequestClient CreateClient<TRequest, TCredentials>(TCredentials credentials)
            where TRequest : Request;

        IRequestClient CreateClient<TRequest>() where TRequest : Request;
    }
}
