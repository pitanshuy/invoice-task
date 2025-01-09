using invoice_task.DTO;
using System.Threading.Tasks;

namespace invoice_task.Interfaces
{
    public interface IAuthenticationService 
    {
        Task<string> AuthenticateUser(LoginRequestDto loginRequest);

    }
}
