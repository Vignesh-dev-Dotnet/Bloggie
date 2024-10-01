using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Blogpost> Blogposts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }

    }
}
