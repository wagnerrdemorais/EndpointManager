using EndpointManager.Domain.Exceptions;

namespace EndpointManager.Domain.Validation
{
    public static class FieldValidation
    {
        public static readonly int MIN_LENGTH = 3;
        public static readonly int MAX_LENGTH = 255;

        public static void NotNullOrEmpty(string? target, string fieldName)
        {
            if (String.IsNullOrWhiteSpace(target))
                throw new EndpointValidationException($"{fieldName} Should not be null or empty!");
        }

        public static void MinLength(string target, int minLength, string fieldName)
        {
            if (target.Length < minLength)
                throw new EndpointValidationException($"{fieldName} Should contain at least {minLength} characters!");
        }

        public static void MaxLength(string target, int maxLength, string fieldName)
        {
            if (target.Length > maxLength)
                throw new EndpointValidationException($"{fieldName} should contain less than {maxLength} characters!");
        }

        public static void StandardStringValidation(string target, string fieldName)
        {
            NotNullOrEmpty(target, fieldName);
            MinLength(target, MIN_LENGTH, fieldName);
            MaxLength(target, MAX_LENGTH, fieldName);
        }
    }
}
