using System.ServiceProcess;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

[Command("start", Description = "Starts a service.")]
public class StartCommand : ICommand
{
    [CommandParameter(0, Description = "The name of the service to start.")]
    public required string Name { get; init; }
    public async ValueTask ExecuteAsync(IConsole console)
    {
        Service? service = ServiceRepository.GetServiceByName(Name);
        if (service != null)
        {
            // Console.WriteLine("Trying to start service: " + service.DisplayName);
            service.Start();
            service.Refresh();
            if (service.Status == ServiceControllerStatus.Running)
            {
                console.Output.WriteLine("Service started successfully.");
            }
        }
        else
        {
            console.Output.WriteLine("Service not found.");
        }
    }
}
