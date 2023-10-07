using MVCCrud.Models.Domain;

namespace MVCCrud.Models
{
    public class ProductViewModel
    {
        public IQueryable<Product> ProductData { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
