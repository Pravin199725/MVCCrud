using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCrud.Data;
using MVCCrud.Models;
using MVCCrud.Models.Domain;
using cloudscribe.Pagination.Models;


namespace MVCCrud.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MVCDbContext mVCDbContext;

        public ProductsController(MVCDbContext mVCDbContext)
        {
            this.mVCDbContext = mVCDbContext;
        }


        
        public async Task<IActionResult> Index(int pageSize=10, int pageNumber = 1)
        {
            int exclude = (pageSize * pageNumber) - pageSize;
            //var prodData = new ProductViewModel();
            var products = (from prod in mVCDbContext.Products
                            select new Product
                            {
                                Id = prod.Id,
                                ProductName = prod.ProductName,
                                CategoryId = prod.CategoryId,
                                CategoryName = prod.CategoryName

                            }).Skip(exclude)
                            .Take(pageSize);
            // prodData.ProductData = products;

            var result = new PagedResult<MVCCrud.Models.Domain.Product>
            {
                Data = products.AsNoTracking().ToList(),
                TotalItems = mVCDbContext.Products.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            //int totalRecords = products.Count();
            ////int pageSize = 10;
            ////int totalPage = (int)Math.Ceiling(totalRecords / (double)pageSize);
            ////products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
            ////prodData.ProductData = products;
            //prodData.CurrentPage = pageNumber;
            //prodData.TotalPages = totalRecords;
            //prodData.PageSize = pageSize;
            return View(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddProductsModel viewModel = new AddProductsModel();
            viewModel.Category = PopulateCategory();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var products = await mVCDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (products != null)
            {
                var viewmodel = new UpdateProductModel()
                {
                    Id = products.Id,
                    ProductName = products.ProductName,
                    Category = PopulateCategory(),
                    CategoryId = products.CategoryId,
                    CategoryName = products.CategoryName
            };
                return await Task.Run(() => View("View", viewmodel));

            }
            return RedirectToAction("Index");
        }


        private List<SelectListItem> PopulateCategory()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var categories =  mVCDbContext.Categories.ToList();
            foreach ( var category in categories )
            {
                items.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }
            return items;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductsModel addProducts)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                ProductName = addProducts.ProductName,
                CategoryId = addProducts.CategoryId,
                CategoryName = addProducts.CategoryName
            };

            await mVCDbContext.Products.AddAsync(product);
            await mVCDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateProductModel model)
        {
            var products = await mVCDbContext.Products.FindAsync(model.Id);

            if (products != null)
            {
                products.ProductName = model.ProductName;
                products.CategoryId = model.CategoryId;
                products.CategoryName = model.CategoryName;
            }

            await mVCDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateProductModel model)
        {
            var product = await mVCDbContext.Products.FindAsync(model.Id);

            if (product != null)
            {
                mVCDbContext.Products.Remove(product);
                await mVCDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
