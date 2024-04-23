using EndpointManager.Domain.Service;
using System.Text;

namespace EndpointManager.ConsoleApp;

internal class App
{

    private readonly IEndpointService _endpointService;
    public App(IEndpointService service)
    {
        _endpointService = service;
    }

    public void Run(string[] args)
    {
        var selectedOption = "";
        printMainMenu();
        selectedOption = Console.ReadLine();

        string serialNumber = "ABCds";
        int modelId = 1234;
        string meterNumber = "meterNumber";
        string meterFirmwareVersion = "meterFirmVer";
        int switchState = 0;

        _endpointService.CreateEndpoint(serialNumber, modelId, meterNumber, meterFirmwareVersion, switchState);
        var endpoints = _endpointService.ListEndpoints();
        endpoints.ForEach(endpoint => {
            Console.WriteLine(endpoint.ToString());
        });
    }

    private void printMainMenu()
    {
        var optionsMenuSBuilder = new StringBuilder();
        optionsMenuSBuilder.AppendLine("Welcome! Please enter the desired option:");
        optionsMenuSBuilder.AppendLine("------------------------------------------------------");
        optionsMenuSBuilder.AppendLine("1) - Insert a new endpoint.");
        optionsMenuSBuilder.AppendLine("2) - Edit an existing endpoint.");
        optionsMenuSBuilder.AppendLine("3) - Delete an existing endpoint.");
        optionsMenuSBuilder.AppendLine("4) - List all endpoints.");
        optionsMenuSBuilder.AppendLine("5) - Find an endpoint by 'Endpoint Serial Number'.");
        optionsMenuSBuilder.AppendLine("6) - Exit.");
        optionsMenuSBuilder.AppendLine("------------------------------------------------------");
        Console.WriteLine(optionsMenuSBuilder.ToString());
    }

}
