using System.Web.Mvc;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin
{
    public class NWeiXinAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NWeiXin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "NWeiXin_default",
                "nwx/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
