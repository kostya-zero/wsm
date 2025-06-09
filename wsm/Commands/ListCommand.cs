using Cocona;
using wsm.Repositories;
using wsm.Models;

namespace wsm.Commands;

public abstract class ListCommand
{
    [Command("list", Description = "List all services.")]
    public void List(
        [Option("sort", Description = "Sort options. Available options are: displayName, serviceName, status.")]
        string sort = "displayName",
        [Option("reverse", Description = "Reverse the order of the list.")]
        bool reverseOrder = false
    )
    {
        Service[] services = ServiceRepository.GetAllServices().ToArray();

        switch (sort)
        {
            case "displayName":
                Array.Sort(services,
                    (x, y) => string.Compare(x.DisplayName, y.DisplayName, StringComparison.OrdinalIgnoreCase));
                break;
            case "serviceName":
                Array.Sort(services,
                    (x, y) => string.Compare(x.ServiceName, y.ServiceName, StringComparison.OrdinalIgnoreCase));
                break;
            case "status":
                Array.Sort(services,
                    (x, y) => string.Compare(x.Status.ToString(), y.Status.ToString(),
                        StringComparison.OrdinalIgnoreCase));
                break;
            default:
                Console.WriteLine(
                    "Invalid sort option. Available options are: displayName, serviceName, status.");
                return;
        }

        if (reverseOrder)
        {
            Array.Reverse(services);
        }

        int maxDisplayNameLength = services.Max(s => s.DisplayName.Length) + 2;
        int maxServiceNameLength = services.Max(s => s.ServiceName.Length) + 2;
        int maxStatusLength = services.Max(s => s.Status.ToString().Length) + 2;

        Console.WriteLine("\e[1m\e[97mDisplay Name" +
                          new string(' ', maxDisplayNameLength - "Display Name".Length) +
                          "Service Name" + new string(' ', maxServiceNameLength - "Service Name".Length) +
                          "Status\e[0m");
        foreach (var service in services)
        {
            Console.WriteLine(
                service.DisplayName + new string(' ', maxDisplayNameLength - service.DisplayName.Length) +
                service.ServiceName + new string(' ', maxServiceNameLength - service.ServiceName.Length) +
                service.Status + new string(' ', maxStatusLength - service.Status.ToString().Length)
            );
        }
    }
}