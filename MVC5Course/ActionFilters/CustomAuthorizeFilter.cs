using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.ActionFilters
{
    public class CustomAuthorizeFilter : AuthorizeAttribute
    {
        public static string NTAccount = "";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var user = filterContext.HttpContext.User;

            // If Authenticated Failed , return HttpUnauthorizedResult and redirect to login page
            if (user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                // Get NT Account from user : user.Identity.Name is like as 'XYZ\DennyChin'
                GetLoggedNTAccountName(user.Identity.Name);
            }
           
        }

        // Get NT Account from 'Domain\NTAccount' string (eg. 'XYZ\DennyChin')
        private void GetLoggedNTAccountName(string allDMAcc)
        {
            string[] aryNTAccStr = allDMAcc.Split(new Char[] { '\\' });
            string ntAccount = aryNTAccStr[1];
            NTAccount = NTAccount.ToUpper(); // 帳號一律轉成大寫
        }
    }
}