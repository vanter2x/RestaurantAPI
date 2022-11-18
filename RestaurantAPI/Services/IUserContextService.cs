using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IUserContextService
    {
        int? GetUderId { get; }
        ClaimsPrincipal User { get; }
    }
}