using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    public class Clients1Controller : Controller
    {
        private FabricsEntities db = new FabricsEntities();

        // GET: Clients1
        public ActionResult Index(int CreditRatingFilter = -1, string LastNameFilter ="")
        {
            // 1. 產生 DropdownList CreditRating 的候選屬性值
            // 從 DB 撈出資料，當作選項清單
            var ratings = (from p in db.Client                          
                           select p.CreditRating
                           ).Distinct().OrderBy(p => p).ToList();

            // 將選項清單餵給 ViewBag.CreditRatingFilter 或是 ViewData["CreditRatingFilter"]
            ViewBag.CreditRatingFilter = new SelectList(ratings);


            // 2. 產生 DropdownList LastName 的候選屬性值
            // 從 DB 撈出資料，當作選項清單
            var lastName = (from p in db.Client
                           select p.LastName
                           ).Distinct().OrderBy(p => p).ToList();

            // 將選項清單餵給 ViewBag.LastName 或是 ViewData["LastName"]
            ViewBag.LastNameFilter = new SelectList(lastName);

            // var client = db.Client.Include(c => c.Occupation);
            var client = db.Client.AsQueryable();

            // 執行篩選作業
            if (CreditRatingFilter >= 0)
            {
                client = client.Where(c => c.CreditRating == CreditRatingFilter);
            }

            if (!string.IsNullOrEmpty(LastNameFilter))
            {
                client = client.Where(c => c.LastName == LastNameFilter);
            }
            
            return View(client.Take(10));
        }

        // GET: Clients1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients1/Create
        public ActionResult Create()
        {
            ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName");
            return View();
        }

        // POST: Clients1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender,DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode,Longitude,Latitude,Notes")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Client.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // GET: Clients1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            // 產生 CreditRating 下拉式選單選項值，塞入 ViewBag or ViewData 中
            var items = new int[] { 0,1,2,3,4,5,6,7,8,9};
            ViewBag.CreditRating = new SelectList(items);
            // 或是
            // ViewData["CreditRating"] = new SelectList(items);

            ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            return View(client);  // 丟回 Model
        }

        // POST: Clients1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender,DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode,Longitude,Latitude,Notes")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // GET: Clients1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Client.Find(id);
            db.Client.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
