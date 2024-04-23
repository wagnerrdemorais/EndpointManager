using EndpointManager.Domain.Entity;
using EndpointManager.Domain.Exceptions;
using EndpointManager.Domain.Repository;
using EndpointManager.Domain.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace EndpointManager.UnitTests.Domain.Service;

public class EndpointServiceTest
{

    [Fact(DisplayName = nameof(FindEndpointOK))]
    [Trait("Domain", "EndpointService - Services")]
    public void FindEndpointOK()
    {
        var mockEndpointRepository = new Mock<IEndpointRepository>();
        var mockModelRepository = new Mock<IModelRepository>();

        var serialNumber = "SerialNumber";
        Endpoint endpoint = new Endpoint(serialNumber, 1234, "MeterNumber", "MeterFirmwareVersion", 0);
        mockEndpointRepository.Setup(repo => repo.FindEndpointBySerialNumber(serialNumber))
                      .Returns(endpoint);

        var subject = new EndpointService(mockEndpointRepository.Object, mockModelRepository.Object);

        Endpoint response = subject.FindEndpoint(serialNumber);

        Assert.Equal(endpoint, response);
    }

    [Fact(DisplayName = nameof(FindEndpointNotFound))]
    [Trait("Domain", "EndpointService - Services")]
    public void FindEndpointNotFound()
    {
        var mockEndpointRepository = new Mock<IEndpointRepository>();
        var mockModelRepository = new Mock<IModelRepository>();

        var serialNumber = "SerialNumber";
        Endpoint endpoint = new Endpoint(serialNumber, 1234, "MeterNumber", "MeterFirmwareVersion", 0);
        mockEndpointRepository.Setup(repo => repo.FindEndpointBySerialNumber(serialNumber))
                      .Returns(endpoint);

        var subject = new EndpointService(mockEndpointRepository.Object, mockModelRepository.Object);

        Action action = () => subject.FindEndpoint("notOK");

        action.Should().Throw<EndpointValidationException>().WithMessage("Endpoint with serial number notOK was not found!");
    }

    [Fact(DisplayName = nameof(ListEndpointsOK))]
    [Trait("Domain", "EndpointService - Services")]
    public void ListEndpointsOK()
    {
        var mockEndpointRepository = new Mock<IEndpointRepository>();
        var mockModelRepository = new Mock<IModelRepository>();

        Endpoint endpoint = new Endpoint("SerialNumber", 1234, "MeterNumber", "MeterFirmwareVersion", 0);
        Endpoint endpoint1 = new Endpoint("SerialNumber1", 12345, "MeterNumber1", "MeterFirmwareVersion1", 0);
        mockEndpointRepository.Setup(repo => repo.ListEndpoints())
                      .Returns(new List<Endpoint>() { endpoint, endpoint1 });

        var subject = new EndpointService(mockEndpointRepository.Object, mockModelRepository.Object);

        var response = subject.ListEndpoints();

        response.Should().HaveCount(2);
    }


    [Fact(DisplayName = nameof(CreateEndpointOK))]
    [Trait("Domain", "EndpointService - Services")]
    public void CreateEndpointOK()
    {
        var mockEndpointRepository = new Mock<IEndpointRepository>();
        var mockModelRepository = new Mock<IModelRepository>();

        Endpoint endpoint = new Endpoint("SerialNumber", 1234, "MeterNumber", "MeterFirmwareVersion", 0);
        mockEndpointRepository.Setup(repo => repo.SaveEndpoint(It.IsAny<Endpoint>()))
                      .Returns(endpoint);

        var subject = new EndpointService(mockEndpointRepository.Object, mockModelRepository.Object);

        var response = subject.CreateEndpoint(endpoint.SerialNumber, endpoint.ModelId, endpoint.MeterNumber, endpoint.MeterFirmwareVersion, endpoint.SwitchState);

        Assert.Equal(endpoint.SerialNumber, response.SerialNumber);
        Assert.Equal(endpoint.ModelId, response.ModelId);
        Assert.Equal(endpoint.MeterNumber, response.MeterNumber);
        Assert.Equal(endpoint.MeterFirmwareVersion, response.MeterFirmwareVersion);
        Assert.Equal(endpoint.SwitchState, response.SwitchState);
    }

