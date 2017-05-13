using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC5Course
{
    /// <summary>
    /// 所有網址結構都由路由來控制
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // 忽略路由
            // 比對中，不處理(不走)下面的 MapRoute 規則，丟給 IIS 處理 (*表萬用，網址列出現 axd 檔案(web form 很多為這種檔案))
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");

            // 比對路由
            // 比對中，就處理；若比對不中，丟給 IIS 處理 
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                // url: "{controller}/{action}/{*id}",      // * Catch All : 全吃多段 http://Home/Index/1/234/55/6
                // url: "{controller}/{action}.aspx/{id}",  // 全站都變成 *.aspx 結尾的 URL
                defaults: new {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional }
            );

            //// 比對路由
            //// 比對中，就處理；若比對不中，丟給 IIS
            //routes.MapRoute(
            //    name: "Next",
            //    url: "{controller}/{*param}",   // 表示 controller/ 後面全接下來
            //    defaults: new
            //    {
            //        controller = "Products",
            //        action = "Index",
            //        param = UrlParameter.Optional  // 允許 null
            //    }
            //);

        }
    }
}
