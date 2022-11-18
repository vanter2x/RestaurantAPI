﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class DishService : IDishService
    {

        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public int CreateDish(int restaurantId, CreateDishDto dto)
        {
            var restaurant = _dbContext
                .Restaurants?
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found.");

            var dish = _mapper.Map<Dish>(dto);

            dish.RestaurantId = restaurantId;

            _dbContext.Dishes!.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        public List<DishDto> GetAllDishes(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants?
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found.");

            var dishes = restaurant.Dishes.ToList();

            if (dishes is null)
                throw new NotFoundException("Dishes not found.");

            var dishesResult = _mapper.Map<List<DishDto>>(dishes);

            return dishesResult;
        }

        public DishDto GetDish(int restaurantId, int dishId)
        {
            var restaurant = _dbContext.Restaurants?.FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found.");

            var dish = _dbContext.Dishes?.FirstOrDefault(d => d.Id == dishId);

            if (dish is null || dish.RestaurantId != restaurantId)
                throw new NotFoundException("Dish not found.");

            var dishResult = _mapper.Map<DishDto>(dish);

            return dishResult;
        }

        public void DeleteDish(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _dbContext.Dishes?.FirstOrDefault(d => d.RestaurantId == restaurantId && d.Id == dishId);

            if (dish is null)
                throw new NotFoundException("Dish not found");

            _dbContext.Dishes?.Remove(dish);
            _dbContext.SaveChanges();
        }

        public void DeleteDishes(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishes = _dbContext.Dishes?.Where(d => d.RestaurantId == restaurantId);

            if (dishes is null)
                throw new NotFoundException("Dishes not found");

            _dbContext.Dishes?.RemoveRange(dishes);
            _dbContext.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext
                .Restaurants?
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }
    }
}
