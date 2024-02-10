using BookManager.Data;
using BookManager.Interfaces;
using BookManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookManager.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Book book)
        {
            _context.Add(book);
            return Save();
        }
        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _context.Books.Include(book => book.Author).ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books.Include(book => book.Author).FirstOrDefaultAsync(item => item.BookID == id);
        }
        public async Task<Book> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Books.Include(book => book.Author).AsNoTracking().FirstOrDefaultAsync(item => item.BookID == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool Delete(Book book)
        {
            _context.Remove(book);
            return Save();
        }
        public bool Update(Book book)
        {
            _context.Update(book);
            return Save();
        }
    }
}
