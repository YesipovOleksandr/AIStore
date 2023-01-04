using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using AIStore.Domain.Abstract;
using AIStore.Web.Models.Settings;

namespace AIStore.Web.App_Code
{
    public static class AngularConfigsRendererHelper
    {
        public static HtmlString RenderAngularConfigs(this IHtmlHelper html, ClientConfig clientConfig, IRoutesParser routesParser)
        {
            clientConfig.StaticUrl = "/template/dist/";
            return new HtmlString($"<script> var clientConfig = {JsonConvert.SerializeObject(clientConfig, Formatting.Indented).ToLower()} </script>");
        }
    }
}