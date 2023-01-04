using AIStore.BLL.Services.Factory.Service;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.Settings;
using System.Configuration;

namespace AIStore.BLL.Services.Factory
{
    static class AuthExternalServiceFactory
    {
        public static IExternalService CreateService(string provider, AppSettingsApi settings)
        {

            if (object.Equals(settings, null))
                return null;

            if ("google".Equals(provider))
            {
                return new GoogleService(settings);
            }

            if ("facebook".Equals(provider))
                return new FacebookService(settings);

            if ("linkedin".Equals(provider))
                return new LinkedinService(settings);
           

            return null;
        }
    }
}
