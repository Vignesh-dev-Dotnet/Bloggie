namespace Bloggie.Web.Models.Domain
{
	public class BlogpostLike
	{
        public Guid Id { get; set; }
		public Guid BlogpostId { get; set; }
		public Guid UserId { get; set; }
    }
}
