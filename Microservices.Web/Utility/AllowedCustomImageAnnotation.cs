using System.ComponentModel.DataAnnotations;

namespace Microservices.Web.Utility
{
    public class AllowedCustomImageAnnotation : ValidationAttribute
    {
        private readonly string[] _extension;
        private readonly int _maxFileSize;

        public AllowedCustomImageAnnotation(string[] extension, int maxFileSize)
        {
            this._extension = extension;
            this._maxFileSize = maxFileSize;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extension.Contains(extension.ToLower()))
                {
                    return new ValidationResult("This photo extension is not allowed");
                }
                if (file.Length > (_maxFileSize * 1024 * 1024))
                {
                    return new ValidationResult($"Maximum allowed file size is {_maxFileSize} MB.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
