using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();

                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }

            };
            return roles;
        }

        //private IEnumerable<User> GetUsers()
        //{
        //    var users = new List<User>()
        //    {
        //        new User()
        //        {
        //            Name = "Kris",
        //            LastName = "Gab",
        //            Email = "kris@dd.pl",
        //            Nationality = "Poland",
        //            DateOfBirth = new DateTime(1980,12,12),
        //            Role = new Role()
        //            {
        //                Name = "User"
        //            }
        //        }
        //    };

        //}

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "Kentucky Fried Chicken",
                    ContactEmail = "asda@o2.pl",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Hot Chicken",
                            Price = 10.00M
                        },

                        new Dish()
                        {
                            Name = "Nuggets",
                            Price = 3.4M
                        }
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Długa 55",
                        PostalCode = "30-123"
                    }
                },
                new Restaurant()
                {
                    Name = "Mc Donald",
                    Category = "Fast Food",
                    Description = "Burgers and Nuggets",
                    ContactEmail = "mc@gg.pl",
                    HasDelivery= false,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Mc Chicken",
                            Price = 3.3M
                        },

                        new Dish()
                        {
                            Name = "Mc Burger",
                            Price = 2.2M
                        }
                    },
                    Address = new Address()
                    {
                        City = "Warszawa",
                        Street = "Staszka 4",
                        PostalCode = "23-333"
                    }
                }
            };
            return restaurants;
        }
    }
}
