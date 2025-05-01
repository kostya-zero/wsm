using System.ServiceProcess;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

[Command("pause", Description = "Pauses a service.")]
public class PauseCommand : ICommand
{
    [CommandParameter(0, Description = "The name of the service to pause.")]
    public required string Name { get; init; }
    
    public async ValueTask ExecuteAsync(IConsole console)
    {
        Service? service = ServiceRepository.GetServiceByName(Name);

        if (service != null)
        {
            if (service.Status != ServiceControllerStatus.Running)
            {
                console.Output.WriteLine("Service is not running.");
                return;
            }

            if (service.Status == ServiceControllerStatus.Paused)
            {
                console.Output.WriteLine("Service is already paused.");
                return;
            }

            service.Pause(true);
            service.Refresh();
            
            if (service.Status == ServiceControllerStatus.Paused)
            {
                console.Output.WriteLine("Service paused successfully.");
            }
            else
            {
                console.Output.WriteLine("Failed to pause service.");
            }
        }
        else
        {
            console.Output.WriteLine("Service not found.");
        }
    }
}