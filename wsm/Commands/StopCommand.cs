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

    [CommandOption("no-wait", 'n', Description = "Do not wait for the service to stop.")]
    public bool NoWait { get; set; } = false;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        Service? service = ServiceRepository.GetServiceByName(Name);

        if (service != null)
        {
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                console.Output.WriteLine("Service is already stopped.");
                return;
            }

            var previousStatus = service.Status;
            service.Stop(!NoWait);
            if (!NoWait)
            {
                const int maxRetries = 10; // Maximum retries (50 * 280ms = 14 seconds)
                int retryCount = 0;

                while (service.Status != ServiceControllerStatus.Stopped && retryCount < maxRetries)
                {
                    await Task.Delay(1000);
                    service.Refresh();
                    retryCount++;

                    if (retryCount > 1 && service.Status == previousStatus)
                    {
                        console.Output.WriteLine("Looks like the service is stuck. Please check the service status manually or try again later.");
                        break;
                    }
                    previousStatus = service.Status;
                }

                if (retryCount >= maxRetries)
                {
                    console.Output.WriteLine("Service stop operation timed out. Please check the service status manually.");
                }

                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    console.Output.WriteLine($"Service '{service.DisplayName}' stopped successfully.");
                }
                return;
            }
            else
            {
                console.Output.WriteLine("Stopping service in background...");
            }
        }
        else
        {
            console.Output.WriteLine("Service not found.");
        }
    }
}