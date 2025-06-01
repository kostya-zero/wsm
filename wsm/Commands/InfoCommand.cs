using Cocona;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

public class InfoCommand
{
    [Command("info", Description = "Display information about the application.")]
    public void Info([Argument(Description = "The name of the service.")] string name)
    {
        Service? services = ServiceRepository.GetServiceByName(name);
        if (services == null)
        {
            Console.WriteLine("Service not found.");
            return;
        }
        
        Console.WriteLine("\e[1m\e[97mService Information\e[0m");
        Console.WriteLine($"Display Name: {services.DisplayName}");
        Console.WriteLine($"Service Name: {services.ServiceName}");
        Console.WriteLine($"Status: {services.Status}");
        Console.WriteLine($"Start Type: {services.StartType}");
    }
}