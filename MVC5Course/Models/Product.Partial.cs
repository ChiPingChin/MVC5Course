using MVC5Course.Models.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models
{
	[MetadataType(typeof(ProductPartial))]
	public partial class Product : IValidatableObject
	{
        public int 訂單數量
        {
            get {
                return this.OrderLine.Count;

                //return this.OrderLine.Count; // 效能不好，因會取出所有資料後才能計算筆數
                //return this.OrderLine.Where(p => p.Qty > 400).Count();  // 效能不好，因會取出所有資料後才能計算筆數
                //return this.OrderLine.Where(p => p.Qty > 400).ToList().Count; // 效能不好，因會取出所有資料後才能計算筆數
                return this.OrderLine.Count(p => p.Qty > 400);  // 效能最好，會自動產生 SELECT COUNT(*) FROM OrderLine; 後傳回來

            }
        }

        /// <summary>
        /// 模型驗證(進階驗證)
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // 輸入驗證結束後才會做此模型驗證 (已經 Model Binding 完成且已經經過輸入驗證後)
            if (this.Price > 100 && this.Stock > 5)
            {
                yield return new ValidationResult("價格與數量不合理"
                    ,new string[] { "Price","Stock"});
            }

            if (this.OrderLine.Count() == 0 && this.Stock > 0)
            {
                yield return new ValidationResult("庫存和訂單數量不匹配" ,
                    new string[] { "Stock"});
            }

            yield break;
        }
    }

    //public class Product1
    //{
    //    public string ProductName { get; set; }
    //}

    public partial class ProductPartial
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "請輸入商品名稱")]
        [MinLength(1), MaxLength(80)]
        //[RegularExpression(".+-.+", ErrorMessage = "商品名稱格式錯誤")]
		[DisplayName("商品名稱")]
        [商品名稱必須包含Will字串(ErrorMessage = "商品名稱必須包含Will字串!")]
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
        [Range(0, 99999999, ErrorMessage = "請輸入正確的庫存範圍")]
        [DisplayName("商品庫存")]
        public Nullable<decimal> Stock { get; set; }

    }
}