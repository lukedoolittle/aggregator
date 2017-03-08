using System;
using System.Threading.Tasks;

namespace Material.Contracts
{
    public interface IOAuthAuthorizationUriFacade
    {
        Task<Uri> GetAuthorizationUriAsync(string requestId);
    }
}
