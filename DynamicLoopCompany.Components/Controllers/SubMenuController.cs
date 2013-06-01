using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace DynamicLoopCompany.Components.Controllers
{
    public class SubMenuController : Umbraco.Web.Mvc.RenderMvcController
    {
        public ActionResult SubMenu(RenderModel model)
        {
            var contentSubMenu = model.Content.Children.FirstOrDefault(node => node.DocumentTypeAlias.Equals("ContentSubMenu"));
            if (contentSubMenu == null)
                return base.Index(model);
            return Redirect(contentSubMenu.Url);
        }
    }
}
