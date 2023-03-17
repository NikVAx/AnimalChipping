using System.ComponentModel.DataAnnotations;

namespace WebApi.Attibutes.ValidationAttibutes
{
    public class MinSingleAttribute : ValidationAttribute
    {
        private float _min { get; }
        private bool _includeMin { get; }

        private string GetErrorMessage()
        {
            if(_includeMin)
                return $"The value must be greater than or equal to {_min}";
            else
                return $"The value must be greater than {_min}";
        }

        public MinSingleAttribute(float value, bool includeMin = true)
        {
            _min = value;
            _includeMin = includeMin;
        }

        public MinSingleAttribute(float value, Func<string> errorMessageAccessor, bool includeMin = true) : base(errorMessageAccessor)
        {
            _min = value;
            _includeMin = includeMin;
        }

        public MinSingleAttribute(float value, string errorMessage, bool includeMin = true) : base(errorMessage)
        {
            _min = value;
            _includeMin = includeMin;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var number = (float?)value;

            if(number == null || (_min < number || (_includeMin && _min == number)))
                return ValidationResult.Success;

            return new ValidationResult(GetErrorMessage());
        }

    }
}