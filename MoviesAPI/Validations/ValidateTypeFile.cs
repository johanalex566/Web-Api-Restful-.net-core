using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MoviesAPI.Validations
{
    public class ValidateTypeFile : ValidationAttribute
    {
        private readonly string[] validTypeFiles;

        public ValidateTypeFile(TypeFiles typeFiles)
        {
            if (typeFiles == TypeFiles.Image)
            {
                this.validTypeFiles = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (!validTypeFiles.Contains(formFile.ContentType))
            {
                return new ValidationResult($"The file type must be one of following: { string.Join(",", validTypeFiles) }");
            }
            return ValidationResult.Success;
        }
    }
}
