using AIStore.Domain.Models.Routing;

namespace AIStore.Domain.Abstract.Services
{
    public interface IRoutesParser
    {
        List<RoutesSet> ParseAIStores();
    }
}
