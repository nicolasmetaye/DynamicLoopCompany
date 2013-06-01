using System;
using DynamicLoopCompany.Components.Mapping;

namespace DynamicLoopCompany.Components
{
    public class GlobalApplication : Umbraco.Web.UmbracoApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            base.OnApplicationStarted(sender, e);

            AutoMapperConfiguration.Configure();
        }
    }
}
