
using EndpointManager.Domain.Exceptions;
using EndpointManager.Domain.SeedWork;
using EndpointManager.Domain.Validation;
using System.Text;

namespace EndpointManager.Domain.Entity;

public class Endpoint : BaseEntity
{
    public string SerialNumber { get; private set; }
    public int ModelId { get; private set; }
    public string MeterNumber { get; private set; }
    public string MeterFirmwareVersion { get; private set; }
    public int SwitchState { get; private set; }

    public Endpoint(string serialNumber, int modelId, string meterNumber, string meterFirmwareVersion, int switchState) : base()
    {
        SerialNumber = serialNumber;
        ModelId = modelId;
        MeterNumber = meterNumber;
        MeterFirmwareVersion = meterFirmwareVersion;
        SwitchState = switchState;
        Validate();
    }

    private void Validate()
    {
        FieldValidation.StandardStringValidation(SerialNumber, nameof(SerialNumber));
        FieldValidation.StandardStringValidation(MeterNumber, nameof(MeterNumber));
        FieldValidation.StandardStringValidation(MeterFirmwareVersion, nameof(MeterFirmwareVersion));

        if (!SwitchStates.IsDefined(typeof(SwitchStates), SwitchState))
            throw new EndpointValidationException($"Invalid switch state: {SwitchState}");
    }

    internal void ChangeSwitchState(int switchState)
    {
        SwitchState = switchState;
        Validate();
    }

    private enum SwitchStates
    {
        Disconnected = 0,
        Connected = 1,
        Armed = 2
    }

    public override string ToString()
    {
        return GetType().GetProperties()
            .Select(info => (info.Name, Value: info.GetValue(this, null) ?? "(null)"))
            .Aggregate(
                new StringBuilder(),
                (sb, pair) => sb.AppendLine($"{pair.Name}: {pair.Value}"),
                sb => sb.ToString());
    }

}
