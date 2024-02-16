using BookManager.Models;

namespace BookManager.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<bool> AddAsync(Author author);
        Task<bool> SaveAsync();
        Task<Author> FindAuthor(string name, string surname);
    }
}
