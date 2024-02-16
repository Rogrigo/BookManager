using BookManager.Data;
using BookManager.Interfaces;
using BookManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _context.Authors.ToListAsync();
        }
        public async Task<bool> AddAsync(Author author)
        {
            await _context.AddAsync(author);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<Author> FindAuthor(string name, string surname)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.AuthorName == name && a.AuthorSurname == surname);
        }
    }
}
