using invoice_task.Domain.Entities;
using System.Threading.Tasks;

namespace invoice_task.Interfaces
{
    public interface IUserRepository
    {


        User GetUserByUsername(string username);

        Task<User> GetByEmailAsync(string email); // Fetches a user by email


    }
}
