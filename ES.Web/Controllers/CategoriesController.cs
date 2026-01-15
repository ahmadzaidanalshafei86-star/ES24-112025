using ES.Web.Models;
using ES.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ES.Web.Controllers.Categories
{
    [AllowAnonymous]
    [Route("Categories")]
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // دعم كلا الشكلين: /Categories/News أو /Categories?slug=News
        [HttpGet("{slug?}")]
        public async Task<IActionResult> Index(string slug, int page = 1, int pageSize = 6)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            // جلب الفئة الرئيسية مع الصفحات
            var model = await _categoryService.GetCategoryWithPagesBySlugAsync(slug);
            if (model == null)
                return NotFound();

            // حفظ slug في ViewModel لاستخدامه في Paging links
            model.Slug = slug;

            // الفئات الفرعية
            model.Subcategories = await _categoryService.GetSubCategoriesByParentIdAsync(model.Id);

            // الأخبار المرتبطة (Many-to-Many)
            model.RelatedNewsList = await _categoryService.GetCategoryWithRelatedNewsAsync(model.Id);

            // Paging
            var totalItems = model.Pages?.Count ?? 0;
            model.Pages = model.Pages?
                .OrderByDescending(x => x.DateInput)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            model.CurrentPage = page;
            model.PageSize = pageSize;
            model.TotalItems = totalItems;

            // عرض الصفحة
            return View("ViewCategory", model);
        }
    }
}
