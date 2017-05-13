using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.ViewModels
{
    public class ProductListFilterVM : IValidatableObject
    {

        public ProductListFilterVM()
        {
            this.StockBegin = 0;
            this.StockEnd = 99999;
        }

        public string ProductName { get; set; }

        public int StockBegin { get; set; }

        public int StockEnd { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.StockEnd < this.StockBegin)
            {
                yield return new ValidationResult("庫存資料篩選條件錯誤!", new[] { "StockBegin", "StockEnd" });
            }

            yield break;
        }
    }
}