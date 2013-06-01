using DynamicLoopCompany.Components.Extensions;
using DynamicLoopCompany.Components.Models.Content;
using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace DynamicLoopCompany.Components.Mapping.Profiles
{
    public class ContentMap : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IPublishedContent, ContentModel>()
                  .ForMember(model => model.Title, expression => expression.ResolveUsing(node => node.GetTitle()))
                  .ForMember(model => model.BodyText, expression => expression.ResolveUsing(node => node.GetPropertyValue<string>("bodyText")));
        }
    }
}
