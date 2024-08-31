using Microsoft.AspNetCore.Mvc;
using RestauracjaApi2.Entities;
using RestauracjaApi2.Interfaces;
using RestauracjaApi2.Models;
using RestauracjaApi2.Services;

namespace RestauracjaApi2.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : Controller
    {
        private readonly IDishService dishService;

        public DishController(IDishService dishService)
        {
            this.dishService = dishService;
        }

        [HttpPost]
        public IActionResult Create([FromRoute]int restaurantId, [FromBody]CreateDishDto dto)
        {
            var id = dishService.Create(restaurantId, dto);
            return Created($"/api/{restaurantId}/dish/{id}", null);
        }

        [HttpGet]
        public IActionResult GetAll([FromRoute]int restaurantId)
        {
            var dishDto = dishService.GetAll(restaurantId);
            return Ok(dishDto);
        }

        [HttpGet("{dishId}")]
        public IActionResult Get([FromRoute] int restaurantId, [FromRoute]int dishId)
        {
            var dishDto = dishService.GetById(restaurantId, dishId);
            return Ok(dishDto);
        }

        [HttpDelete]
        public IActionResult RemoveAllDishes([FromRoute] int restaurantId)
        {
            dishService.RemoveAllDishes(restaurantId);
            return NoContent();
        }

        [HttpDelete("{dishId}")]
        public IActionResult RemoveById([FromRoute]int restaurantId, [FromRoute]int dishId)
        {
            dishService.RemoveById(restaurantId, dishId);
            return NoContent();
        }
    }
}
