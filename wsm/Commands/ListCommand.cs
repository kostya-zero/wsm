using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;


[Command("list", Description = "List all services with their status.")]
public class ListCommand : ICommand
{
    [CommandOption("sort", 's', Description = "Sort options. Available options are: displayName, serviceName, status.")]
    public string Sort { get; set; } = "displayName";

    [CommandOption("reverse", 'r', Description = "Reverse the order of the list.")]
    public bool ReverseOrder { get; set; } = false;
    public async ValueTask ExecuteAsync(IConsole console)
    {
        Service[] services = ServiceRepository.GetAllServices().ToArray();

        switch (Sort)
        {
            case "displayName":
                Array.Sort(services, (x, y) => string.Compare(x.DisplayName, y.DisplayName, StringComparison.OrdinalIgnoreCase));
                break;
            case "serviceName":
                Array.Sort(services, (x, y) => string.Compare(x.ServiceName, y.ServiceName, StringComparison.OrdinalIgnoreCase));
                break;
            case "status":
                Array.Sort(services, (x, y) => string.Compare(x.Status.ToString(), y.Status.ToString(), StringComparison.OrdinalIgnoreCase));
                break;
            default:
                console.Output.WriteLine("Invalid sort option. Available options are: displayName, serviceName, status.");
                return;
        }

        if (ReverseOrder)
        {
            Array.Reverse(services);
        }

        int maxDisplayNameLength = services.Max(s => s.DisplayName.Length) + 2;
        int maxServiceNameLength = services.Max(s => s.ServiceName.Length) + 2;
        int maxStatusLength = services.Max(s => s.Status.ToString().Length) + 2;

        console.Output.WriteLine("\e[1m\e[97mDisplay Name" + new string(' ', maxDisplayNameLength - "Display Name".Length) +
                          "Service Name" + new string(' ', maxServiceNameLength - "Service Name".Length) +
                          "Status\e[0m");
        foreach (var service in services)
        {
            console.Output.WriteLine(
                service.DisplayName + new string(' ', maxDisplayNameLength - service.DisplayName.Length) +
                service.ServiceName + new string(' ', maxServiceNameLength - service.ServiceName.Length) +
                service.Status + new string(' ', maxStatusLength - service.Status.ToString().Length)
            );
        }
    }
}