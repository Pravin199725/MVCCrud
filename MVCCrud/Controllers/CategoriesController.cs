using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCrud.Data;
using MVCCrud.Models;
using MVCCrud.Models.Domain;
using cloudscribe.Pagination.Models;


namespace MVCCrud.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly MVCDbContext mVCDbContext;

        public CategoriesController(MVCDbContext mVCDbContext)
        {
            this.mVCDbContext = mVCDbContext;
        }

        //[HttpGet]
        //public async Task<IActionResult> Index(string categoryId)
        //{
        //    var categories = await mVCDbContext.Categories.ToListAsync();
        //    return View(categories);
        //}

        [HttpGet]
        public async Task<IActionResult> Index(int pageSize = 10, int pageNumber = 1)
        {
            
            int exclude = (pageSize * pageNumber) - pageSize;
            //var prodData = new ProductViewModel();
            var categories = (from cat in mVCDbContext.Categories
                            select new Category
                            {
                                Id = cat.Id,
                                Name = cat.Name

                            }).Skip(exclude)
                            .Take(pageSize);
            // prodData.ProductData = products;

            var result = new PagedResult<MVCCrud.Models.Domain.Category>
            {
                Data = categories.AsNoTracking().ToList(),
                TotalItems = mVCDbContext.Categories.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return View(result);

        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> Add(AddCategoriesModel addCategories)
        {
            var category = new Category
            { 
                Id = Guid.NewGuid(),
                Name = addCategories.Name 
            };

            await mVCDbContext.Categories.AddAsync(category);
            await mVCDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id) 
        {
            var categories = await mVCDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (categories != null)
            {
                var viewmodel = new UpdateCategoriesModel()
                {
                    Id = categories.Id,
                    Name = categories.Name
                };
            return await Task.Run(() => View("View", viewmodel));
            
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateCategoriesModel model) 
        {
            var categories = await mVCDbContext.Categories.FindAsync(model.Id);

            if (categories != null)
            {
                categories.Name = model.Name;
            }

            await mVCDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateCategoriesModel model) 
        {
            var categories = await mVCDbContext.Categories.FindAsync(model.Id);

            if (categories != null) 
            {
                mVCDbContext.Categories.Remove(categories);
                await mVCDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
