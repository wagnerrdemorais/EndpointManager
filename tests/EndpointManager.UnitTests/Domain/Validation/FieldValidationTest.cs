using FluentAssertions;
using Xunit;
using Bogus;
using EndpointManager.Domain.Validation;
using EndpointManager.Domain.Exceptions;

namespace EndpointManager.UnitTests.Domain.Validation
{
    public class FieldValidationTest
    {

        [Trait("Domain", "FieldValidation - Validators")]
        [Theory(DisplayName =nameof(NotNullOrEmptyOK))]
        [MemberData(nameof(GetValidField), parameters:10)]
        public void NotNullOrEmptyOK(string? field)
        {
            Action action = () => FieldValidation.NotNullOrEmpty(field, nameof(field));
            action.Should().NotThrow();
        }

        [Trait("Domain", "FieldValidation - Validators")]
        [Theory(DisplayName = nameof(NotNullOrEmptyThrow))]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void NotNullOrEmptyThrow(string? field)
        {
            Action action = () => FieldValidation.NotNullOrEmpty(field, nameof(field));
            Assert.Throws<EndpointValidationException>(action);
        }

        [Trait("Domain", "FieldValidation - Validators")]
        [Theory(DisplayName = nameof(MinLengthOK))]
        [MemberData(nameof(GetValidField), parameters: 10)]
        public void MinLengthOK(string? field)
        {
            Action action = () => FieldValidation.MinLength(field, 1, nameof(field));
            action.Should().NotThrow();
        }

        [Trait("Domain", "FieldValidation - Validators")]
        [Fact(DisplayName = nameof(MinLengthThrow))]
        public void MinLengthThrow()
        {
            var field = "1234567";
            Action action = () => FieldValidation.MinLength(field, 10, nameof(field));
            Assert.Throws<EndpointValidationException>(action);
        }


        [Trait("Domain", "FieldValidation - Validators")]
        [Theory(DisplayName = nameof(MaxLengthOK))]
        [MemberData(nameof(GetValidField), parameters: 10)]
        public void MaxLengthOK(string? field)
        {
            Action action = () => FieldValidation.MaxLength(field, 255, nameof(field));
            action.Should().NotThrow();
        }

        [Trait("Domain", "FieldValidation - Validators")]
        [Fact(DisplayName = nameof(MaxLengthThrow))]
        public void MaxLengthThrow()
        {
            var field = "01234567789";
            Action action = () => FieldValidation.MaxLength(field, 10, nameof(field));
            Assert.Throws<EndpointValidationException>(action);
        }

        [Trait("Domain", "FieldValidation - Validators")]
        [Theory(DisplayName = nameof(StandardStringValidationOK))]
        [MemberData(nameof(GetValidField), parameters: 10)]
        public void StandardStringValidationOK(string? field)
        {
            Action action = () => FieldValidation.StandardStringValidation(field, nameof(field));
            action.Should().NotThrow();
        }

        [Trait("Domain", "FieldValidation - Validators")]
        [Fact(DisplayName = nameof(StandardStringValidationThrowMaxRange))]
        public void StandardStringValidationThrowMaxRange()
        {
            var invalidField = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
            Action action = () => FieldValidation.StandardStringValidation(invalidField, nameof(invalidField));
            Assert.Throws<EndpointValidationException>(action);
        }

        [Trait("Domain", "FieldValidation - Validators")]
        [Fact(DisplayName = nameof(StandardStringValidationThrowMinRange))]
        public void StandardStringValidationThrowMinRange()
        {
            var invalidField = "ab";
            Action action = () => FieldValidation.StandardStringValidation(invalidField, nameof(invalidField));
            Assert.Throws<EndpointValidationException>(action);
        }

        [Trait("Domain", "FieldValidation - Validators")]
        [Theory(DisplayName = nameof(StandardStringValidationThrowEmptyNull))]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void StandardStringValidationThrowEmptyNull(string field)
        {
            Action action = () => FieldValidation.StandardStringValidation(field, nameof(field));
            Assert.Throws<EndpointValidationException>(action);
        }

        public static IEnumerable<object[]> GetValidField(int parameters)
        {
            var Faker = new Faker();
            for (int i = 0; i <= parameters; i++)
            {
                var example = Faker.Commerce.ProductName();
                if (example.Length > 255)
                {
                    example = example[..255];
                }
                if (example.Length < 3)
                {
                    example = "test";
                }
                yield return new object[] { example };
            }
        }
    }
}
