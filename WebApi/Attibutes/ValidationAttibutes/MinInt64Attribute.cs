using System.ComponentModel.DataAnnotations;

namespace WebApi.Attibutes.ValidationAttibutes
{
    public class MinInt64Attribute : ValidationAttribute
    {
        private long _min { get; }
        private bool _includeMin { get; }

        private string GetErrorMessage()
        {
            if(_includeMin)
                return $"The value must be greater than or equal to {_min}";
            else
                return $"The value must be greater than {_min}";
        }

        public MinInt64Attribute(long min, bool includeMin = true)
        {
            _min = min;
            _includeMin = includeMin;
        }

        public MinInt64Attribute(long min, Func<string> errorMessageAccessor, bool includeMin = true) : base(errorMessageAccessor)
        {
            _min = min;
            _includeMin = includeMin;
        }

        public MinInt64Attribute(long min, string errorMessage, bool includeMin = true) : base(errorMessage)
        {
            _min = min;
            _includeMin = includeMin;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var number = (long?)value;

            if(number == null || (_min < number || (_includeMin && _min == number)))
                return ValidationResult.Success;

            return new ValidationResult(GetErrorMessage());
        }
    }
}
