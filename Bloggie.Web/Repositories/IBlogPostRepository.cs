using Bloggie.Web.Models.Domain;
namespace Bloggie.Web.Repositories
{
	public interface IBlogPostRepository
	{
		Task<IEnumerable<Blogpost>>GetAllAsync();
		Task<Blogpost?>GetAsync(Guid id);
		Task<Blogpost?> GetByUrlHandleAsync(string urlHandle);
		Task<Blogpost> AddAsync(Blogpost blogpost);
		Task<Blogpost?> UpdateAsync(Blogpost blogpost);
		Task<Blogpost?> DeleteAsync(Guid id);
	}
}
