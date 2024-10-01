using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogpostLikesController : ControllerBase
    {
        private readonly IBlogpostLikesRepository blogpostLikesRepository;

        public BlogpostLikesController(IBlogpostLikesRepository blogpostLikesRepository)
        {
            this.blogpostLikesRepository = blogpostLikesRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLikes([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new BlogpostLike
            {
                UserId = addLikeRequest.UserId,
                BlogpostId = addLikeRequest.BlogPostId
            };

           await blogpostLikesRepository.AddLikes(model);

            return Ok();
        }

        
        [HttpGet]
        [Route("{blogpostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogpostId )
        {
            var totalLikes = await blogpostLikesRepository.GetTotalLikes(blogpostId);
            return Ok(totalLikes);
        }
    }
}
