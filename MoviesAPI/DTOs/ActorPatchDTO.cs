using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class ActorPatchDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
