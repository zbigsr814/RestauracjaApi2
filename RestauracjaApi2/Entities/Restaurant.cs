﻿using System.ComponentModel.DataAnnotations;

namespace RestauracjaApi2.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDelivery { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }

        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
