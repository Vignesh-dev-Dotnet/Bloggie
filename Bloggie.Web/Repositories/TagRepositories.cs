﻿using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
	public class TagRepositories : ITagRepository
	{
		private readonly BloggieDbContext bloggieDbContext;

		public TagRepositories(BloggieDbContext bloggieDbContext)
        {
			this.bloggieDbContext = bloggieDbContext;
		}
        public async Task<Tag> AddAsync(Tag tag)
		{
			await bloggieDbContext.Tags.AddAsync(tag);
			await bloggieDbContext.SaveChangesAsync();
			return tag;
		}

		public async Task<Tag?> DeleteAsync(Guid id)
		{
			var existingtag = await bloggieDbContext.Tags.FindAsync(id);
			if (existingtag != null)
			{
				bloggieDbContext.Tags.Remove(existingtag);
				await bloggieDbContext.SaveChangesAsync();
				return existingtag;
			}
			return null;
		}

		public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery,string? sortBy,string? sortDirection)
		{
			var query = bloggieDbContext.Tags.AsQueryable();

			//Filtering
			if(string.IsNullOrWhiteSpace(searchQuery) == false)
			{
				query = query.Where(x => x.Name.Contains(searchQuery) ||
					                     x.DisplayName.Contains(searchQuery));
			}
			//Sorting
			if(string.IsNullOrWhiteSpace(sortBy) == false)
			{
				var isDesc = string.Equals(sortDirection ,"Desc", StringComparison.OrdinalIgnoreCase);
				if(string.Equals(sortBy,"Name",StringComparison.OrdinalIgnoreCase))
				{
					query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);

				}
				if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
				{
					query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);

				}
			}
			//Pagination

			return await query.ToListAsync();
			// return await bloggieDbContext.Tags.ToListAsync();
		}

		public Task<Tag?> GetAsync(Guid id)
		{
			return bloggieDbContext.Tags.FirstOrDefaultAsync(x=>x.Id == id);
		}

		public async Task<Tag?> UpdateAsync(Tag tag)
		{
			var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);
			if (existingTag != null) 
			{ 
				existingTag.Name  = tag.Name;
				existingTag.DisplayName = tag.DisplayName;

				await bloggieDbContext.SaveChangesAsync();

				return existingTag;
			}
			return null;

		}
	}
}