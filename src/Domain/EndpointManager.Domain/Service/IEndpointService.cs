using EndpointManager.Domain.Entity;

namespace EndpointManager.Domain.Service
{
    public interface IEndpointService
    {
        Endpoint CreateEndpoint(string serialNumber, int modelId, string meterNumber, string meterFirmwareVersion, int switchState);
        void DeleteEndpoint(string serialNumber);
        Endpoint EditEndpoint(string serialNumber, int switchState);
        Endpoint FindEndpoint(string serialNumber);
        List<Endpoint> ListEndpoints();
    }
}