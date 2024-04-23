using FluentAssertions;
using Xunit;
using EndpointManager.Domain.Entity;
using EndpointManager.Domain.Exceptions;
using EndpointManager.Domain.Validation;

namespace EndpointManager.UnitTests.Domain.Entity
{
    public class EndpointTest
    {
        [Trait("Domain", "Endpoint - Entity")]
        [Fact(DisplayName = nameof(InstantiateOK))]
        public void InstantiateOK()
        {
            string serialNumber = "abcd";
            int modelId = 1;
            string meterNumber = "meterNumber";
            string meterFirmwareVersion = "meterFirmwareVersion";
            int switchState = 1;

            Action action = () => new Endpoint(serialNumber, modelId, meterNumber, meterFirmwareVersion, switchState);

            action.Should().NotThrow();
            action.Should().NotBeNull();
        }

        [Trait("Domain", "Endpoint - Entity")]
        [Fact(DisplayName = nameof(InstantiateThrowInvalidMinSerialNumber))]
        public void InstantiateThrowInvalidMinSerialNumber()
        {
            string serialNumber = "ab";
            int modelId = 1;
            string meterNumber = "meterNumber";
            string meterFirmwareVersion = "meterFirmwareVersion";
            int switchState = 1;

            Action action = () => new Endpoint(serialNumber, modelId, meterNumber, meterFirmwareVersion, switchState);

            action.Should()
                .Throw<EndpointValidationException>()
                .WithMessage($"{nameof(serialNumber)} Should contain at least {FieldValidation.MIN_LENGTH} characters!");
        }

        [Trait("Domain", "Endpoint - Entity")]
        [Fact(DisplayName = nameof(InstantiateThrowInvalidMinMeterNumber))]
        public void InstantiateThrowInvalidMinMeterNumber()
        {
            string serialNumber = "abcd";
            int modelId = 1;
            string meterNumber = "ab";
            string meterFirmwareVersion = "meterFirmwareVersion";
            int switchState = 1;

            Action action = () => new Endpoint(serialNumber, modelId, meterNumber, meterFirmwareVersion, switchState);

            action.Should()
                .Throw<EndpointValidationException>()
                .WithMessage($"{nameof(meterNumber)} Should contain at least {FieldValidation.MIN_LENGTH} characters!");
        }
    }
}
