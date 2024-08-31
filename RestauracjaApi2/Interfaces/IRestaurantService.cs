using RestauracjaApi2.Models;

namespace RestauracjaApi2.Interfaces
{
    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);
        public IEnumerable<RestaurantDto> GetAll();
        public int Create(RestaurantDto dto);
        public void Delete(int id);
        public void Update(int id, UpdateRestaurantDto updatingModel);
    }
}
