using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models
{
	[MetadataType(typeof(ProductPartial))]
	public partial class Product
	{
	}

    //public class Product1
    //{
    //    public string ProductName { get; set; }
    //}

    public partial class ProductPartial
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "請輸入商品名稱吧")]
        [MinLength(3), MaxLength(30)]
        [RegularExpression(".+-.+", ErrorMessage = "商品名稱格式錯誤")]
		[DisplayName("商品名稱")]
        public string ProductName { get; set; }

        [Required]
        [Range(0, 9999, ErrorMessage = "請輸入正確的價格範圍")]
		[DisplayFormat(DataFormatString ="{0:0}")]
        [DisplayName("商品價格")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        [DisplayName("是否上架")]
        public Nullable<bool> Active { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "請輸入正確的庫存範圍")]
        [DisplayName("商品庫存")]
        public Nullable<decimal> Stock { get; set; }

    }
}