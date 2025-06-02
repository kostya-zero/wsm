using System.ServiceProcess;
using Cocona;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

public class RestartCommand
{
    [Command("restart", Description = "Restarts a service.")]
    public void Restart(
        [Argument(Description = "The name of the service to restart.")]
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

        if (service.Status == ServiceControllerStatus.Stopped)
        {
            Console.WriteLine("Service is not running.");
            return;
        }

        try
        {
            service.Restart(!noWait);
            if (noWait)
            {
                Console.WriteLine("Restarting service in the background...");
            }
            else
            {
                service.Refresh();
                if (service.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("Service restarted successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to restart service.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured: " + ex.Message);
            Environment.Exit(1);
        }
    }
}