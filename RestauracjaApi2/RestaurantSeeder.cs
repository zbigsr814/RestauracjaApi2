using RestauracjaApi2.Entities;

namespace RestauracjaApi2
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            List<Role> roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Admin"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "User"
                }
            };
            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = new List<Restaurant>(){
                new Restaurant
                {
                    Name = "KFC",
                    Description = "Original USA Restaurant",
                    Dishes = new List<Dish>()
                    {
                        new Dish
                        {
                            Name = "Wings",
                            Description = "Chicken Wings",
                            Prise = 12
                        }
                    },
                    Address = new Address(){
                        City = "Gliwice",
                        Street = "Kujawska 3",
                        PostalCode = "11-100"
                    }
                },
                new Restaurant
                {
                    Name = "Kuchnia Polska",
                    Description = "Original PL Restaurant",
                    Dishes = new List<Dish>()
                        {
                            new Dish
                            {
                                Name = "Schabowy",
                                Description = "Schabowy",
                                Prise = 12
                            },
                            new Dish
                            {
                                Name = "Gołąbki",
                                Description = "Gołąbki",
                                Prise = 12
                            },
                            new Dish
                            {
                                Name = "Frytki",
                                Description = "Frytki",
                                Prise = 11
                            }
                        },
                    Address = new Address()
                    {
                        City = "Katowice",
                        Street = "Kujawska 3",
                        PostalCode = "11-100"
                    }
                }
            };
            return restaurants;
        }
    }
}
