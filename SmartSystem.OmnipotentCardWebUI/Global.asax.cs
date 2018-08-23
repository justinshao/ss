using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common.Entities;
using System.Configuration;
using Common.Services;
using SmartSystem.WeiXinServices.Background;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorFilterAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                  "wxAPI",
                  "api_{token}",
                  new { controller = "Api", action = "Index" }
                  );
            routes.MapRoute(
              name: "LoadPrompt",
              url: "L/{id}",
              defaults: new { controller = "LoadPrompt", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                  name: "RedirectHandle",
                  url: "RedirectHandle/{id}",
                  defaults: new { controller = "RedirectHandle", action = "Index", id = UrlParameter.Optional }
              );
            routes.MapRoute(
                name: "R",
                url: "R/{id}",
                defaults: new { controller = "RedirectHandle", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "WeiXinAuthorize",
               url: "WeiXinAuthorize/{id}",
               defaults: new { controller = "WeiXinAuthorize", action = "Index", id = UrlParameter.Optional }
           );
            //微信扫码
            routes.MapRoute(
             name: "QRCodeLoadPrompt",
             url: "qrl/{id}",
             defaults: new { controller = "QRCodeLoadPrompt", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
               name: "QRCodeEntrance",
               url: "qre/ix/{id}",
               defaults: new { controller = "QRCodeEntrance", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "QRCodeParkPayment",
             url: "qrp/ix",
             defaults: new { controller = "QRCodeParkPayment", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
          name: "ScanCodeInOut",
          url: "scio/ix",
          defaults: new { controller = "ScanCodeInOut", action = "Index", id = UrlParameter.Optional }
      );
            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            InitData();
        }
        private void InitData()
        {
            try
            {
                SystemDefaultConfig.WriteConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                SystemDefaultConfig.ReadConnectionString = ConfigurationManager.ConnectionStrings["ReadConnectionString"].ToString();
                SystemDefaultConfig.BWPKID = ConfigurationManager.AppSettings["BWPKID"] ?? "";
                SystemDefaultConfig.SFMPKID = ConfigurationManager.AppSettings["SFMPKID"] ?? "";
                //Session["SmartSystem_LogFrom"] = LogFrom.UnKnown;
                SystemDefaultConfig.DatabaseProvider = "Sql";
                SystemDefaultConfig.DataUpdateFlag = 3;
                SystemDefaultConfig.SystemDomain = ConfigurationManager.AppSettings["SystemDomain"] ?? "";
                SystemDefaultConfig.CreateImageUploadFile();
                SystemDefaultConfig.Secretkey = ConfigurationManager.AppSettings["Secretkey"] ?? "";
                BackgroundWorkerManager.EnvironmentPath = Server.MapPath("~");
                CompanyServices.InitSystemDefaultCompany();
                string startBackground = ConfigurationManager.AppSettings["BackgroundWorkeStart"] ?? "0";
                if (startBackground == "1")
                {
                    BackgroundWorkerManager.Start();
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("Application_Start", ex);
            }
        }
    }
}