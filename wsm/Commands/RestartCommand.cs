using System.ServiceProcess;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

[Command("restart", Description = "Restarts a service.")]
public class RestartCommand : ICommand
{
    [CommandParameter(0, Description = "The name of the service to restart.")]
    public required string Name { get; init; }
    public async ValueTask ExecuteAsync(IConsole console)
    {
        Service? service = ServiceRepository.GetServiceByName(Name);

        if (service != null)
        {
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                console.Output.WriteLine("Service is not running.");
                return;
            }

            // Console.WriteLine("Trying to start service: " + service.DisplayName);
            service.Restart(true);
            service.Refresh();
            if (service.Status == ServiceControllerStatus.Running)
            {
                console.Output.WriteLine("Service restarted successfully.");
            }
        }
        else
        {
            console.Output.WriteLine("Service not found.");
        }
    }
}
