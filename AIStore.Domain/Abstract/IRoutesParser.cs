using AIStore.Domain.Models.Routing;

namespace AIStore.Domain.Abstract
{
    public interface IRoutesParser
    {
        List<RoutesSet> ParseAIStores();
    }
}
