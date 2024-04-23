using EndpointManager.Domain.Entity;
using EndpointManager.Domain.Exceptions;
using EndpointManager.Domain.Repository;

namespace EndpointManager.Domain.Service
{
    public class EndpointService : IEndpointService
    {

        private readonly IEndpointRepository EndpointRepository;
        private readonly IModelRepository ModelRepository;

        public EndpointService(IEndpointRepository endpointRepository, IModelRepository modelRepository)
        {
            EndpointRepository = endpointRepository;
            ModelRepository = modelRepository;
        }

        public List<Endpoint> ListEndpoints()
        {
            return EndpointRepository.ListEndpoints();
        }

        public Endpoint FindEndpoint(string serialNumber)
        {
            return GetEndpointOrThrow(serialNumber);
        }

        public Endpoint CreateEndpoint(string serialNumber, int modelId, string meterNumber, string meterFirmwareVersion, int switchState)
        {
            ThrowExceptionIfEndpointExists(serialNumber);

            int existingModelID = ModelRepository.GetDefaultModelIdBySerialNumber(serialNumber);
            Endpoint newEndpoint = new Endpoint(serialNumber, existingModelID != 0 ? existingModelID : modelId, meterNumber, meterFirmwareVersion, switchState);
            return EndpointRepository.SaveEndpoint(newEndpoint);
        }

        public Endpoint EditEndpoint(string serialNumber, int switchState)
        {
            var endpoint = GetEndpointOrThrow(serialNumber);
            endpoint.ChangeSwitchState(switchState);
            return EndpointRepository.SaveEndpoint(endpoint);
        }

        public void DeleteEndpoint(string serialNumber)
        {
            var endpoint = GetEndpointOrThrow(serialNumber);
            bool deleted = EndpointRepository.DeleteEndpointBySerialNumber(serialNumber);
            if (!deleted)
                throw new Exception($"Could not remove endpoint with serial number: {serialNumber}!");
        }

        private Endpoint GetEndpointOrThrow(string serialNumber)
        {
            var endpoint = EndpointRepository.FindEndpointBySerialNumber(serialNumber);
            if (endpoint == null)
                throw new EndpointValidationException($"Endpoint with serial number {serialNumber} was not found!");
            else
                return endpoint;
        }

        private void ThrowExceptionIfEndpointExists(string serialNumber)
        {
            if (EndpointRepository.FindEndpointBySerialNumber(serialNumber) != null)
                throw new EndpointValidationException("Endpoint serial number already exists!");
        }
    }
}
