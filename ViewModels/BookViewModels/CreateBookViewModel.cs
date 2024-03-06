using BookManager.Data.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookManager.ViewModels.BookViewModels
{
    public class CreateBookViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string BookShortInfo { get; set; }
        public string? AuthorShortInfo { get; set; }
        public Genre Genre { get; set; }
        public IFormFile Image { get; set; }
        public double Rating { get; set; }
    }
}
