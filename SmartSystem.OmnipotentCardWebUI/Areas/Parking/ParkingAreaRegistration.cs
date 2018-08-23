using System.Web.Mvc;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking
{
    public class ParkingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Parking";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "p_default",
               "p/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );
            context.MapRoute(
                "Parking_default",
                "Parking/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
