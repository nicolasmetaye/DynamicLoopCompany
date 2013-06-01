using Umbraco.Core.Models;
using Umbraco.Web;

namespace DynamicLoopCompany.Components.Extensions
{
    public static class NodeExtensions
    {
        public static string GetTitle(this IPublishedContent content)
        {
            var heading = content.GetPropertyValue<string>("heading");
            if (!string.IsNullOrEmpty(heading))
                return heading;
            return content.Name;
        }
    }
}
