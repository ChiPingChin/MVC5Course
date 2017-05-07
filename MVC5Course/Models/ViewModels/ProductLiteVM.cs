using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.ViewModels
{
    /// <summary>
    /// 精簡版的 Product(即是商品資料的 ViewModel) , 主要用於建立商品資料
    /// </summary>
    public class ProductLiteVM
    {
        public int ProductId { get; set; }

        [Required]
        [MinLength(5)]
        public string ProductName { get; set; }

        public Nullable<decimal> Price { get; set; }

        public Nullable<decimal> Stock { get; set; }
    }
}