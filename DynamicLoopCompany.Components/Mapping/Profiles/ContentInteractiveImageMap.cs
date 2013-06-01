using System;
using System.Collections.Generic;
using DynamicLoopCompany.Components.Models.ContentInteractiveImage;
using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace DynamicLoopCompany.Components.Mapping.Profiles
{
    public class ContentInteractiveImageMap : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IPublishedContent, ContentInteractiveImageModel>()
                  .ForMember(model => model.ExplanationsText, expression => expression.ResolveUsing(node => node.GetPropertyValue<string>("explanationsText")))
                  .ForMember(model => model.FooterText, expression => expression.ResolveUsing(node => node.GetPropertyValue<string>("bodyText")))
                  .ForMember(model => model.Image, expression => expression.ResolveUsing(node =>
                      {
                          var helper = new UmbracoHelper(UmbracoContext.Current);
                          var imageMedia = helper.TypedMedia(node.GetPropertyValue<int>("image"));
                          if (imageMedia == null)
                              return string.Empty;
                          return imageMedia.Url;
                      }))
                  .ForMember(model => model.Items, expression => expression.ResolveUsing(node =>
                      {
                          var items = new List<ContentInteractiveImageItemModel>();
                          foreach (var childNode in node.Children.Where(content => content.DocumentTypeAlias.Equals("InteractiveImagePopUp", StringComparison.OrdinalIgnoreCase)))
                          {
                              items.Add(new ContentInteractiveImageItemModel
                                  {
                                      BodyText = childNode.GetPropertyValue<string>("bodyText"),
                                      Title = childNode.GetPropertyValue<string>("title"),
                                      BottomRightY = childNode.GetPropertyValue<int>("bottomRightY"),
                                      BottomRightX = childNode.GetPropertyValue<int>("bottomRightX"),
                                      TopLeftX = childNode.GetPropertyValue<int>("topLeftX"),
                                      TopLeftY = childNode.GetPropertyValue<int>("topLeftY")
                                  });
                          }
                          return items;
                      }));
        }
    }
}
