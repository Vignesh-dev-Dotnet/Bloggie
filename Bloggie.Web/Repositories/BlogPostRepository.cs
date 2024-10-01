using Bloggie.Web.Models.Domain;
using Bloggie.Web.Data;
using Microsoft.EntityFrameworkCore;
namespace Bloggie.Web.Repositories
{
	public class BlogPostRepository : IBlogPostRepository
	{
		private readonly BloggieDbContext bloggieDbContext;

		public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
			this.bloggieDbContext = bloggieDbContext;
		}
        public async Task<Blogpost> AddAsync(Blogpost blogpost)
		{
			await bloggieDbContext.Blogposts.AddAsync(blogpost);
			await bloggieDbContext.SaveChangesAsync();
			return blogpost;
		}

		public async Task<Blogpost?> DeleteAsync(Guid id)
		{
			var existingBlog = await bloggieDbContext.Blogposts.FindAsync(id);
			if(existingBlog != null)
			{
				bloggieDbContext.Blogposts.Remove(existingBlog);
				await bloggieDbContext.SaveChangesAsync();
				return existingBlog;
			}
			return null;
		}

		public async Task<IEnumerable<Blogpost>> GetAllAsync()
		{
			return await bloggieDbContext.Blogposts.Include(x => x.Tags).ToListAsync();
		}
		public async Task<Blogpost?> GetAsync(Guid id)
		{
			return await bloggieDbContext.Blogposts.Include(x=> x.Tags).FirstOrDefaultAsync(x => x.Id == id);
		}

        public async Task<Blogpost?> GetByUrlHandleAsync(string urlHandle)
        {
           return await bloggieDbContext.Blogposts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<Blogpost?> UpdateAsync(Blogpost blogpost)
		{
			var existingTag = bloggieDbContext.Blogposts.Include(x => x.Tags).FirstOrDefault(x => x.Id == blogpost.Id);
			if (existingTag != null)
			{
				existingTag.Id = blogpost.Id;
				existingTag.Heading = blogpost.Heading;
				existingTag.PageTitle = blogpost.PageTitle;
				existingTag.Content = blogpost.Content;
				existingTag.ShotrDescription = blogpost.ShotrDescription;
				existingTag.Author = blogpost.Author;
				existingTag.FeaturedImageUrl = blogpost.FeaturedImageUrl;
				existingTag.UrlHandle = blogpost.UrlHandle;
				existingTag.Visible = blogpost.Visible;
				existingTag.PublishedDate = blogpost.PublishedDate;
				existingTag.Tags = blogpost.Tags;

				await bloggieDbContext.SaveChangesAsync();
				return(existingTag);
			}

			return(null);
		}
	}
}



