using Grand.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Api.Commands.Models.Mobile.Catalog
{
    public class CategoryModel : BaseEntityModel
    {
        public CategoryModel()
        {
            SubCategories = new List<CategoryModel>();
        }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string FlagStyle { get; set; }
        public string Icon { get; set; }
        public string ImageUrl { get; set; }
        public int? NumberOfProducts { get; set; }
        public bool IncludeInTopMenu { get; set; }
        public List<CategoryModel> SubCategories { get; set; }
    }
}
