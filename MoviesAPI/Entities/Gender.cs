using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Gender : Entity
    {
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
