using System.ServiceProcess;
using Cocona;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

public abstract class ResumeCommand
{
    [Command("resume", Description = "Resume a paused service.")]
    public void Resume(
        [Argument(Description = "The name of the service to resume.")]
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

        if (service.Status != ServiceControllerStatus.Paused)
        {
            Console.WriteLine("Service is not paused.");
            return;
        }

        try
        {
            service.Resume(!noWait);
            if (noWait)
            {
                Console.WriteLine("Resuming service in the background...");
            }
            else
            {
                service.Refresh();
                if (service.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("Service resumed successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to resume service.");
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