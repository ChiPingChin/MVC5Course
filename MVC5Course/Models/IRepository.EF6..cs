

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MVC5Course.Models
{ 
    /// <summary>
    /// 提供資料操作服務介面
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IRepository<T> 
	{
		IUnitOfWork UnitOfWork { get; set; }
		IQueryable<T> All();
		IQueryable<T> Where(Expression<Func<T, bool>> expression);
		void Add(T entity);
		void Delete(T entity);
	}
}

