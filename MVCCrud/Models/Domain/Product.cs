using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCCrud.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

             
    }
}
