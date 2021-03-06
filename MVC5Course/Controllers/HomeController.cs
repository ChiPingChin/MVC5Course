﻿using MVC5Course.ActionFilters;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    //[CustomAuthorizeFilter]
    public class HomeController : BaseController
    {        
        public ActionResult Index()
        {
            return View();
        }

        [SharedViewBag]
        public ActionResult Who()
        {
            return View();
        }

        /// <summary>
        /// 檔案上傳作業
        /// </summary>
        /// <param name="file">參數名稱要和前端 View 的 input name 相同，才能正常做 Data Binding</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            //檢查是否有選擇檔案
            if (file != null)
            {
                //檢查檔案大小要限制也可以在這裡做
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/FileUploads"), fileName);

                    file.SaveAs(path);

                    // 若成功，將結果訊息存放到 TempData 準備轉到另一頁顯示訊息
                    // http://ithelp.ithome.com.tw/articles/10159173
                    TempData["FileUploadResult"] = "檔案上傳成功!";

                    // 轉導向到另一頁面並顯示成功訊息
                    return RedirectToAction("FileUploadResult");
                }
            }

            // 若失敗，將結果訊息存放到 TempData 準備轉到另一頁顯示訊息
            TempData["FileUploadResult"] = "檔案上傳失敗，請確認!";

            // 若失敗，回到原來頁面
            return View(file);
        }

        /// <summary>
        /// 檔案上傳結果顯示
        /// </summary>
        /// <returns></returns>
        public ActionResult FileUploadResult()
        {
            // 接收轉導向的成功訊息，準備顯示在 View
            ViewBag.ResultMsg = TempData["FileUploadResult"];
            return View();
        }


        /// <summary>
        /// 檔案下載作業(方式一)
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadFile1()
        {
            string filePath = Server.MapPath("~/FileUploads/IMG20170422184840.jpg");
            string fileType = @"image/jpg";
            return File(filePath, fileType);
        }

        /// <summary>
        /// 檔案下載作業(方式二)
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadFile2()
        {
            //我要下載的檔案位置
            string filePath = Server.MapPath("~/FileUploads/IMG20170424105915.zip");
            string fileType = @"application/zip";

            //取得檔案名稱 (準備給之後下載時的檔案預設命名)
            string fileName = Path.GetFileName(filePath);

            // 讀成串流
            Stream iStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            // 回傳出檔案
            return File(iStream, fileType, fileName);
        }


        /// <summary>
        /// 檔案下載作業3-1(方式三：多個檔自由選擇下載，含壓縮成一個檔案輸出)
        /// https://www.mikesdotnetting.com/article/286/downloading-multiple-files-in-asp-net
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadFileIndex()
        {
            ViewBag.Files = Directory.EnumerateFiles(Server.MapPath("~/FileUploads"));
            return View();
        }

        /// <summary>
        /// 檔案下載作業3-2
        /// </summary>
        /// <param name="files">此參數的名字要和 View 上的 input name 相同才能進行 Model Binding </param>
        /// <returns></returns>
        [HttpPost]
        public FileResult DownloadFileIndex(List<string> files)
        {
            var archive = Server.MapPath("~/My_Photo_Archive.zip");
            var temp = Server.MapPath("~/Temp");

            // clear any existing archive
            if (System.IO.File.Exists(archive))
            {
                System.IO.File.Delete(archive);
            }

            // empty the temp folder
            Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));

            // copy the selected files to the temp folder
            files.ForEach(f => System.IO.File.Copy(f, Path.Combine(temp, Path.GetFileName(f))));

            // create a new archive file
            ZipFile.CreateFromDirectory(temp, archive);

            // 回傳 FileResult
            // 第二個參數為下載檔案的完整路徑名稱
            // 第二個參數為下載檔案的 Content-Type
            // 第三個參數為下載時的檔案預設命名
            return File(archive, "application/zip", "My_Photo_Archive.zip");  
        }


        //[ActionName("About.aspx")]
        // Action Filter Case : 適用場景：寫 Log, 做 Auth, All Shared Common Operations for Actions
        // Action Filter Case : 可以讓 Controller 變輕，且程式碼只要撰寫一次，即可套用在不同 Action / Controller 上面
        [SharedViewBag]
        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            throw new ArgumentException("Error Handled!");

            return View();
        }

        public ActionResult PartialAbout()
        {
            ViewBag.Message = "Your application description page.";

            if (Request.IsAjaxRequest())
            {
                return PartialView("About");
            }
            else
            {
                return View("About");
            }
        }


        //public String SomeAction()
        public ActionResult SomeAction()
        {
            // 不好，不要這樣回傳，把 Javascript 寫在 Controller 裡面，且 JS Code 分散在不同 Acrion，維護性變差 (架構不佳)
            // 應該是 View 的工作，回傳一個值，讓 View 去處理顯示就好
            //Response.Write("<script>alert('Created Success!'); location.href ='/'; </script>");  // 不好，不要這樣回傳
            //return Content("<script>alert('Created Success!'); location.href ='/'; </script>");

            // 改架構(不要用 Content 來回應內容，改用 PartialView 來回應)，交由 View 去做顯示(JS 寫在 View 中做顯示處理)，
            //       建立一個  PartialView (在 Shared 目錄下)，裡面放此 JS 程式碼(交給 View 處理顯示)，就可以全站共用，以後只改一個地方即可，容易維護
            return PartialView("SuccessRedirect", "/");
        }

        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFile()
        {
            // 第三個參數為自訂下載檔案名稱
            //return File("~/Content/wannacry.png","image/png","downNewName.png");  

            return File("~/Content/wannacry.png", "image/png");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Action()
        {
            return View();
        }

        /// <summary>
        /// 取得 Json 資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJson()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Json(db.Product.Take(5), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VT()
        {
            ViewBag.IsEnabled = true;
            return View();
        }

        public ActionResult RazorTest()
        {
            int[] data = { 1, 2, 3, 4, 5 };
            return View(data);
        }
    }
}