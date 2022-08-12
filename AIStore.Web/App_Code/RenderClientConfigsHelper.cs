using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using AIStore.Domain.Models.Settings.ClientConfigs;
using AIStore.Domain.Abstract;

namespace AIStore.Web.App_Code
{
    public static class AngularConfigsRendererHelper
    {
        public static HtmlString RenderAngularConfigs(this IHtmlHelper html, ClientConfig clientConfig, IRoutesParser routesParser)
        {
            //clientConfig.StaticUrl = $"/{routesParser.GetRuleByName("static").Url}/dist/";
            clientConfig.StaticUrl = "/template/dist/";
            return new HtmlString($"<script> var clientConfig = {JsonConvert.SerializeObject(clientConfig, Formatting.Indented).ToLower()} </script>");
        }
    }
}