using System.Collections.Generic;

namespace DynamicLoopCompany.Components.Models.ContentInteractiveImage
{
    public class ContentInteractiveImageModel
    {
        public string Image { get; set; }
        public List<ContentInteractiveImageItemModel> Items;
        public string ExplanationsText { get; set; }
        public string FooterText { get; set; }
    }
}