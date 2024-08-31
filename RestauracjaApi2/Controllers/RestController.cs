using Microsoft.AspNetCore.Mvc;
using RestauracjaApi2.Entities;

namespace RestauracjaApi2.Controllers
{

    public class RestController : ControllerBase
    {
        private readonly RestaurantDbContext dbContext;

        public RestController(RestaurantDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAll()
        {
            var restaurants = dbContext
                .Restaurants
                .ToList();

            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<string>> Get([FromRoute] int id)
        {
            var restaurant = dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }
    }
}
