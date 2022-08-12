using Microsoft.AspNetCore.Routing;

namespace AIStore.Domain.Abstract
{
    public interface IRoutesConfigurator
    {
        void BuildRoutesUsingAIStores(IEndpointRouteBuilder builder);
    }
}
