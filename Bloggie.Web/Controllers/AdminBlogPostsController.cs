using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Bloggie.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminBlogPostsController : Controller
	{
		private readonly ITagRepository tagRepository;
		private readonly IBlogPostRepository blogPostRepository;

		public AdminBlogPostsController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)
        {
			this.tagRepository = tagRepository;
			this.blogPostRepository = blogPostRepository;
		}
        public async Task<ActionResult> Add()
		{
			var tags = await tagRepository.GetAllAsync();

			var model = new AddBlogPostsRequest
			{
				Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
			};


			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddBlogPostsRequest addBlogPostsRequest)
		{
			var blogPost = new Blogpost
			{
				Heading = addBlogPostsRequest.Heading,
				PageTitle = addBlogPostsRequest.PageTitle,
				Content = addBlogPostsRequest.Content,
				ShotrDescription = addBlogPostsRequest.ShotrDescription,
				FeaturedImageUrl = addBlogPostsRequest.FeaturedImageUrl,
				UrlHandle = addBlogPostsRequest.UrlHandle,
				PublishedDate = addBlogPostsRequest.PublishedDate,
				Author = addBlogPostsRequest.Author,
				Visible = addBlogPostsRequest.Visible,
				
			};

			var selectedTags = new List<Tag>();
			foreach (var selectedTagId in addBlogPostsRequest.SelectedTags)
			{
				var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
				var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
				if(existingTag != null)
				{
					selectedTags.Add(existingTag);
				}
			}
			//Mapping tags back to domain model
			blogPost.Tags = selectedTags;

			await blogPostRepository.AddAsync(blogPost);
			return RedirectToAction("Add");
		}

		public async Task<IActionResult> List()
		{
			var blogPosts = await blogPostRepository.GetAllAsync();
			return View(blogPosts);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			//Retrive the information from the repository
			var blogpost = await blogPostRepository.GetAsync(id);
			var tagsDomainModel = await tagRepository.GetAllAsync();

			if (blogpost != null)
			{
				var model = new EditBlogPostRequest
				{
					//Map the domain model to the view model

					Id = blogpost.Id,
					Heading = blogpost.Heading,
					PageTitle = blogpost.PageTitle,
					Content = blogpost.Content,
					ShotrDescription = blogpost.ShotrDescription,
					FeaturedImageUrl = blogpost.FeaturedImageUrl,
					UrlHandle = blogpost.UrlHandle,
					PublishedDate = blogpost.PublishedDate,
					Author = blogpost.Author,
					Visible = blogpost.Visible,
					Tags = tagsDomainModel.Select(x => new SelectListItem
					{
						Text = x.Name,
						Value = x.Id.ToString()
					}),
					SelectedTags = blogpost.Tags.Select(x => x.Id.ToString()).ToArray()

				};
				//pass data to view
				return View(model);
			}
			return View(null);
			
		}
		
		[HttpPost]
		public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
		{
			//Map the view model to the domain model
			var blogPostDomainModel = new Blogpost
			{
				Id = editBlogPostRequest.Id,
				Heading = editBlogPostRequest.Heading,
				PageTitle = editBlogPostRequest.PageTitle,
				Content = editBlogPostRequest.Content,
				Author = editBlogPostRequest.Author,
				ShotrDescription = editBlogPostRequest.ShotrDescription,
				FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
				PublishedDate = editBlogPostRequest.PublishedDate,
				UrlHandle = editBlogPostRequest.UrlHandle,
				Visible = editBlogPostRequest.Visible,

			};
			//Map tags into domain models

			var selectedTags = new List<Tag>();
			foreach(var selectedTag in editBlogPostRequest.SelectedTags)
			{
				if(Guid.TryParse(selectedTag,out var tag))
				{
					var foundTag = await tagRepository.GetAsync(tag);
					if(foundTag != null)
					{
						selectedTags.Add(foundTag);
					}
				}
			}
			blogPostDomainModel.Tags = selectedTags;

			//Submit information to repository to update

			var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

			if(updatedBlog != null)
			{
				//show success notification
				return RedirectToAction("Edit");
			}

			//show failure notification
			return RedirectToAction("Edit");
		
		}

		[HttpPost]
		public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
		{
			//Talk to repository to delete this blogpost and related tags

			var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

			if(deletedBlogPost != null)
			{
				return RedirectToAction("List");
			}
			
			return RedirectToAction("Edit", new {id = editBlogPostRequest.Id});
		}
	}
}







