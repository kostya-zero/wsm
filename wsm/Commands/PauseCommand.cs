using System.ServiceProcess;
using Cocona;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

public abstract class PauseCommand
{
    [Command("pause", Description = "Pauses a service.")]
    public void Pause(
        [Argument(Description = "The name of the service to pause.")]
        string name,
        [Option("n", Description = "Do not wait for the service to start.")]
        bool noWait = false
    )
    {
        Service? service = ServiceRepository.GetServiceByName(name);
        if (service == null)
        {
            Console.WriteLine("Service not found.");
            return;
        }

        if (service.Status != ServiceControllerStatus.Running)
        {
            Console.WriteLine("Service is not running.");
            return;
        }

        if (service.Status == ServiceControllerStatus.Paused)
        {
            Console.WriteLine("Service is already paused.");
            return;
        }

        try
        {
            service.Pause(!noWait);
            if (noWait)
            {
                Console.WriteLine("Pausing service in the background...");
            }
            else
            {
                service.Refresh();
                Console.WriteLine(service.Status == ServiceControllerStatus.Paused
                    ? "Service paused successfully."
                    : "Failed to pause service.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured: " + ex.Message);
            Environment.Exit(1);
        }
    }
}