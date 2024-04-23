using EndpointManager.Domain.Entity;
using EndpointManager.Domain.Repository;

namespace ApplicationManager.Infrastructure.Repository
{
    public class EndpointRepositoryInMemory : IEndpointRepository
    {

        private Dictionary<string, Endpoint> InMemoryEndpointRepository;

        public EndpointRepositoryInMemory()
        {
            InMemoryEndpointRepository = new Dictionary<string, Endpoint>();
        }

        public Endpoint? FindEndpointBySerialNumber(string serialNumber)
        {
            return InMemoryEndpointRepository
                .Where(kv => kv.Key == serialNumber)
                .Select(kv => kv.Value)
                .FirstOrDefault();
        }

        public List<Endpoint> ListEndpoints()
        {
            return InMemoryEndpointRepository
                .Select(kv => kv.Value)
                .ToList();
        }

        public Endpoint SaveEndpoint(Endpoint endpoint)
        {
            return InMemoryEndpointRepository[endpoint.SerialNumber] =  endpoint;
        }

        public bool DeleteEndpointBySerialNumber(string serialNumber)
        {
            return InMemoryEndpointRepository.Remove(serialNumber);
        }
    }
}
