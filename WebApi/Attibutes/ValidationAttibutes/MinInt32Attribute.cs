using System.ComponentModel.DataAnnotations;

namespace WebApi.Attibutes.ValidationAttibutes
{
    public class MinInt32Attribute : ValidationAttribute
    {
        private int _min { get; }
        private bool _includeMin { get; }

        private string GetErrorMessage()
        {
            if (_includeMin)
                return $"The value must be greater than or equal to {_min}";
            else
                return $"The value must be greater than {_min}";
        }

        public MinInt32Attribute(int min, bool includeMin = true)
        {
            _min = min;
            _includeMin = includeMin;
        }

        public MinInt32Attribute(int min, Func<string> errorMessageAccessor, bool includeMin = true) : base(errorMessageAccessor)
        {
            _min = min;
            _includeMin = includeMin;
        }

        public MinInt32Attribute(int min, string errorMessage, bool includeMin = true) : base(errorMessage)
        {
            _min = min;
            _includeMin = includeMin;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var number = (int?)value;

            if (number == null || (_min < number || (_includeMin && _min == number)))
                return ValidationResult.Success;

            return new ValidationResult(GetErrorMessage());            
        }
    }
}