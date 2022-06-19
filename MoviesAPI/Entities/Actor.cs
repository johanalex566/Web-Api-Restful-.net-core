using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Actor : Entity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Photo { get; set; }
    }
}
