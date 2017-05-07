using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Net;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    //[Authorize]
    public class EFController : Controller
    {
        FabricsEntities db = new FabricsEntities();

        // GET: EF
        public ActionResult Index()
        {
            
            var all = db.Product.AsQueryable();  // 只是查詢物件，不會真的查回資料(延遲載入，故不會立刻執行)，除非之後用 ToList()
            // var all = db.Product.AsEnumerable(); // 會真的查回資料(立即載入 n 筆資料，會立刻執行)

            //var data = all.Where(p => p.Active == true).ToList();

            var data = all.Where(p =>
                p.Active == true
                && p.ProductName.Contains("TT")
                && p.Is刪除 == false
                )
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
            var product = db.Product.Find(id);

            // 先刪除關聯表格中資料
            // <法一>利用導覽屬性查出關聯資料後逐一刪除他
            //foreach (var item in product.OrderLine.ToList())
            //{
            //    db.OrderLine.Remove(item);
            //}

            //// <法二> 一次刪除
            //db.OrderLine.RemoveRange(product.OrderLine);

            //db.Product.Remove(product);

            try
            {
                // 改為用標記 Flag 方式
                product.Is刪除 = true;

                db.SaveChanges();  // 最後一部才做 Commit，以免中途發生錯誤在 DB 中產生贓資料

                return RedirectToAction("Index");

            }
            catch (DbEntityValidationException ex) // 用 try ~ catch 包住錯誤，以便於發生錯誤時看詳細錯誤資訊
            {

                throw ex;
            }

            
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

        public ActionResult Details(int id)
        {
            // 效能調教使用(直接下 SQL)
            var data = db.Database.SqlQuery<Product>("SELECT * FROM dbo.Product WHERE ProductId=@p0" ,id)
                .FirstOrDefault();

            return View(data);
        }

        public ActionResult RemoveAll()
        {
            // 效能差
            //db.Product.RemoveRange(db.Product);
            //db.SaveChanges();

            // 效能好
            db.Database.ExecuteSqlCommand("Delete from dbo.Product");

            return View();
        }


    }
}