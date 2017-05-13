using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class HelloController : BaseController
    {
        // GET: Hello
        public ActionResult Index()
        {
            // 法一法二同義

            // 法一
            //return new ViewResult()
            //{

            //};

            // 法二 (輔助方法,一樣產生 ActionResult)
            return View();
        }
    }
}