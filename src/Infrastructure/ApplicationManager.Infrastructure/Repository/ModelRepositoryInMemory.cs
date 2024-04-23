using EndpointManager.Domain.Repository;

namespace ApplicationManager.Infrastructure.Repository;

public class ModelRepositoryInMemory : IModelRepository
{
    private Dictionary<string, int> InMemoryModelRepository;

    public ModelRepositoryInMemory()
    {
        InMemoryModelRepository = new Dictionary<string, int>() {
            {"NSX1P2W", 16 },
            {"NSX1P3W", 17 },
            {"NSX2P3W", 18 },
            {"NSX3P4W", 19 }
        };
    }

    public int GetDefaultModelIdBySerialNumber(string serialNumber)
    {
        return InMemoryModelRepository.Where(kv => kv.Key == serialNumber)
        .Select(kv => kv.Value)
        .FirstOrDefault();
    }
}