namespace ES.Web.Models
{
    public class CategoryPageViewModel
    {
      
        // الصفحات الأساسية
        public IEnumerable<PageViewModel> Pages { get; set; } = new List<PageViewModel>();

        // الأخبار أو الفئات المتعلقة
        public CategoryWithPagesViewModel RelatedNews { get; set; } = new CategoryWithPagesViewModel();
        public CategoryWithPagesViewModel SubCategories { get; set; } = new CategoryWithPagesViewModel();
    }
}
