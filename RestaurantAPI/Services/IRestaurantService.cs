using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetRestaurantById(int id);
        List<RestaurantDto> GetAllRestaurants(RestaurantQuery query);
        int CreateRestaurant(CreateRestaurantDto dto);
        void DeleteRestaurant(int id);
        void UpdateRestaurant(int id, UpdateRestaurantDto dto);

    }
}
