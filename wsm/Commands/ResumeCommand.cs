using System.ServiceProcess;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

[Command("resume", Description = "Resume a paused service.")]
public class ResumeCommand : ICommand
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
                await console.Output.WriteLineAsync("Service is not paused.");
                return;
            }

            try
            {
                service.Resume(true);
            }
            catch (Exception ex)
            {
                await console.Output.WriteLineAsync("Error occured: " + ex.Message);   
                Environment.Exit(1);
            }
            
            service.Refresh();

            if (service.Status == ServiceControllerStatus.Running)
            {
                await console.Output.WriteLineAsync("Service resumed successfully.");
            }
            else
            {
                await console.Output.WriteLineAsync("Failed to continue service.");
            }
        }
        else
        {
            await console.Output.WriteLineAsync("Service not found.");
        }
    }
}