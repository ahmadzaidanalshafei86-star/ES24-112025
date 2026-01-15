namespace ES.Web.Models
{
    public class GalleryImageViewModel
    {
        public int PageId { get; set; }   // ضروري لربط الصور بالصفحة
        public string GalleryImageUrl { get; set; } = null!;
        public string GalleryImageAltName { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
}
