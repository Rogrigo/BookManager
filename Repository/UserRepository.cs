using BookManager.Data;
using BookManager.Interfaces;
using BookManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.Repository
{
    public class UserRepository : IUserReporitory
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(User user)
        {
            _context.Users.Add(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var existedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return existedUser;
        }


        public bool Delete(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
