using EndpointManager.Domain.Entity;

namespace EndpointManager.Domain.Repository;

    public interface IEndpointRepository
    {

        Endpoint? FindEndpointBySerialNumber(string serialNumber);
        List<Endpoint> ListEndpoints();
        Endpoint SaveEndpoint(Endpoint endpoint);
        bool DeleteEndpointBySerialNumber(string serialNumber);
    }

