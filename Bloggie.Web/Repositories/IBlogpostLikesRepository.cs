using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
	public interface IBlogpostLikesRepository
	{
		Task<int> GetTotalLikes(Guid blogPostId);
		Task<IEnumerable<BlogpostLike>> GetLikesForBlog(Guid blogpostId);
		Task<BlogpostLike> AddLikes(BlogpostLike blogpostLike);

	}
}
