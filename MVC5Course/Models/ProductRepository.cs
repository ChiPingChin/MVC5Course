using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Course.Models
{
    /// <summary>
    /// ��@ EFRepository(IRepository)�A�i�H��{�Y�@�� Model Entity ���ӷ~�޿�A�� EF �s�����ӷ~�޿�q Controller �������X�ӡA���ʸ˦��@�ӭӪ���k
    /// </summary>
    public class ProductRepository : EFRepository<Product>, IProductRepository
    {
        public override IQueryable<Product> All()
        {
            return base.All().Where(p => !p.Is�R��);
        }

        public IQueryable<Product> All(bool showAll)
        {
            if (showAll)  // Show All
            {
                return base.All();
            }
            else // Show Not �R�� Data
            {
                return base.All().Where(p => !p.Is�R��);
            }

        }

        /// <summary>
        /// ���F�w������k�A�ۭq�@�ӥu���@����ƪ���k
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product Get�浧���ByProductId(int? id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public IQueryable<Product> GetProduct�C���Ҧ����(bool Active, bool ShowAll = false)
        {
            //return this.All().Where(x => x.Active == Active)
            //                      .OrderByDescending(p => p.ProductId).Take(10);

            IQueryable<Product> all = this.All(); // �^�� IQueryable�A��ܦ��ɩ|���u���h�s�� DB (��Ʈw�S�� Loading)
            if (ShowAll)
            {
                all = base.All();  // �^�� IQueryable�A��ܦ��ɩ|���u���h�s�� DB (��Ʈw�S�� Loading)
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

            entity.Is�R�� = true;

            // Controller �~�M�w����A�o�䤣���ӨM�w����A�� Controller �� Commit
            //this.UnitOfWork.Commit();
        }

    }

    public interface IProductRepository : IRepository<Product>
    {

    }
}