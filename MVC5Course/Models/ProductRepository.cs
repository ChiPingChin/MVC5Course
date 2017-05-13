using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Course.Models
{
    /// <summary>
    /// 實作 EFRepository(IRepository)，可以實現某一個 Model Entity 的商業邏輯，把 EF 存取的商業邏輯從 Controller 中抽離出來，都封裝成一個個的方法
    /// </summary>
    public class ProductRepository : EFRepository<Product>, IProductRepository
    {
        public override IQueryable<Product> All()
        {
            return base.All().Where(p => !p.Is刪除);
        }

        public IQueryable<Product> All(bool showAll)
        {
            if (showAll)  // Show All
            {
                return base.All();
            }
            else // Show Not 刪除 Data
            {
                return base.All().Where(p => !p.Is刪除);
            }

        }

        /// <summary>
        /// 除了已有的方法，自訂一個只取一筆資料的方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product Get單筆資料ByProductId(int? id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public IQueryable<Product> GetProduct列表頁所有資料(bool Active, bool ShowAll = false)
        {
            //return this.All().Where(x => x.Active == Active)
            //                      .OrderByDescending(p => p.ProductId).Take(10);

            IQueryable<Product> all = this.All(); // 回傳 IQueryable，表示此時尚未真正去存取 DB (資料庫沒有 Loading)
            if (ShowAll)
            {
                all = base.All();  // 回傳 IQueryable，表示此時尚未真正去存取 DB (資料庫沒有 Loading)
            }
            else // Not Show All
            {
                all = all
                .Where(p => p.Active.HasValue && p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10);
            }

            return all;
            //return all
            //    .Where(p => p.Active.HasValue && p.Active.Value == Active)
            //    .OrderByDescending(p => p.ProductId).Take(10);
        }

        public void Update(Product product)
        {
            this.UnitOfWork.Context.Entry(product).State = EntityState.Modified;
        }

        public override void Delete(Product entity)
        {
            //base.Delete(entity);

            entity.Is刪除 = true;

            // Controller 才決定交易，這邊不應該決定交易，由 Controller 做 Commit
            //this.UnitOfWork.Commit();
        }

    }

    public interface IProductRepository : IRepository<Product>
    {

    }
}