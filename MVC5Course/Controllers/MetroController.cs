using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class MetroController : Controller
    {
        // GET: Metro
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tasks()
        {
            //var a = new {
            //    id = 1,
            //    name ="Denny",
            //    @class = "ASP.NET MVC"   // class 為 C# keyword
            //};

            return View();
        }
    }
}