    [Fact(DisplayName = nameof(CreateEndpointAlreadyExists))]
    [Trait("Domain", "EndpointService - Services")]
    public void CreateEndpointAlreadyExists()
    {
        var mockEndpointRepository = new Mock<IEndpointRepository>();
        var mockModelRepository = new Mock<IModelRepository>();

        Endpoint endpoint = new Endpoint("SerialNumber", 1234, "MeterNumber", "MeterFirmwareVersion", 0);
        mockEndpointRepository.Setup(repo => repo.FindEndpointBySerialNumber(endpoint.SerialNumber))
              .Returns(endpoint);

        var subject = new EndpointService(mockEndpointRepository.Object, mockModelRepository.Object);

        Action action = () => subject.CreateEndpoint(endpoint.SerialNumber, endpoint.ModelId, endpoint.MeterNumber, endpoint.MeterFirmwareVersion, endpoint.SwitchState);

        action.Should().Throw<EndpointValidationException>().WithMessage("Endpoint serial number already exists!");
    }

    [Fact(DisplayName = nameof(EditEndpointOK))]
    [Trait("Domain", "EndpointService - Services")]
    public void EditEndpointOK()
    {
        var mockEndpointRepository = new Mock<IEndpointRepository>();
        var mockModelRepository = new Mock<IModelRepository>();

        var capturedEndpoint = default(Endpoint);

        Endpoint endpoint = new Endpoint("SerialNumber", 1234, "MeterNumber", "MeterFirmwareVersion", 0);
        mockEndpointRepository.Setup(repo => repo.FindEndpointBySerialNumber(endpoint.SerialNumber))
              .Returns(endpoint);

        mockEndpointRepository.Setup(repo => repo.SaveEndpoint(It.IsAny<Endpoint>()))
                      .Callback<Endpoint>(endpoint => capturedEndpoint = endpoint)
                      .Returns((Endpoint endpoint) => endpoint);

        var subject = new EndpointService(mockEndpointRepository.Object, mockModelRepository.Object);

        var response = subject.EditEndpoint(endpoint.SerialNumber, 2);

        Assert.NotNull(capturedEndpoint);
        Assert.Equal(2, response.SwitchState);
    }

    [Fact(DisplayName = nameof(DeleteEndpointOK))]
    [Trait("Domain", "EndpointService - Services")]
    public void DeleteEndpointOK()
    {
        var mockEndpointRepository = new Mock<IEndpointRepository>();
        var mockModelRepository = new Mock<IModelRepository>();

        var capturedSerialNumber = default(string);

        Endpoint endpoint = new Endpoint("SerialNumber", 1234, "MeterNumber", "MeterFirmwareVersion", 0);
        Endpoint endpoint1 = new Endpoint("SerialNumber1", 12345, "MeterNumber1", "MeterFirmwareVersion1", 1);

        mockEndpointRepository.Setup(repo => repo.FindEndpointBySerialNumber(endpoint.SerialNumber))
        .Returns(endpoint);

        mockEndpointRepository.Setup(repo => repo.DeleteEndpointBySerialNumber(It.IsAny<string>()))
                      .Callback<String>(serialNumber => capturedSerialNumber = serialNumber)
                      .Returns(true);

        var subject = new EndpointService(mockEndpointRepository.Object, mockModelRepository.Object);

        subject.DeleteEndpoint(endpoint.SerialNumber);

        Assert.NotNull(capturedSerialNumber);
        Assert.Equal(endpoint.SerialNumber, capturedSerialNumber);
    }

    [Fact(DisplayName = nameof(GetDefauldModelOK))]
    [Trait("Domain", "EndpointService - Services")]
    public void GetDefauldModelOK()
    {
        var mockEndpointRepository = new Mock<IEndpointRepository>();
        var mockModelRepository = new Mock<IModelRepository>();

        var serialNumber = "SerialNumber";
        Endpoint endpoint = new Endpoint(serialNumber, 1234, "MeterNumber", "MeterFirmwareVersion", 0);
        mockModelRepository.Setup(repo => repo.GetDefaultModelIdBySerialNumber(serialNumber))
                      .Returns(19);

        var subject = new EndpointService(mockEndpointRepository.Object, mockModelRepository.Object);

        int response = subject.GetDefaultModel(serialNumber);

        Assert.Equal(19, response);
    }
}