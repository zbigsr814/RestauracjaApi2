using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauracjaApi2.Entities;
using RestauracjaApi2.Interfaces;
using RestauracjaApi2.Models;

namespace RestauracjaApi2.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    //[Authorize]
    public class RestaurantController : ControllerBase
    {
        public IRestaurantService restaurantService { get; }

        public RestaurantController(IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;
        }

        [HttpGet]
        //[Authorize(Roles = "Manager")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll() 
        {
            var restaurantsDto = restaurantService.GetAll();
            return Ok(restaurantsDto);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult CreateRestaurant([FromBody] RestaurantDto dto)
        {
            var id = restaurantService.Create(dto);
            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet("{id}")]
        //[Authorize(Policy = "HasNationality")]
        public ActionResult<IEnumerable<RestaurantDto>> Get([FromRoute] int id) 
        {
            var restaurantDto = restaurantService.GetById(id);
            return Ok(restaurantDto);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            restaurantService.Delete(id);

            return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateRestaurantDto updatingModel)
        {
            restaurantService.Update(id, updatingModel);

            return NotFound();
        }
    }
}
