using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bloggie.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminTagsController : Controller
	{
		private readonly ITagRepository tagRepository;

		public AdminTagsController(ITagRepository tagRepository)
        {
			this.tagRepository = tagRepository;
		}

		
        [HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddTagRequest addTagRequest)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var tag = new Tag
			{
				Name = addTagRequest.Name,
				DisplayName = addTagRequest.DisplayName
			};

			await tagRepository.AddAsync(tag);
			
			return RedirectToAction("List");
		}

		[HttpGet]
		public async Task<IActionResult> List(string? searchQuery,string? sortBy,string? sortDirection)
		{
			ViewBag.SearchQuery = searchQuery;
			ViewBag.SortBy = sortBy;
			ViewBag.SortDirection = sortDirection;
			var tag = await tagRepository.GetAllAsync(searchQuery,sortBy,sortDirection);
			return View(tag);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			//1 st method
			//var tag = bloggieDbContext.Tags.Find(id);
			//2nd method
			var tag = await tagRepository.GetAsync(id);

			if (tag != null)
			{
				var editTagRequest = new EditTagRequest
				{
					Id = tag.Id,
					Name = tag.Name,
					DisplayName = tag.DisplayName
				};

				return View(editTagRequest);
			}
			else
			{
				return View(null);
			}

		  
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
		{
			var tag = new Tag
			{
				Id = editTagRequest.Id,
				Name = editTagRequest.Name,
				DisplayName = editTagRequest.DisplayName,
			};

			var updatedTag = await tagRepository.UpdateAsync(tag);
			if (updatedTag != null)
			{
				//show succes message
			}
			else
			{
				//show error message
			}



			return RedirectToAction("Edit", new { editTagRequest.Id }); 
		}

		[HttpPost]
		public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
		{
			var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);
			if(deletedTag != null)
			{
				//Show succes notification
				return RedirectToAction("List");
			}
			return RedirectToAction("Edit", new {editTagRequest.Id});
		}
	}
}






