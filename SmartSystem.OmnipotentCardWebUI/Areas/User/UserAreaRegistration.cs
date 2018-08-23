using System.Web.Mvc;

namespace SmartSystem.OmnipotentCardWebUI.Areas.User
{
    public class UserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "U_default",
               "U/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );
            context.MapRoute(
                "User_default",
                "User/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
