using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestauracjaApi2.Entities;
using RestauracjaApi2.Exceptions;
using RestauracjaApi2.Interfaces;
using RestauracjaApi2.Models;

namespace RestauracjaApi2.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<RestaurantService> logger;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = dbContext
                .Restaurants
                .Include(r => r.Dishes)
                .Include(r => r.Address)
                .FirstOrDefault(x => x.Id == id);

            var restaurantDto = mapper.Map<RestaurantDto>(restaurant);

            if (restaurant == null) throw new NotFoundException("Nie znaleziono takiej restauracji");
            return restaurantDto;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = dbContext
                .Restaurants
                .Include(r => r.Dishes)
                .Include(r => r.Address)
                .ToList();

            var restaurantsDto = mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDto;
        }

        public int Create(RestaurantDto dto)
        {
            var restaurant = mapper.Map<Restaurant>(dto);
            dbContext.Add(restaurant);
            dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void Delete(int id)
        {
            logger.LogError($"Restaurant with Id:{id} has DELETE action.");
            var restaurant = dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null) throw new NotFoundException("Nie znaleziono takiej restauracji");

            dbContext.Restaurants.Remove(restaurant);
            dbContext.SaveChanges();
        }

        public void Update(int id, UpdateRestaurantDto updatingModel)
        {
            var restaurant = dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null) throw new NotFoundException("Nie znaleziono takiej restauracji");

            if (updatingModel.Name is not null) restaurant.Name = updatingModel.Name;
            if (updatingModel.Description is not null) restaurant.Description = updatingModel.Description;
            if (updatingModel.HasDelivery is not null) restaurant.HasDelivery = (bool)updatingModel.HasDelivery;
            dbContext.SaveChanges(); 
        }
    }
}
