using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Who()
        {
            return View();
        }

        //[ActionName("About.aspx")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult PartialAbout()
        {
            ViewBag.Message = "Your application description page.";

            if (Request.IsAjaxRequest())
            {
                return PartialView("About");
            }
            else
            {
                return View("About");
            }            
        }

        
        //public String SomeAction()
        public ActionResult SomeAction()
        {
            // 不好，不要這樣回傳，把 Javascript 寫在 Controller 裡面，且 JS Code 分散在不同 Acrion，維護性變差 (架構不佳)
            // 應該是 View 的工作，回傳一個值，讓 View 去處理顯示就好
            //Response.Write("<script>alert('Created Success!'); location.href ='/'; </script>");  // 不好，不要這樣回傳
            //return Content("<script>alert('Created Success!'); location.href ='/'; </script>");

            // 改架構(不要用 Content 來回應內容，改用 PartialView 來回應)，交由 View 去做顯示(JS 寫在 View 中做顯示處理)，
            //       建立一個  PartialView (在 Shared 目錄下)，裡面放此 JS 程式碼(交給 View 處理顯示)，就可以全站共用，以後只改一個地方即可，容易維護
            return PartialView("SuccessRedirect","/");
        }

        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFile()
        {
            // 第三個參數為自訂下載檔案名稱
            //return File("~/Content/wannacry.png","image/png","downNewName.png");  

            return File("~/Content/wannacry.png", "image/png");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Action()
        {
            return View();
        }

        /// <summary>
        /// 取得 Json 資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJson()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Json(db.Product.Take(5), JsonRequestBehavior.AllowGet);
        }
    }
}