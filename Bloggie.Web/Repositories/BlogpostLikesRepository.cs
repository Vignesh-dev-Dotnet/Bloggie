
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
	public class BlogpostLikesRepository : IBlogpostLikesRepository
	{
		private readonly BloggieDbContext bloggieDbContext;

		public BlogpostLikesRepository(BloggieDbContext bloggieDbContext)
        {
			this.bloggieDbContext = bloggieDbContext;
		}

        public async Task<BlogpostLike> AddLikes(BlogpostLike blogpostLike)
        {
            await bloggieDbContext.BlogpostLikes.AddAsync(blogpostLike);
			await bloggieDbContext.SaveChangesAsync();
			return blogpostLike;
        }

        public async Task<IEnumerable<BlogpostLike>> GetLikesForBlog(Guid blogpostId)
        {
			return await bloggieDbContext.BlogpostLikes.Where(x => x.BlogpostId == blogpostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
		{
			return await bloggieDbContext.BlogpostLikes.CountAsync(x => x.BlogpostId == blogPostId);
		}
	}
}
