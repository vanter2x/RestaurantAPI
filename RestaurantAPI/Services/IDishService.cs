using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int CreateDish(int restaurantId, CreateDishDto dto);
        DishDto GetDish(int restaurantId, int dishId);
        List<DishDto> GetAllDishes(int restaurantId);
        void DeleteDishes(int restaurantId);
        void DeleteDish(int restaurantId, int dishId);
    }
}
