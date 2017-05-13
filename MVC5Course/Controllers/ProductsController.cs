﻿using System;
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
    public class ProductsController : Controller
    {
        //private FabricsEntities db = new FabricsEntities();

        // 改透過 Repository 服務去存取 DB
        ProductRepository repo = RepositoryHelper.GetProductRepository();       

        // GET: Products
        public ActionResult Index(bool Active = true)
        {
            //return View(db.Product.ToList());
            //var data = db.Product.OrderByDescending(p => p.ProductId).Take(10);

            // 使用 Repository 方式操作資料，隱藏商業邏輯，不寫在 Controller，改寫在 Repository 中
            // 法一
            //var repo = new ProductRepository();
            //repo.UnitOfWork = new EFUnitOfWork();
            // 法二 (法一的簡寫方式)
            //ProductRepository repo = RepositoryHelper.GetProductRepository();

            //var data = db.Product
            //    .Where(x => x.Active.HasValue && x.Active == Acㄋtive)
            //    .OrderByDescending(p => p.ProductId).Take(10);

            //var data = repo.All()  // 在 Controller 中完全看不到 EF 操作(多隔了一層)
            //   .Where(x => x.Active.HasValue && x.Active == Active)
            //   .OrderByDescending(p => p.ProductId).Take(10);
                        
            // 改用 Repository 方式存取
            var data = repo.All(Active)
                 .Where(p => p.Active.HasValue && p.Active.Value == Active)
                  .OrderByDescending(p => p.ProductId).Take(10);

            return View(data);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // 註解：直接用 EF 方式存取，並將商業邏輯寫在 Controller 中(不佳)
            // Product product = db.Product.Find(id);
            // 改用 Repository 方式存取 (隱藏商業邏輯在 Repository 中)(較佳)
            Product product = repo.Get單筆資料ByProductId(id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid) // 取得驗證結果(綜合驗證結果)，如果正確，繼續下去做新增
            {
                //db.Product.Add(product);
                //db.SaveChanges();

                // 改透過 Repository 服務去存取 DB
                repo.Add(product);
                repo.UnitOfWork.Commit();

                //TempData["Msg"] = "新增成功!!";
                return RedirectToAction("Index");
            }

            //ViewBag.product = product;

            return View(product); // 輸入錯誤時繼續顯示輸入的資料在頁面上
        }

        public class NewMergeData
        {
            public IEnumerable<Product> products { get; set; }
            public Product prod { get; set; }

        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Product product = db.Product.Find(id);
            // 改透過 Repository 服務去存取 DB
            Product product = repo.Get單筆資料ByProductId(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(product).State = EntityState.Modified;
                //db.SaveChanges();

                // 改透過 Repository 服務去存取 DB
                repo.Update(product);
                repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            // <Model Binding> 把資料讀入 ACTION 的參數中的功能
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Product product = db.Product.Find(id);
            // 改透過 Repository 服務去存取 DB
            Product product = repo.Get單筆資料ByProductId(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Product product = db.Product.Find(id);
            //db.Product.Remove(product);
            //db.SaveChanges();

            // 改透過 Repository 服務去存取 DB
            Product product = repo.Get單筆資料ByProductId(id);
            repo.Delete(product);
            repo.UnitOfWork.Commit();

            return RedirectToAction("Index");
        }

        // 改透過 Repository 服務去存取 DB，就不需要以下程式碼去釋放 FabricsEntities 資源了
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        public ActionResult ListProducts()
        {
            //var data = db.Product
            //    .Where(p => p.Active == true)
            // 改透過 Repository 服務去存取 DB
            var data = repo.GetProduct列表頁所有資料(true)
                .Select(p => new ProductLiteVM
                {
                    ProductId= p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Stock = p.Stock
                })
                .Take(10);

            return View(data);
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost] //
        public ActionResult CreateProduct(ProductLiteVM data)  // 使用 ViewModel (較佳)
        //public ActionResult CreateProduct(
        //    [Bind(Include = "ProductName,Price,Stock")]  // 只會接受此三欄位個欄位資料，其他欄位為預設值，且會原定義受驗證(不佳)
        //    Product data)
        {
            if (ModelState.IsValid)
            {
                // TODO: 儲存資料進資料庫...

                // 儲存成功，導向顯示產品 List 頁面(用自製的 ViewModel 顯示資料內容)
                return RedirectToAction("ListProducts");
            }

            // 驗證失敗，繼續顯示原本的表單
            return View();
        }
    }
}