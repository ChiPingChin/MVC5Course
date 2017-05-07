using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Net;

namespace MVC5Course.Controllers
{
    public class EFController : Controller
    {
        FabricsEntities db = new FabricsEntities();

        // GET: EF
        public ActionResult Index()
        {
            
            var all = db.Product.AsQueryable();

            //var data = all.Where(p => p.Active == true).ToList();

            var data = all.Where(p =>
                p.Active == true
                && p.ProductName.Contains("TT"))
                .OrderByDescending(p => p.ProductId);

            //var data = all.Where(p => p.Active == true);

            //var data1 = all.Where(p => p.ProductId == 1);          // Return IQueryable<Product> Inherit IEnumerable<Product>
            //var data2 = all.FirstOrDefault(p => p.ProductId == 1); // Return Product
            //var data3 = db.Product.Find(1);                        // PK only! Return Product

            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Edit(int id)
        {
            var item = db.Product.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(int id,Product product)
        {

            if (ModelState.IsValid)
            {
                var item = db.Product.Find(id);             

                //db.Entry(item).State = EntityState.Modified;
                item.ProductName = product.ProductName;
                item.Price = product.Price;
                item.Stock = product.Stock;
                item.Active = product.Active;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

     
        public ActionResult Delete(int id)
        {
            var item = db.Product.Find(id);
            db.Product.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //// GET: EF
        //public ActionResult Index()
        //{
        //    using (FabricsEntities db = new FabricsEntities())
        //    {
        //        var all = db.Product.AsQueryable();

        //        var data = all.Where(p => p.Active == true).ToList();  // Get all data , if many , memory explode

        //        return View(data);
        //    }  
        //}


    }
}