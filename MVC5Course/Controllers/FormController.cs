using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class FormController : BaseController
    {
        // GET: Form
        public ActionResult Index()
        {
            var data = db.Product.Take(10);
            return View(data);
        }

        /// <summary>
        ///  http://localhost:52102/form/edit/1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            // 此時 Model Binding 只有 Bind id 一個欄位值到 ModelState 中，ViewData.Model 中尚無資料

            // 作法一：詳細寫法 (餵資料給 ViewData.Model ，ViewData.Model 會傳到 View 最上方宣告的型別物件中，自動轉型後綁定個欄位在 View 中顯示，轉型失敗會丟出錯誤)
            // ViewData.Model = db.Product.Find(id);
            // return View();

            // 作法二：精簡寫法 (效果同作法一，預設會將傳入 View 的 Data 放在 ViewData.Model 預設屬性中，所以簡寫成以下，不用特別寫 ViewData.Model = db.Product.Find(id); )
            return View(db.Product.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            var product = db.Product.Find(id);

            if (TryUpdateModel(product,
                includeProperties: new string[] { "ProductName" }))   // Model Binding 只做 ProductName 欄位綁定 Binding
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // ***  View 中個欄位顯示以 Model Binding 的 ModelState 為優先，而 ViewData.Model 綁定則次之
            // 所以以下再撈一次以補上方只 Bind 一個欄位的不足，補上其他欄位值，Binding 以 Model Binding 的 ModelState 為優先(在前面只綁定了一個欄位 ProductName)
            //product = db.Product.Find(id);  
            // View 會先找 ModelState (Binding 的內容優先找)，然後再找 ViewData.Model 欄位值做欄位值顯示
            ViewData.Model = db.Product.Find(id);

            return View();
        }
    }
}