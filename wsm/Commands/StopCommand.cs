using System.ServiceProcess;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

[Command("stop", Description = "Stops a service.")]
public class StopCommand : ICommand
{
    [CommandParameter(0, Description = "The name of the service to stop.")]
    public required string Name { get; init; }
    public async ValueTask ExecuteAsync(IConsole console)
    {
        Service? service = ServiceRepository.GetServiceByName(Name);
        if (service != null)
        {
            // Console.WriteLine("Trying to stop service: " + service.DisplayName);
            service.Stop();
            service.Refresh();
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                console.Output.WriteLine("Service stopped successfully.");
            }
        }
        else
        {
            console.Output.WriteLine("Service not found.");
        }
    }
}