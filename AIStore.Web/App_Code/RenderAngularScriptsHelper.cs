using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIStore.Web.App_Code
{
    public static class AngularScriptsRendererHelper
    {
        public static HtmlString RenderAngularScripts(this IHtmlHelper html, bool isForJsString = false)
        {
            string scripts = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/dist/index.html"))
            {
                scripts = reader.ReadToEnd();
            }
            if (isForJsString)
            {
                scripts = scripts.Replace("</", "<\\/");
            }
            else
            {
                scripts = scripts.Replace("src=", "async src=");
            }
            return new HtmlString(scripts);
        }
    }
}