using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Validations
{
    public class ValidateSizeFile:ValidationAttribute
    {
        private readonly int maxSizeMegaBytes;

        public ValidateSizeFile(int maxSizeMegaBytes)
        {
            this.maxSizeMegaBytes = maxSizeMegaBytes;
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

            int validSize = maxSizeMegaBytes * 1024 * 1024; //conver to bytes

            if (formFile.Length > validSize)
            {
                return new ValidationResult($"The size of file must not be greater than {validSize} bytes");
            }

            return ValidationResult.Success;
        }
    }
}
