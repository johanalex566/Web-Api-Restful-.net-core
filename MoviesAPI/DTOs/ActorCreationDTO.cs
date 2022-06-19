using Microsoft.AspNetCore.Http;
using MoviesAPI.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class ActorCreationDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        [ValidateSizeFile(maxSizeMegaBytes: 4)]
        [ValidateTypeFile(typeFiles: TypeFiles.Image)]
        public IFormFile photoFile { get; set; }
    }
}
