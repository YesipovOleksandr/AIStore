using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIStore.Domain.Models.Routing.LocalizedRoute;

namespace AIStore.Domain.Abstract.Services
{
    public interface ILocalizedRoutesParser
    {
        LocalizedRulesRoot ParseAIStores();
    }
}
