using BookManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLibrary> UserLibraries { get; set; }
        public DbSet<WishList> WishLists { get; set; }
    }
}
