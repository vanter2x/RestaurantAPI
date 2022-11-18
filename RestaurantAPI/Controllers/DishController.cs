using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private IDishService _service;

        public DishController(IDishService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            var newDishId = _service.CreateDish(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> GetDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = _service.GetDish(restaurantId, dishId);

            return Ok(dish);
        }

        [HttpGet]
        public ActionResult<List<DishDto>> GetAll(int restaurantId)
        {
            var dishes = _service.GetAllDishes(restaurantId);

            return Ok(dishes);
        }

        [HttpDelete("{dishId}")]
        public ActionResult DeleteDish([FromRoute]int restaurantId, [FromRoute]int dishId)
        {
            _service.DeleteDish(restaurantId, dishId);

            return NoContent();
        }

        [HttpDelete]
        public ActionResult DeleteDishes([FromRoute] int restaurantId)
        {
            _service.DeleteDishes(restaurantId);

            return NoContent();
        }
    }
}
