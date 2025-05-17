using System.ServiceProcess;
using Cocona;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

public class StartCommand
{
    [Command("start", Description = "Starts a service.")]
    public void Start(
        [Argument(Description = "Name of the service.")]
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

        if (service.Status == ServiceControllerStatus.Running)
        {
            Console.WriteLine("Service is already running.");
            return;
        }

        // Console.WriteLine("Trying to start service: " + service.DisplayName);
        try
        {
            service.Start(!noWait);
            if (noWait)
            {
                Console.WriteLine("Starting service in the background...");
            }
            else
            {
                service.Refresh();
                if (service.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("Service started successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to start service.");
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