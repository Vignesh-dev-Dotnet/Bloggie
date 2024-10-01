﻿using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddBlogPostsRequest
    {
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShotrDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        //Display tags
        public IEnumerable<SelectListItem> Tags {  get; set; }
        
        //Collect or Capture tags
        public string[] SelectedTags { get; set; } = Array.Empty<string>(); 


    }
}


