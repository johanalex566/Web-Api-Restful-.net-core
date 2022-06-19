﻿using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class GenderDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
