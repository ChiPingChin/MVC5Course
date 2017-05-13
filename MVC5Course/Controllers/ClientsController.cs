using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.Models.ViewModels;

namespace MVC5Course.Controllers
{
    public class ClientsController : Controller
    {
        private FabricsEntities db = new FabricsEntities();

        private void GetClients()
        {
            var client = db.Client.Include(c => c.Occupation).Take(10);
            ViewData.Model = client;
        }

        // GET: Clients
        public ActionResult BatchUpdate()
        {
            GetClients();

            return View();
        }

        [HttpPost]
        public ActionResult BatchUpdate(ClientBatchUpdateVM[] items)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in items)
                {
                    var c = db.Client.Find(item.ClientId);
                    c.FirstName = item.FirstName;
                    c.MiddleName = item.MiddleName;
                    c.LastName = item.LastName;
                }
                db.SaveChanges();

                return RedirectToAction("BatchUpdate");
            }

            GetClients();

            return View();
        }


      
    }
}
