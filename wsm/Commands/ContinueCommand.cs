using System.ServiceProcess;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

[Command("continue", Description = "Continues a paused service.")]
public class ContinueCommand : ICommand
{
    [CommandParameter(0, Description = "The name of the service to resume.")]
    public required string Name { get; init; }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        Service? service = ServiceRepository.GetServiceByName(Name);

        if (service != null)
        {
            if (service.Status != ServiceControllerStatus.Paused)
            {
                console.Output.WriteLine("Service is not paused.");
                return;
            }

            service.Continue(true);
            service.Refresh();

            if (service.Status == ServiceControllerStatus.Running)
            {
                console.Output.WriteLine("Service continued successfully.");
            }
            else
            {
                console.Output.WriteLine("Failed to continue service.");
            }
        }
        else
        {
            console.Output.WriteLine("Service not found.");
        }
    }
}