using System;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    internal class SharedViewBagAttribute : ActionFilterAttribute
    {
        public int MyProperty { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int inputParam = MyProperty;

            filterContext.Controller.ViewBag.Message = "Your application description page.";
            //base.OnActionExecuting(filterContext);
        }


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}