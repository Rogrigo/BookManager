using BookManager.Data.Enum;

namespace BookManager.ViewModels
{
    public class EditBookViewModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public string ShortBookInfo { get; set; }
        public Genre Genre { get; set; }
        public IFormFile? Image { get; set; }
        public string? URL { get; set; }
        public double Rating { get; set; }
    }
}
