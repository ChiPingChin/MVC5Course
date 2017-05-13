using System.Data.Entity;

namespace MVC5Course.Models
{
    /// <summary>
    /// 做實際 DB 資料存取
    /// </summary>
	public interface IUnitOfWork
	{
		DbContext Context { get; set; }
		void Commit();
		bool LazyLoadingEnabled { get; set; }
		bool ProxyCreationEnabled { get; set; }
		string ConnectionString { get; set; }
	}
}