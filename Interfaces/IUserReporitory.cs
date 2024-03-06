using BookManager.Models;

namespace BookManager.Interfaces
{
    public interface IUserReporitory
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetUserByEmail(string email);
        bool Add(User user);
        bool Update(User user);
        bool Delete(User user);
        bool Save();
    }
}
