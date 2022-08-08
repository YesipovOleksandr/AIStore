using AIStore.Domain.Models.Routing;

namespace AIStore.Domain.Abstract.Services
{
    public interface IApiRoutesParser
    {
        List<RoutesSet> ParseAIStores();
    }
}
