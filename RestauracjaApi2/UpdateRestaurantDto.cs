using System.ComponentModel.DataAnnotations;

namespace RestauracjaApi2
{
    public class UpdateRestaurantDto
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? HasDelivery { get; set; }
    }
}
