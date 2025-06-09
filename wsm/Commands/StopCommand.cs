using System.ServiceProcess;
using Cocona;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

public abstract class StopCommand
{
    [Command("stop", Description = "Stops a service.")]
    public async Task Stop(
        [Argument(Description = "Name of the service.")]
        string name,
        [Option("n", Description = "Do not wait for the service to stop.")]
        bool noWait = false
    )
    {
        Service? service = ServiceRepository.GetServiceByName(name);

        if (service == null)
        {
            Console.WriteLine("Service not found.");
            return;
        }

        if (service.Status == ServiceControllerStatus.Stopped)
        {
            Console.WriteLine("Service is already stopped.");
            return;
        }

        try
        {
            // Stop the service
            service.Stop(!noWait);

            if (noWait)
            {
                Console.WriteLine("Stopping service in background...");
            }
            else
            {
                service.Refresh();
                Console.WriteLine(service.Status == ServiceControllerStatus.Stopped
                    ? "Service stopped successfully."
                    : "Failed to stop service.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping service: {ex.Message}");
        }
    }
}