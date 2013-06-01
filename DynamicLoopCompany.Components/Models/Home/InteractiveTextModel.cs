using System.Collections.Generic;

namespace DynamicLoopCompany.Components.Models.Home
{
    public class InteractiveTextModel
    {
        public string BodyText { get; set; }
        public List<InteractiveTextItemModel> LeftText { get; set; }
        public List<InteractiveTextItemModel> RightText { get; set; }
    }
}