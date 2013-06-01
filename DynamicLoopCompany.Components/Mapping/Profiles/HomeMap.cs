using System;
using System.Collections.Generic;
using System.Linq;
using DynamicLoopCompany.Components.Models.Home;
using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace DynamicLoopCompany.Components.Mapping.Profiles
{
    public class HomeMap : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IPublishedContent, InteractiveTextModel>()
                .ForMember(model => model.BodyText, expression => expression.ResolveUsing(node => node.GetPropertyValue<string>("bodyText")))
                .ForMember(model => model.LeftText, expression => expression.ResolveUsing(node => ExtractInteractiveTextItems(node, "leftText", true)))
                .ForMember(model => model.RightText, expression => expression.ResolveUsing(node => ExtractInteractiveTextItems(node, "rightText", false)));
        }

        private List<InteractiveTextItemModel> ExtractInteractiveTextItems(IPublishedContent node, string textProperty, bool isLeft)
        {
            var interactiveTextNode = node.Children.FirstOrDefault(content => content.DocumentTypeAlias.Equals("InteractiveText", StringComparison.OrdinalIgnoreCase));
            if (interactiveTextNode == null)
                return new List<InteractiveTextItemModel>();
            var text = interactiveTextNode.GetPropertyValue<string>(textProperty);
            var splittedTextItems = new List<InteractiveTextItemModel> { new InteractiveTextItemModel { TextToHighlight = text } };
            var index = 0;
            foreach (var childNode in GetInteractiveTextChildNodes(interactiveTextNode, isLeft))
            {
                var word = childNode.GetPropertyValue<string>("wordToHighlight");
                var popupItem = new InteractiveTextItemModel
                {
                    BodyText = childNode.GetPropertyValue<string>("bodyText"),
                    PopUpId = textProperty + index,
                    TextToHighlight = word
                };
                var newSplittedTextItems = new List<InteractiveTextItemModel>();
                foreach (var splittedTextItem in splittedTextItems)
                {
                    if (!string.IsNullOrEmpty(splittedTextItem.BodyText))
                    {
                        newSplittedTextItems.Add(splittedTextItem);
                        continue;
                    }

                    var wordStartPosition = splittedTextItem.TextToHighlight.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                    if (wordStartPosition == -1)
                    {
                        newSplittedTextItems.Add(splittedTextItem);
                        continue;
                    }
                    if (wordStartPosition != 0)
                        newSplittedTextItems.Add(new InteractiveTextItemModel { TextToHighlight = splittedTextItem.TextToHighlight.Substring(0, wordStartPosition) });
                    newSplittedTextItems.Add(popupItem);
                    var wordEndPosition = wordStartPosition + word.Count();
                    var textCount = splittedTextItem.TextToHighlight.Count();
                    if (wordEndPosition < textCount)
                        newSplittedTextItems.Add(new InteractiveTextItemModel { TextToHighlight = splittedTextItem.TextToHighlight.Substring(wordEndPosition, textCount - wordEndPosition) });
                }
                splittedTextItems = newSplittedTextItems;
                index++;
            }
            return splittedTextItems;
        }

        private List<IPublishedContent> GetInteractiveTextChildNodes(IPublishedContent node, bool isLeft)
        {
            var nodesToReturn = new List<IPublishedContent>();
            var childNodes = node.Children.Where(content => content.DocumentTypeAlias.Equals("InteractiveTextPopUp", StringComparison.OrdinalIgnoreCase));
            foreach (var childNode in childNodes)
            {
                if (childNode.GetPropertyValue<bool>("isLeft") == isLeft)
                    nodesToReturn.Add(childNode);
            }
            return nodesToReturn;
        }
    }
}
