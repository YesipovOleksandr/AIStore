using Newtonsoft.Json;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.Routing;

namespace AIStore.Domain.Concrete
{
    public class RoutesParser : IRoutesParser
    {
        private readonly string _routesPaths;
        public RoutesParser(string routesPaths)
        {
            _routesPaths = routesPaths;
        }
        public List<RoutesSet> ParseAIStores()
        {
            List<RoutesSet> routesCollection = new List<RoutesSet>();

            using (StreamReader reader = new StreamReader(string.Format(_routesPaths)))
            {

                string json = reader.ReadToEnd();
                var routesSet = JsonConvert.DeserializeObject<RoutesSet>(json);
                if (routesSet != null)
                {
                    routesCollection.Add(routesSet);
                }
            }

            return routesCollection;
        }
    }
}
