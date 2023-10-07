using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCCrud.Models
{
    public class AddProductsModel
    {
        public string ProductName { get; set; }
        public List<SelectListItem> Category { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
