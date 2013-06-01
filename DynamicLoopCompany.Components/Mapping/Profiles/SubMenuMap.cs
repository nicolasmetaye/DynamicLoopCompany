using System;
using System.Collections.Generic;
using DynamicLoopCompany.Components.Extensions;
using DynamicLoopCompany.Components.Models.SubMenu;
using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace DynamicLoopCompany.Components.Mapping.Profiles
{
    public class SubMenuMap : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IPublishedContent, SubMenuModel>()
                  .ForMember(model => model.Title, expression => expression.ResolveUsing(node => node.Parent.GetPropertyValue<string>("menuTitle")))
                  .ForMember(model => model.Items, expression => expression.ResolveUsing(node =>
                      {
                          var helper = new UmbracoHelper(UmbracoContext.Current);
                          var items = new List<SubMenuItemModel>();
                          foreach (var childNode in node.Parent.Children.Where(childNode => childNode.DocumentTypeAlias.Equals("ContentSubMenu", StringComparison.OrdinalIgnoreCase)))
                          {
                              items.Add(new SubMenuItemModel
                                  {
                                      IsSelected = node.Id == childNode.Id,
                                      Title = childNode.GetTitle(),
                                      Url = childNode.Url
                                  });
                          }
                          return items;
                      }));
        }
    }
}
