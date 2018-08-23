using System.Web.Mvc;

namespace SmartSystem.OmnipotentCardWebUI.Areas.AliPay
{
    public class AliPayAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AliPay";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                  "A_default",
                  "A/{controller}/{action}/{id}",
                  new { action = "Index", id = UrlParameter.Optional }
              );
            context.MapRoute(
                "AliPay_default",
                "AliPay/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
