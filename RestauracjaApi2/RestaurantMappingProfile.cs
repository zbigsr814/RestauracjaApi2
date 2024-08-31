using AutoMapper;
using JetBrains.Annotations;
using RestauracjaApi2.Entities;
using RestauracjaApi2.Models;

namespace RestauracjaApi2
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(m => m.City, c => c.MapFrom(r => r.Address.City))
                .ForMember(m => m.Street, s => s.MapFrom(r => r.Address.Street))
                .ForMember(m => m.PostalCode, p => p.MapFrom(r => r.Address.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<RestaurantDto, Restaurant>()
                .ForMember(r => r.Address,
                c => c.MapFrom(dto => new Address()
                { City = dto.City, Street = dto.Street, PostalCode = dto.PostalCode }));

            CreateMap<CreateDishDto, Dish>();
        }
    }
}
