using RestauracjaApi2.Entities;
using RestauracjaApi2.Models;

namespace RestauracjaApi2.Interfaces
{
    public interface IDishService
    {
        public int Create(int restaurantId, CreateDishDto dto);
        public List<DishDto> GetAll(int restaurantId);
        public DishDto GetById(int restaurantId, int dishId);
        public void RemoveAllDishes(int restaurantId);
        public void RemoveById(int restaurantId, int dishId);
    }
}
