using BookManager.Models;

namespace BookManager.ViewModels.BookViewModels
{
    public class BooksIndexViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Author> Authors { get; set; }
    }
}
