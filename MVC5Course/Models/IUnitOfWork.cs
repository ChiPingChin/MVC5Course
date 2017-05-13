using System.Data.Entity;

namespace MVC5Course.Models
{
    /// <summary>
    /// ����� DB ��Ʀs��
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