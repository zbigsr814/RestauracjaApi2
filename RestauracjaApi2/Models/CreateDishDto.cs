using System.ComponentModel.DataAnnotations;

namespace RestauracjaApi2.Models
{
    public class CreateDishDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Prise { get; set; }

        public int RestaurantId { get; set; }
    }
}
