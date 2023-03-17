using System.ComponentModel.DataAnnotations;

namespace WebApi.Attibutes.ValidationAttibutes
{
    public class MinDoubleAttribute : ValidationAttribute
    {
        private double _min { get; }
        private bool _includeMin { get; }

        private string GetErrorMessage()
        {
            if(_includeMin)
                return $"The value must be greater than or equal to {_min}";
            else
                return $"The value must be greater than {_min}";
        }

        public MinDoubleAttribute(double value, bool includeMin = true)
        {
            _min = value;
            _includeMin = includeMin;
        }

        public MinDoubleAttribute(double value, Func<string> errorMessageAccessor, bool includeMin = true) : base(errorMessageAccessor)
        {
            _min = value;
            _includeMin = includeMin;
        }

        public MinDoubleAttribute(double value, string errorMessage, bool includeMin = true) : base(errorMessage)
        {
            _min = value;
            _includeMin = includeMin;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var number = (double?)value;

            if(number == null || (_min < number || (_includeMin && _min == number)))
                return ValidationResult.Success;

            return new ValidationResult(GetErrorMessage());
        }

    }
}