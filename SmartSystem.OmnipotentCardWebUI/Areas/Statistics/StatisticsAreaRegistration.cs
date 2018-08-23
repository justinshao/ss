using System.Web.Mvc;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Statistics
{
    public class StatisticsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Statistics";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Statistics_default",
                "S/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
