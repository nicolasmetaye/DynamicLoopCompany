using System;
using System.Collections.Generic;
using System.Linq;
using DynamicLoopCompany.Components.Extensions;
using DynamicLoopCompany.Components.Helpers;
using DynamicLoopCompany.Components.Models.Master;
using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace DynamicLoopCompany.Components.Mapping.Profiles
{
    public class MasterMap : Profile
    {
        private readonly StringModifier _stringModifier;

        public MasterMap()
        {
            _stringModifier = new StringModifier();
        }

        protected override void Configure()
        {
            Mapper.CreateMap<IPublishedContent, MetaTagsModel>()
                .ForMember(model => model.Keywords, expression => expression.ResolveUsing(node => node.GetPropertyValue<string>("metaKeywords")))
                .ForMember(model => model.Description, expression => expression.ResolveUsing(node => node.GetPropertyValue<string>("metaDescription")));

            Mapper.CreateMap<IPublishedContent, MainMenuModel>()
                .ForMember(model => model.Items, expression => expression.ResolveUsing(node =>
                    {
                        var items = new List<MainMenuItemModel>();
                        var homePage = node.AncestorOrSelf(1);
                        items.Add(new MainMenuItemModel { IsSelected = (node.Id == homePage.Id), Link = "/", Title = "Home" });
                        var mainMenuNode = node.AncestorOrSelf(2);
                        foreach (var childNode in homePage.Children.Where(content => 
                            content.DocumentTypeAlias.Equals("Content", StringComparison.OrdinalIgnoreCase) ||
                            content.DocumentTypeAlias.Equals("ContentInteractiveImage", StringComparison.OrdinalIgnoreCase) ||
                            content.DocumentTypeAlias.Equals("SubMenu", StringComparison.OrdinalIgnoreCase)))
                        {
                            items.Add(new MainMenuItemModel 
                            { 
                                IsSelected = (mainMenuNode != null && childNode.Id == mainMenuNode.Id), 
                                Link = childNode.Url, 
                                Title = childNode.GetTitle() 
                            });
                        }
                        return items;
                    }));

            Mapper.CreateMap<IPublishedContent, FooterLinksModel>()
                .ForMember(model => model.LeftSectionLinks, expression => expression.ResolveUsing(node =>
                    {
                        var helper = new UmbracoHelper(UmbracoContext.Current);
                        var items = new List<FooterLinkModel>();
                        foreach (var childNode in GetFooterChildNodes(node))
                        {
                            var item = new FooterLinkModel();
                            item.Title = childNode.GetTitle();
                            if (childNode.DocumentTypeAlias.Equals("Content", StringComparison.OrdinalIgnoreCase))
                            {
                                item.IsExternal = false;
                                item.Url = childNode.Url;
                            }
                            else if (childNode.DocumentTypeAlias.Equals("InternalLink", StringComparison.OrdinalIgnoreCase))
                            {
                                item.IsExternal = false;
                                var linkNode = helper.TypedContent(childNode.GetPropertyValue<int>("link"));
                                if (linkNode != null)
                                    item.Url = linkNode.Url;
                            }
                            else if (childNode.DocumentTypeAlias.Equals("InternalMediaLink", StringComparison.OrdinalIgnoreCase))
                            {
                                item.IsExternal = false;
                                var linkMedia = helper.TypedMedia(childNode.GetPropertyValue<int>("link"));
                                if (linkMedia != null)
                                    item.Url = linkMedia.GetPropertyValue<string>("umbracoFile");
                            }
                            else if (childNode.DocumentTypeAlias.Equals("ExternalLink", StringComparison.OrdinalIgnoreCase))
                            {
                                item.IsExternal = true;
                                item.Disclaimer = _stringModifier.CleanJSString(childNode.GetPropertyValue<string>("disclaimer"));
                                item.Url = childNode.GetPropertyValue<string>("link");
                            }
                            else
                            {
                                continue;
                            }
                            items.Add(item);
                        }
                        return items;
                    }));
        }

        private List<IPublishedContent> GetFooterChildNodes(IPublishedContent node)
        {
            var homePage = node.AncestorOrSelf(1);
            var footer = homePage.Children.FirstOrDefault(content => content.DocumentTypeAlias.Equals("Footer", StringComparison.OrdinalIgnoreCase));
            if (footer == null)
                return new List<IPublishedContent>();
            return footer.Children.ToList();
        }
    }
}
