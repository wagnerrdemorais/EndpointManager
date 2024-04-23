using EndpointManager.Domain.Repository;
using EndpointManager.Domain.Service;
using System.Text;

namespace EndpointManager.ConsoleApp;

internal class App
{

    private readonly IEndpointService _endpointService;
    private readonly Dictionary<string, Action> _availableActions;
    public App(IEndpointService service)
    {
        _endpointService = service;
        _availableActions = GetAvailableActions();
    }

    public void Run(string[] args)
    {
        MainMenu();
    }

    private void MainMenu()
    {
        try
        {
            PrintMainMenuOptions();

            var selectedOption = Console.ReadLine();
            if (_availableActions.ContainsKey(selectedOption))
            {
                _availableActions[selectedOption].Invoke();
            }
            else
            {
                Console.WriteLine("Invalid option selected!");
                MainMenu();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            MainMenu();
        }
    }

    private static void PrintMainMenuOptions()
    {
        var optionsMenuSBuilder = new StringBuilder();
        optionsMenuSBuilder.AppendLine("Please enter the desired option:");
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

    private void InsertEndpointOption()
    {
        string serialNumber;
        int modelId;
        string meterNumber;
        string meterFirmwareVersion;
        int switchState;

        Console.WriteLine("Please insert 'Serial Number' (string)");
        serialNumber = Console.ReadLine();
        var defaultModel = _endpointService.GetDefaultModel(serialNumber);
        if (defaultModel == null || defaultModel == 0)
        {
            Console.WriteLine("Please insert 'Model ID' (int)");
            modelId = int.Parse(Console.ReadLine());
        }
        else
        {
            modelId = defaultModel;
        }

        Console.WriteLine("Please insert 'Meter Number' (string)");
        meterNumber = Console.ReadLine();

        Console.WriteLine("Please insert 'Meter Firmware Version' (string)");
        meterFirmwareVersion = Console.ReadLine();

        Console.WriteLine("Please insert 'Switch State' (int, (0= Disconnected, 1= Connected, 2= Armed))");
        switchState = int.Parse(Console.ReadLine());

        var createdEndpoint = _endpointService.CreateEndpoint(serialNumber, modelId, meterNumber, meterFirmwareVersion, switchState);
        Console.WriteLine("Endpoint created: ");
        Console.WriteLine(createdEndpoint.ToString());

        MainMenu();
    }

    private void ListEndpointsOption()
    {
        foreach (var endpoint in _endpointService.ListEndpoints())
            Console.WriteLine(endpoint.ToString());

        MainMenu();
    }

    private void FindEndpointOption()
    {
        Console.WriteLine("Enter 'Endpoint Serial Number'");

        string serialNumber = Console.ReadLine();
        var endpoint = _endpointService.FindEndpoint(serialNumber);

        Console.WriteLine("Endpoint found: ");
        Console.WriteLine(endpoint.ToString());

        MainMenu();
    }

    private void DeleteEndpointOption()
    {
        Console.WriteLine("Enter 'Endpoint Serial Number'");

        string serialNumber = Console.ReadLine();
        _endpointService.DeleteEndpoint(serialNumber);

        Console.WriteLine("Endpoint Deleted");

        MainMenu();
    }

    private void EditEndpointOption()
    {
        Console.WriteLine("Enter 'Endpoint Serial Number'");

        string serialNumber = Console.ReadLine();
        var endpoint = _endpointService.FindEndpoint(serialNumber);

        Console.WriteLine("Please select the desired 'Switch State' (int, (0= Disconnected, 1= Connected, 2= Armed))");
        int switchState = int.Parse(Console.ReadLine());
        var editedEndpoint = _endpointService.EditEndpoint(serialNumber, switchState);

        Console.WriteLine("Switch state changed for endpoint:");
        Console.WriteLine(editedEndpoint.ToString());

        MainMenu();
    }

    private void ExitOption()
    {
        Console.WriteLine("Are you sure? Y/N");
        var option = Console.ReadLine();
        if (option.ToLower().Equals("y"))
        {
            Environment.Exit(0);
        }else if (!option.ToLower().Equals("n"))
        {
            Console.WriteLine("Invalid Option");
        }
        MainMenu();
    }

    private Dictionary<string, Action> GetAvailableActions()
    {
        return new() {
            {"1", InsertEndpointOption },
            {"2", EditEndpointOption },
            {"3", DeleteEndpointOption },
            {"4", ListEndpointsOption },
            {"5", FindEndpointOption },
            {"6", ExitOption }
        };
    }

}
