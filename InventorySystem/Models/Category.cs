﻿using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public required string Name { get; set; }

        public List<Product>? Products { get; set; }
    }
}
