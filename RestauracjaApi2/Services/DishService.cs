using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestauracjaApi2.Entities;
using RestauracjaApi2.Exceptions;
using RestauracjaApi2.Interfaces;
using RestauracjaApi2.Models;

namespace RestauracjaApi2.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<DishService> logger;

        public DishService(RestaurantDbContext dbContext, IMapper mapper, ILogger<DishService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant == null) throw new NotFoundException($"Restauracja z id = {restaurantId}, nie znaleziono");

            var dish = mapper.Map<Dish>(dto);
            dish.RestaurantId = restaurantId;
            dbContext.Dishes.Add(dish);
            dbContext.SaveChanges();

            return dish.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant == null) throw new NotFoundException($"Restauracja z id = {restaurantId}, nie znaleziono");

            var dish = restaurant.Dishes.Where(d => d.Id == dishId).FirstOrDefault();
            if (dish is null || dish.RestaurantId != restaurantId) throw new NotFoundException($"Nie znaleziono dania o id = {dishId}");

            var mappedDto = mapper.Map<DishDto>(dish);
            return mappedDto;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = dbContext
                .Restaurants
                .Include (r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null) throw new NotFoundException($"Restauracja z id = {restaurantId}, nie znaleziono");

            var dishesDto = mapper.Map<List<DishDto>>(restaurant.Dishes);
            return dishesDto;
        }

        public void RemoveAllDishes(int restaurantId)
        {
            var restaurant = dbContext
                .Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null) throw new NotFoundException($"Restauracja z id = {restaurantId}, nie znaleziono");

            dbContext.RemoveRange(restaurant.Dishes);
            dbContext.SaveChanges();
        }

        public void RemoveById(int restaurantId, int dishId)
        {
            var restaurant = dbContext
                .Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null) throw new NotFoundException($"Restauracja z id = {restaurantId}, nie znaleziono");

            var dish = restaurant.Dishes.Where(d => d.Id == dishId).FirstOrDefault();
            if (dish is null || dish.RestaurantId != restaurantId) throw new NotFoundException($"Nie znaleziono dania o id = {dishId}");

            dbContext.Remove(dish); dbContext.SaveChanges();
        }
    }
}
