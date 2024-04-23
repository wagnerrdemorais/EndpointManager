namespace EndpointManager.Domain.Repository;

public interface IModelRepository
{
    public int GetDefaultModelIdBySerialNumber(string serialNumber);
}
