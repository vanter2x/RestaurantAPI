using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Linq.Expressions;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _autorizationservice;
        private readonly IUserContextService _userContext;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger,
            IAuthorizationService authorizationService, IUserContextService userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _autorizationservice = authorizationService;
            _userContext = userContext;
        }

        public RestaurantDto GetRestaurantById(int id)
        {
            _logger.LogWarning("Pobrano encję.");
            var restaurant = _dbContext
                .Restaurants!
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found.");

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;
        }

        public PagedResult<RestaurantDto> GetAllRestaurants(RestaurantQuery query)
        {
            var baseQuery = _dbContext
                .Restaurants?
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .Where(r => query.SearchPhrase == null ||
                query.SearchPhrase.ToLower() == r.Description.ToLower() ||
                query.SearchPhrase.ToLower() == r.Name.ToLower());

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>()
                {
                    {nameof(Restaurant.Name), r => r.Name },
                    {nameof(Restaurant.Description), r => r.Description },
                    {nameof(Restaurant.Category), r => r.Category }
                };

                baseQuery = query.SortDirection == SortDirection.ASC ?
                     baseQuery.OrderBy(columnSelectors[query.SortBy]) :
                     baseQuery.OrderByDescending(columnSelectors[query.SortBy]);
            }

            var restaurants = baseQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize).ToList();

            var totalCount = baseQuery.Count();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return new PagedResult<RestaurantDto>(restaurantsDto, totalCount, query.PageSize, query.PageNumber);

            //return result;
        }

        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContext.GetUderId;
            _dbContext.Restaurants!.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void DeleteRestaurant(int id)
        {
            var restaurant = _dbContext
                .Restaurants!
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found.");

            var authorizationresult = _autorizationservice.AuthorizeAsync(
                _userContext.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationresult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Restaurants!.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public void UpdateRestaurant(int id, UpdateRestaurantDto dto)
        {
            var restaurant = _dbContext
                .Restaurants!
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found.");

            var authorizationresult = _autorizationservice.AuthorizeAsync(
                _userContext.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationresult.Succeeded)
            {
                throw new ForbidException();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            _dbContext.SaveChanges();
        }
    }
}
