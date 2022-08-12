using Newtonsoft.Json;
using AIStore.Domain.Models.Routing;
using AIStore.Domain.Abstract;

namespace AIStore.Domain.Concrete
{
    public class ApiRoutesParser : IApiRoutesParser
    {
        private readonly string _routesPaths;
        public ApiRoutesParser(string routesPaths)
        {
            _routesPaths = routesPaths;
        }
        public List<RoutesSet> ParseAIStores()
        {
            List<RoutesSet> routesCollection = new List<RoutesSet>();

            using (StreamReader reader = new StreamReader(string.Format(_routesPaths)))
            {
                string json = reader.ReadToEnd();
                routesCollection.Add(JsonConvert.DeserializeObject<RoutesSet>(json));
            }

            return routesCollection;
        }
    }
}

