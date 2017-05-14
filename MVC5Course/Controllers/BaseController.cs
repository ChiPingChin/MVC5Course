using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.ActionFilters;

namespace MVC5Course.Controllers
{
    public abstract class BaseController : Controller
    {
        protected FabricsEntities db = new FabricsEntities();

        [LocalOnly]
        public ActionResult Debug()
        {
            return Content("Hello");
        }

        ///// <summary>
        ///// 某個 Action 找不到時(找不到頁面時)，一律倒回首頁，不建議加上此，除非有此需求
        ///// </summary>
        ///// <param name="actionName"></param>
        //protected override void HandleUnknownAction(string actionName)
        //{
        //    this.RedirectToAction("Index","Home").ExecuteResult(this.ControllerContext);
        //}
    }
}