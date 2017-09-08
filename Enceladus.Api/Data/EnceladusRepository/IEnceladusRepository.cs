using Enceladus.Api.Models;
using System.Security.Claims;
namespace Enceladus.Api.Data.EnceladusRepository
{
    public interface IEnceladusRepository
    {
        bool SaveAll(ClaimsPrincipal userName);
        bool SaveAll(string email);
        AppUser GetAppUserByEmail(string email);
    }
}